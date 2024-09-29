using SamsysDemo.Infrastructure.Helpers;
using SamsysDemo.Infrastructure.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamsysDemo.BLL.Validators
{
    public static class ClientValidator
    {
        public static MessagingHelper<ClientDTO> IsValidClientToCreate(CreateClientDTO clientToCreate)
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
    }
}
