using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamsysDemo.Infrastructure.Models.Client
{
    //I will comment in English to maintain consistency with the codebase, although i considered to write in Portuguese, 
    //  because there are some comments written in Portuguese in some classes
    //  for example, (DAL\Repositories\UnitofWork.cs or Infrastructure\Helpers\PaginatedList).
    //
    //This DTO is going to be used only at the time of creating a client.
    //It will only have the necessary fields for creating a client and to separate the purposes of the DTOs (ClientDTO and CreateClientDTO),
    //  for example, the ID and ConcurrencyToken should only be included in the ClientDTO and not on CreateClientDTO.
    //
    //The ClientDTO represents a client that already exists in the database.
    //I will map the DateOfBirth field as a option field.        

    public class CreateClientDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }
}
