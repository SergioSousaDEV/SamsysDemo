using Microsoft.EntityFrameworkCore;
using SamsysDemo.BLL.Mappers;
using SamsysDemo.BLL.Validators;
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
        //Added Mapper to split the responsability of mapping data and logic
        //It will be necessary to be possible to implement unit testing 
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
                    response.Obj = clients.Map(totalRecords, pageNumber, pageSize);
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
                    PhoneNumber = client.PhoneNumber,
                    DateOfBirth = client.DateOfBirth
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
                response = ClientValidator.IsValidClientToCreate(clientToCreate);
                if (response.Success)
                {
                    //As a good practice, will be passed the CreateClientDTO to Client to be easier to manage if in future requires more fields
                    client = new Client(clientToCreate);
                    await _unitOfWork.ClientRepository.Insert(client);
                    await _unitOfWork.SaveAsync();
                    response.Success = true;
                    response.Obj = client.Map();
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
        
        
    }
}
