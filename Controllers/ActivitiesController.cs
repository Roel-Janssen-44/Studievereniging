using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivitiesController(ApplicationData context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Methode om het huidige ingelogde gebruikers-ID te krijgen
        private async Task<string> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
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

        // GET: Activities/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ActivityViewModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                AvailableOrganizers = await GetAvailableOrganizers()
            };

            return View(viewModel);
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var activity = new Activity
                {
                    Name = viewModel.Name,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    Location = viewModel.Location,
                    MaxParticipants = viewModel.MaxParticipants,
                    Price = viewModel.Price,
                    RegistrationDeadline = viewModel.RegistrationDeadline,
                    Category = viewModel.Category,
                    Image = viewModel.Image,
                    IsPublic = viewModel.IsPublic,
                    AdminId = currentUser?.Id,
                    Admin = currentUser
                };

                // Voeg organisatoren toe
                if (viewModel.SelectedOrganizerIds != null)
                {
                    var organizers = await _context.Users
                        .Where(u => viewModel.SelectedOrganizerIds.Contains(u.Id))
                        .ToListAsync();
                    activity.Organisers = organizers;
                }

                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableOrganizers = await GetAvailableOrganizers();
            return View(viewModel);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Organisers)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            var viewModel = new ActivityViewModel
            {
                Id = activity.Id,
                Name = activity.Name,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                Location = activity.Location,
                MaxParticipants = activity.MaxParticipants,
                Price = activity.Price,
                RegistrationDeadline = activity.RegistrationDeadline,
                Category = activity.Category,
                Image = activity.Image,
                IsPublic = activity.IsPublic,
                SelectedOrganizerIds = activity.Organisers.Select(o => o.Id).ToList(),
                AvailableOrganizers = await GetAvailableOrganizers()
            };

            return View(viewModel);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activity = await _context.Activities
                        .Include(a => a.Organisers)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (activity == null)
                    {
                        return NotFound();
                    }

                    // Update de eigenschappen van de activiteit
                    activity.Name = viewModel.Name;
                    activity.StartDate = viewModel.StartDate;
                    activity.EndDate = viewModel.EndDate;
                    activity.Location = viewModel.Location;
                    activity.MaxParticipants = viewModel.MaxParticipants;
                    activity.Price = viewModel.Price;
                    activity.RegistrationDeadline = viewModel.RegistrationDeadline;
                    activity.Category = viewModel.Category;
                    activity.Image = viewModel.Image;
                    activity.IsPublic = viewModel.IsPublic;

                    // Update organisatoren
                    activity.Organisers.Clear();
                    if (viewModel.SelectedOrganizerIds != null)
                    {
                        var organizers = await _context.Users
                            .Where(u => viewModel.SelectedOrganizerIds.Contains(u.Id))
                            .ToListAsync();
                        activity.Organisers = organizers;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.AvailableOrganizers = await GetAvailableOrganizers();
            return View(viewModel);
        }

        // POST: Activities/JoinActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinActivity(int id)
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users", new { returnUrl = Url.Action("Details", "Activities", new { id }) });
            }

            var activity = await _context.Activities
                .Include(a => a.Participants)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if user is already a participant
            if (activity.Participants.Any(p => p.Id == user.Id))
            {
                TempData["Error"] = "You are already participating in this activity";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

            // Check if activity is full
            if (activity.MaxParticipants.HasValue && activity.Participants.Count >= activity.MaxParticipants.Value)
            {
                TempData["Error"] = "This activity is full";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

            // Check if registration deadline has passed
            if (activity.RegistrationDeadline.HasValue && DateTime.Now > activity.RegistrationDeadline.Value)
            {
                TempData["Error"] = "Registration deadline has passed";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

            activity.Participants.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Successfully joined the activity!";
            return RedirectToAction(nameof(Details), new { id = activity.Id });
        }

        // POST: Activities/LeaveActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveActivity(int id)
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }

            var activity = await _context.Activities
                .Include(a => a.Participants)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if user is a participant
            if (!activity.Participants.Any(p => p.Id == user.Id))
            {
                TempData["Error"] = "You are not participating in this activity";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

            activity.Participants.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Successfully left the activity";
            return RedirectToAction(nameof(Details), new { id = activity.Id });
        }

        // GET: Activities/Suggestions
        public async Task<IActionResult> Suggestions()
        {
            var suggestions = await _context.Suggestions
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(suggestions);
        }

        // GET: Activities/SuggestActivity
        public IActionResult SuggestActivity()
        {
            return View();
        }

        // POST: Activities/SuggestActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuggestActivity(string suggestionText)
        {
            if (string.IsNullOrWhiteSpace(suggestionText))
            {
                ModelState.AddModelError(string.Empty, "Suggestie mag niet leeg zijn.");
                return View();
            }

            // Sla de suggestie op in de database
            var suggestion = new Suggestions
            {
                Text = suggestionText
            };

            _context.Suggestions.Add(suggestion);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bedankt voor je suggestie!";
            return RedirectToAction(nameof(Suggestions));
        }

        private async Task<List<UserSelectViewModel>> GetAvailableOrganizers()
        {
            return await _context.Users
                .Select(u => new UserSelectViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Role = u.Role
                })
                .ToListAsync();
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        [HttpGet("api/activities/past")]
        public async Task<IActionResult> GetPastActivities()
        {
            var pastActivities = await _context.Activities
                .Where(a => a.EndDate < DateTime.Now) // Filter for past activities
                .OrderByDescending(a => a.EndDate) // Sort by most recent past activities
                .ToListAsync();

            return Ok(pastActivities);
        }

    }
}