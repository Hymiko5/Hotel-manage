using AutoMapper;
using Baseline;
using HotelAPI.Extensions;
using HotelAPI.Models;
using IdentityServer4.Models;
using ImTools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly HotelContext db;
        private readonly ElasticClient _client;
        private ISearchResponse<RoomType> searchResponse;

        public RoomTypeController(ILogger<RoomTypeController> logger, HotelContext db, ElasticClient client)
        {
            this._logger = logger;
            this.db = db;
            this._client = client;
            //_logger.LogInformation(_client.ToString());
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<IEnumerable<RoomTypeDTO>>> GetRoomTypes()
        {
            if (db.users == null) return NotFound();
            List<RoomType> roomTypes = await db.roomTypes.ToListAsync();
            List<RoomTypeDTO> roomTypeDTOs = db.roomTypeToDTO().Map<List<RoomType>, List<RoomTypeDTO>>(roomTypes);
            return roomTypeDTOs;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<RoomTypeDTO>> GetRoomType(int id)
        {
            if (db.roomTypes == null) return NotFound();
            var roomType = await db.roomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (roomType == null) return NotFound();
            return db.roomTypeToDTO().Map<RoomTypeDTO>(roomType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomTypeDTO>> CreateRoomType(RoomTypeDTO roomTypeDTO)
        {
            string INDEX_NAME = "hoteldb";
            if (db.roomTypes == null) return Problem("Entity set 'HotelContext.RoomTypes' is null.");
            var roomType = db.DTOToRoomType().Map<RoomType>(roomTypeDTO);
            db.roomTypes.Add(roomType);
            await db.SaveChangesAsync();
            ElasticSearchExtensions.CreateRoomTypeDocument(_client, INDEX_NAME, roomType, roomType.Id.ToString());
            return CreatedAtAction(nameof(GetRoomType), new { id = roomType.Id }, roomType);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoomType(int id, RoomTypeDTO roomTypeDTO)
        {
            var roomType = await db.roomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (roomType == null) return NotFound();

            roomType.TypeName = roomTypeDTO.TypeName;
            roomType.Description = roomTypeDTO.Description;
            roomType.Totals = roomTypeDTO.Totals;

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

        public async Task<IActionResult> DeleteRoomType(int id)
        {
            if (db.roomTypes == null)
            {
                return NotFound();
            }
            var roomType = await db.roomTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (roomType == null) return NotFound();

            db.roomTypes.Remove(roomType);
            await db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("/search")]
        public async Task<IActionResult> SearchRoomType([FromBody] string keywords)
        {
            if (keywords.IsEmpty()) return BadRequest();
            string INDEX_NAME = "hoteldb";
            searchResponse = await ElasticSearchExtensions.GetRoomTypeDocumentByName(_client, INDEX_NAME, keywords);
            if(searchResponse == null) return NotFound();
            else if(!searchResponse.IsValid) return BadRequest();
            return Ok(ElasticSearchExtensions.ToRoomTypeList(searchResponse));
        }

        

    }
}
