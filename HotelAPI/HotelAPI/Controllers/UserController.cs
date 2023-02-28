using AutoMapper;
using HotelAPI.Models;
using HotelAPI.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly HotelContext db;

        private readonly UserValidator validator = new();

        public UserController(ILogger<RoomTypeController> logger, HotelContext db)
        {
            this._logger = logger;
            this.db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                _logger.LogInformation(identity.FindFirst("role").Value);

            }
            if (db.users == null) return NotFound();
            List<User> users = await db.users.ToListAsync();
            List<UserDTO> userDTOs = db.userToDTO().Map<List<User>, List<UserDTO>>(users);
            return userDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (db.users == null) return NotFound();
            var user = await db.users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound();
            return db.userToDTO().Map<UserDTO>(user);
        }

        [HttpPost]

        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var validatorResult = validator.Validate(userDTO);
            if (!validatorResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validatorResult.Errors);
            }
            if (db.users == null) return Problem("Entity set 'HotelContext.Users'  is null.");
            var user = db.DTOToUser().Map<User>(userDTO);
            db.users.Add(user);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            var validatorResult = validator.Validate(userDTO);
            if (!validatorResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validatorResult.Errors);
            }
            var user = await db.users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound();
            user.Name = userDTO.Name;
            user.Phone = userDTO.Phone;
            user.IdentificationCard = userDTO.IdentificationCard;
            user.Gmail = userDTO.Gmail;

            try
            {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException ex)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteUser(int id)
        { 
            if(db.users == null)
            {
                return NotFound();
            }
            var user = await db.users.FirstOrDefaultAsync(x => x.Id == id);

            if(user == null) return NotFound();

            db.users.Remove(user);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
