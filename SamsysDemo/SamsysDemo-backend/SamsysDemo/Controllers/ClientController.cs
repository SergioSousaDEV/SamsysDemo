using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamsysDemo.BLL.Services;
using SamsysDemo.Infrastructure.Helpers;
using SamsysDemo.Infrastructure.Models.Client;

namespace SamsysDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }


        [HttpGet("{id}")]
        public async Task<MessagingHelper<ClientDTO>> Get(long id)
        {
            return await _clientService.Get(id);
        }

        [HttpGet]
        public async Task<ActionResult<MessagingHelper<PaginatedList<ClientDTO>>>> GetAllPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber > 0 && pageSize > 0)
            {

                MessagingHelper<PaginatedList<ClientDTO>> result = await _clientService.GetAllPaginated(pageNumber, pageSize);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            else
            {
                return BadRequest("Page number and Page size must be higher than 0");
            }
        }


        [HttpPost]
        public async Task<ActionResult<MessagingHelper<ClientDTO>>> CreateClient(CreateClientDTO clientToCreateDTO)
        {
            //I think it's a good approach to validate the state of the DTO before sending to the BLL
            if(clientToCreateDTO != null)
            {
                MessagingHelper<ClientDTO> result = await _clientService.CreateClient(clientToCreateDTO);
                if (result.Success)
                {
                    //Return a 201 Created with the ID of this new Client
                    return CreatedAtAction(nameof(Get), new { id = result.Obj.Id }, result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest("Client data is null");
            }
        }
        [HttpPut("{id}")]
        public async Task<MessagingHelper> Update(int id, UpdateClientDTO clientToUpdateDTO)
        {
            return await _clientService.Update(id, clientToUpdateDTO);
        }

        [HttpPost("{id}/[action]")]
        public async Task<MessagingHelper> Enable(long id)
        {
            return await _clientService.EnableClient(id);
        }

        [HttpPost("{id}/[action]")]
        public async Task<MessagingHelper> Disable(long id)
        {
            return await _clientService.DisableClient(id);
        }
    }
}
