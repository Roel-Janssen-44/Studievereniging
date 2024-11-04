using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;
using Studievereniging.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Studievereniging.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationData _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // In-memory lijst om suggesties tijdelijk op te slaan
        private static List<string> ActivitySuggestions = new List<string>();

        public ActivitiesController(ApplicationData context)
        public ActivitiesController(ApplicationData context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var activities = await _context.Activities
                .Include(a => a.Organisers)
                .Include(a => a.Participants)
                .ToListAsync();
            return View(activities);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Organisers)
                .Include(a => a.Participants)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Suggestions
        public IActionResult Suggestions()
        {
            return View(ActivitySuggestions);
        }

        // GET: Activities/SuggestActivity
        public IActionResult SuggestActivity()
        {
            return View();
        }

        // POST: Activities/SuggestActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuggestActivity(string suggestionText)
        {
            if (string.IsNullOrWhiteSpace(suggestionText))
            {
                return BadRequest("Suggestion cannot be empty.");
            }

            // Voeg de suggestie toe aan de in-memory lijst
            ActivitySuggestions.Add(suggestionText);

            return RedirectToAction(nameof(Suggestions));
        }

        public async Task<IActionResult> AddOrganiser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            // Add your organizer logic here
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,StartTime,EndTime,Location,MaxParticipants")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                activity.AdminId = currentUser?.Id;  // Use Id instead of converting to int
                activity.Admin = currentUser;
                // ... rest of the method
            }
            return View(activity);
        }

        // Update other methods similarly
    }
}
