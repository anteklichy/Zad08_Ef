using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zad08_Ef.Context;
using Zad08_Ef.DTOs;
using Zad08_Ef.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zad08_Ef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ApbdContext _context;

        public TripsController(ApbdContext context)
        {
            _context = context;
        }

        // GET: api/trips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            var trips = await _context.Trips
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.Client)
                .Include(t => t.CountryTrips)
                .ThenInclude(ct => ct.Country)
                .AsSplitQuery()
                .OrderByDescending(t => t.DateFrom)
                .Select(t => new TripDTO
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.CountryTrips.Select(ct => new CountryDTO
                    {
                        Name = ct.Country.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDTO
                    {
                        FirstName = ct.Client.FirstName,
                        LastName = ct.Client.LastName
                    }).ToList()
                })
                .ToListAsync();

            return Ok(trips);
        }


        // DELETE: api/clients/{idClient}
        [HttpDelete("clients/{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
            {
                return NotFound();
            }

            if (client.ClientTrips.Any())
            {
                return BadRequest("Client has assigned trips.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/trips/{idTrip}/clients
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, ClientDTO clientDto)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null)
            {
                return NotFound("Trip not found.");
            }

            var client = await _context.Clients
                .SingleOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (client == null)
            {
                client = new Client
                {
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Email = clientDto.Email,
                    Telephone = clientDto.Telephone,
                    Pesel = clientDto.Pesel
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var clientTripExists = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
            if (clientTripExists)
            {
                return BadRequest("Client already assigned to this trip.");
            }

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
