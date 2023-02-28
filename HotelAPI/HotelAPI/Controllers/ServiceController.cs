using HotelAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly HotelContext db;

        public ServiceController(ILogger<ServiceController> logger, HotelContext db)
        {
            this._logger = logger;
            this.db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServices()
        {
            if (db.services == null) return NotFound();
            List<Service> services = await db.services.ToListAsync();
            List<ServiceDTO> serviceDTOs = db.serviceToDTO().Map<List<Service>, List<ServiceDTO>>(services);
            return serviceDTOs;
        }

        [HttpGet("{userID}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<List<ServiceDTO>>> GetService(int userID)
        {
            if (db.services == null) return NotFound();
            var service = await db.services.Where(x => x.UserID == userID).Select(x => new ServiceDTO { Name = x.Name, Description = x.Description, Price = x.Price, UserID = x.UserID }).ToListAsync();
            if (service == null) return NotFound();
            _logger.LogInformation(service.ToString());
            return service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> CreateService(ServiceDTO serviceDTO)
        {
            if (db.services == null) return Problem("Entity set 'HotelContext.Services'  is null.");
            var service = db.DTOToService().Map<Service>(serviceDTO);
            db.services.Add(service);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServices), new { id = service.UserID }, service);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateService(int id, ServiceDTO serviceDTO)
        {
            var service = await db.services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null) return NotFound();
            service.Name = serviceDTO.Name;
            service.Description = serviceDTO.Description;
            service.Price = serviceDTO.Price;
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteUser(int id)
        {
            if (db.services == null)
            {
                return NotFound();
            }
            var service = await db.services.FirstOrDefaultAsync(x => x.Id == id);

            if (service == null) return NotFound();

            db.services.Remove(service);
            await db.SaveChangesAsync();

            return NoContent();
        }


    }
}
