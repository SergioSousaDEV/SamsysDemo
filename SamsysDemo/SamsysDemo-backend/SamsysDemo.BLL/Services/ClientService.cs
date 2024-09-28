using Microsoft.EntityFrameworkCore;
using SamsysDemo.Infrastructure.Entities;
using SamsysDemo.Infrastructure.Helpers;
using SamsysDemo.Infrastructure.Interfaces.Repositories;
using SamsysDemo.Infrastructure.Models.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamsysDemo.BLL.Services
{
    //Interface for client service contracts
    //Will be usefull for implementation of unit testing
    public interface IClientService
    {
        Task<MessagingHelper<PaginatedList<ClientDTO>>> GetAllPaginated(int pageNumber, int pageSize);
        Task<MessagingHelper<ClientDTO>> Get(long id);
        Task<MessagingHelper<ClientDTO>> CreateClient(CreateClientDTO clientDTO);
        Task<MessagingHelper> Update(long id, UpdateClientDTO clientToUpdate);
        Task<MessagingHelper> EnableClient(long id);
        Task<MessagingHelper> DisableClient(long id);
    }

    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //It's a good practice to paginate the GetAll to improve performance and scalability
        //Will be used the PaginatedList Helper although wasn't mentioned to be used
        public async Task<MessagingHelper<PaginatedList<ClientDTO>>> GetAllPaginated(int pageNumber, int pageSize)
        {
            MessagingHelper<PaginatedList<ClientDTO>> response = new();           
            response.Success = true;
            try
            {
                int totalRecords = await _unitOfWork.ClientRepository.GetTotalRecordsCount();
                List<Client> clients = await _unitOfWork.ClientRepository.GetAllPaginated(pageNumber, pageSize);
                if (clients != null && clients.Count == 0)
                {
                    response.SetMessage($"Não existem clientes!");                 
                }
                else
                {                    
                    response.Obj = Map(clients, totalRecords, pageNumber, pageSize);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro inesperado ao obter o cliente.");
            }
            return response;
        }

        private PaginatedList<ClientDTO> Map(List<Client>? clients,int totalRecords, int pageNumber, int pageSize)
        {
            PaginatedList<ClientDTO> result = new PaginatedList<ClientDTO>();   
            result.TotalRecords = totalRecords;
            result.CurrentPage = pageNumber;
            result.PageSize = pageSize;

            if (clients != null)
            {
                foreach (Client clientBO in clients)
                {
                    result.Items.Add(Map(clientBO));
                }              
            }
            return result;
        }

        public async Task<MessagingHelper<ClientDTO>> Get(long id)
        {
            MessagingHelper<ClientDTO> response = new();
            try
            {
                Client? client = await _unitOfWork.ClientRepository.GetById(id);
                if (client is null)
                {
                    response.SetMessage($"O cliente não existe. | Id: {id}");
                    response.Success = false;
                    return response;
                }
                response.Obj = new ClientDTO
                {
                    Id = client.Id,
                    IsActive = client.IsActive,
                    ConcurrencyToken = Convert.ToBase64String(client.ConcurrencyToken),
                    Name = client.Name,
                    PhoneNumber = client.PhoneNumber
                };
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro inesperado ao obter o cliente.");
                return response;
            }
        }

        public async Task<MessagingHelper<ClientDTO>> CreateClient(CreateClientDTO clientToCreate)
        {
            MessagingHelper<ClientDTO> response = new();
            Client? client = null;

            try
            {
                response = IsValidClientToCreate(clientToCreate);
                if (response.Success)
                {
                    //As a good practice, will be passed the CreateClientDTO to Client to be easier to manage if in future requires more fields
                    client = new Client(clientToCreate);
                    await _unitOfWork.ClientRepository.Insert(client);
                    await _unitOfWork.SaveAsync();
                    response.Success = true;
                    response.Obj = Map(client);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro inesperado ao criar o cliente. Tente novamente.");
            }
            return response;
        }

        public async Task<MessagingHelper> Update(long id, UpdateClientDTO clientToUpdate)
        {
            MessagingHelper<Client> response = new();
            try
            {
                Client? client = await _unitOfWork.ClientRepository.GetById(id);
                if (client is null)
                {
                    response.SetMessage($"O cliente não existe. | Id: {id}");
                    response.Success = false;
                    return response;
                }
                client.Update(clientToUpdate.Name, clientToUpdate.PhoneNumber);
                _unitOfWork.ClientRepository.Update(client, clientToUpdate.ConcurrencyToken);
                await _unitOfWork.SaveAsync();
                response.Success = true;
                response.Obj = client;
                return response;
            }
            catch (DbUpdateConcurrencyException exce)
            {
                response.Success = false;
                response.SetMessage($"Os dados do cliente foram atualizados posteriormente por outro utilizador!.");
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro inesperado ao atualizar o cliente. Tente novamente.");
                return response;
            }
        }

        public async Task<MessagingHelper> DisableClient(long id)
        {
            MessagingHelper<Client> response = new();
            try
            {
                Client? client = await _unitOfWork.ClientRepository.GetById(id);
                if (client is null)
                {
                    response.SetMessage($"O cliente não existe. | Id: {id}");
                    response.Success = false;
                    return response;
                }
                client.SetStatus(false);
                _unitOfWork.ClientRepository.Update(client, Convert.ToBase64String(client.ConcurrencyToken));
                await _unitOfWork.SaveAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro inativar o cliente.");
                return response;
            }
        }
        public async Task<MessagingHelper> EnableClient(long id)
        {
            MessagingHelper<Client> response = new();
            try
            {
                Client? client = await _unitOfWork.ClientRepository.GetById(id);
                if (client is null)
                {
                    response.SetMessage($"O cliente não existe. | Id: {id}");
                    response.Success = false;
                    return response;
                }
                client.SetStatus(true);
                _unitOfWork.ClientRepository.Update(client, Convert.ToBase64String(client.ConcurrencyToken));
                await _unitOfWork.SaveAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Ocorreu um erro ativar o cliente.");
                return response;
            }
        }
        private MessagingHelper<ClientDTO> IsValidClientToCreate(CreateClientDTO clientToCreate)
        {
            //Will be considered an error if the specified date is later than today.

            MessagingHelper<ClientDTO> result = new MessagingHelper<ClientDTO>();
            result.Success = true;

            if (clientToCreate.DateOfBirth.HasValue && clientToCreate.DateOfBirth.Value > DateTime.UtcNow)
            {
                result.Success = false;
                result.SetMessage($"A data de nascimento indicada não é válida!.");
            }

            return result;
        }
        private ClientDTO Map(Client client)
        {
            return new ClientDTO
            {
                Id = client.Id,
                Name = client.Name,
                PhoneNumber = client.PhoneNumber,
                IsActive = client.IsActive,
                DateOfBirth = client.DateOfBirth,
                ConcurrencyToken = Convert.ToBase64String(client.ConcurrencyToken)

            };
        }
    }
}
