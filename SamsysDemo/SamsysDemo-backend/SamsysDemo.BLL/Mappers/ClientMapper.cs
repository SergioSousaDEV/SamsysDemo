using SamsysDemo.Infrastructure.Entities;
using SamsysDemo.Infrastructure.Helpers;
using SamsysDemo.Infrastructure.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamsysDemo.BLL.Mappers
{
    public static class ClientMapper
    {        

        public static PaginatedList<ClientDTO> Map(this List<Client>? clients, int totalRecords, int pageNumber, int pageSize)
        {
            PaginatedList<ClientDTO> result = new PaginatedList<ClientDTO>();
            result.TotalRecords = totalRecords;
            result.CurrentPage = pageNumber;
            result.PageSize = pageSize;

            if (clients != null)
            {
                foreach (Client clientBO in clients)
                {
                    result.Items.Add(clientBO.Map());
                }
            }
            return result;
        }

        public static ClientDTO Map(this Client client)
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
