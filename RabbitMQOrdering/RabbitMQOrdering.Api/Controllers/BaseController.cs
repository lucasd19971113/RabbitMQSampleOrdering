using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared.Responses;

namespace RabbitMQOrdering.Api.Controllers
{
    [Produces ("application/json")]
    [Route ("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController<S, TModel> : Controller
    where S : IService<TModel>
    {
        protected readonly S service;


        public BaseController (S service) {
            this.service = service;
        }

        [HttpPost]
        [NonAction]
        public async Task<IActionResult> Create (TModel entity) {
            try {
                var result = await service.Add (entity);
                if (result) {

                    return Ok (entity);
                }

                return StatusCode (result.StatusCode, ErrorDto.Create (result.StatusCode, result.Error));
            } catch (Exception ex) {

                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }

        
        [HttpGet]
        [Route ("{id}")]
        public virtual async Task<IActionResult> Get (int id) {
            
            try {
             
                var result = await service.GetById (id);
                if (result) {
                    
                    return Ok (result.Value);
                }

                return StatusCode (result.StatusCode, ErrorDto.Create (result.StatusCode, result.Error));
                
            } catch (Exception ex) {

                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }
        

        [HttpGet]
        public virtual async Task<IActionResult> GetAll () {
            try {
                var result = await service.GetAll ();
                if (result) {
                    return Ok (result.Value);
                }

                return StatusCode (result.StatusCode, ErrorDto.Create (result.StatusCode, result.Error));
            } catch (Exception ex) {

                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }

        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> Delete (int id) {
            try {
                var result = await service.Delete (id);
                if (result) {
                    return Ok ();
                }

                return StatusCode (result.StatusCode, ErrorDto.Create (result.StatusCode, result.Error));
            } catch (Exception ex) {

                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }
    }
}