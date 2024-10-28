using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Studievereniging.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationData _context;

        public HomeController(ApplicationData context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get upcoming activities for the homepage
            var upcomingActivities = await _context.Activities
                .Include(a => a.Participants)
                .Where(a => a.StartDate > DateTime.Now)
                .OrderBy(a => a.StartDate)
                .Take(3)
                .ToListAsync();

            ViewBag.UpcomingActivities = upcomingActivities;
            return View();
        }

        [Route("/api/activities/upcoming")]
        public async Task<IActionResult> GetUpcomingActivities()
        {
            var activities = await _context.Activities
                .Include(a => a.Participants)
                .Where(a => a.StartDate > DateTime.Now)
                .OrderBy(a => a.StartDate)
                .Take(3)
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    startDate = a.StartDate,
                    location = a.Location,
                    participantCount = a.Participants.Count,
                    maxParticipants = a.MaxParticipants,
                    image = a.Image
                })
                .ToListAsync();

            return Json(activities);
        }

        // Nieuwe contact actie
        public IActionResult Contact()
        {
            // Je kunt eventueel ViewBag gebruiken voor extra gegevens op de pagina
            ViewBag.Message = "Neem contact met ons op voor meer informatie.";

            return View();
        }
    }
}