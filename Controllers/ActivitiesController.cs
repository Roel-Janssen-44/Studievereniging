using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;
using Studievereniging.Models.ViewModels;

namespace Studievereniging.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationData _context;

        // In-memory lijst om suggesties tijdelijk op te slaan
        private static List<string> ActivitySuggestions = new List<string>();

        public ActivitiesController(ApplicationData context)
        {
            _context = context;
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
    }
}
