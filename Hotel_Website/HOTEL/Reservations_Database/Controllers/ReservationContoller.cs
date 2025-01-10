using Microsoft.AspNetCore.Mvc;
using Reservations_Database.Models;

namespace Reservations_Database.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationDbContext _context;

        public ReservationController(ReservationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create([FromForm] IFormCollection form)
        {
            try
            {
                // Map form fields to the Reservation model
                var reservation = new Reservation
                {
                    Name = form["name"],
                    SecondName = form["second-name"],
                    Surname = form["surname"],
                    Phone = form["phone"],
                    Email = form["mail"],
                    Arriving = ConvertToUtc(DateTime.Parse(form["arriving"])),
                    Leaving = ConvertToUtc(DateTime.Parse(form["leaving"])),
                    Room = form["room"],
                    Adults = int.TryParse(form["adults"], out int adults) ? adults : 0,
                    Children = int.TryParse(form["children"], out int children) ? children : 0,
                    Message = form["message"]
                };

                // Validate the model
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                // Ensure valid date range
                if (reservation.Arriving >= reservation.Leaving)
                {
                    return BadRequest("Leaving date must be after the arriving date.");
                }

                // Save the reservation
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("Success");
            }
            catch (FormatException ex)
            {
                // Handle date parsing errors
                Console.WriteLine($"Date parsing error: {ex.Message}");
                return BadRequest("Invalid date format.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult Success()
        {
            return Ok("Успешно направихте резервацията си!");
        }

        private DateTime ConvertToUtc(DateTime dateTime)
        {
            // Convert to UTC if not already
            return dateTime.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                : dateTime.ToUniversalTime();
        }
    }
}
