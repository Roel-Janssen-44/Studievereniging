using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;
using Studievereniging.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace Studievereniging.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationData _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(ApplicationData context, UserManager<ApplicationUser> userManager, ILogger<ActivitiesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // Method to get the current logged-in user's ID
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
                AvailableOrganizers = await GetAvailableOrganizers(),
                Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name")
            };

            return View(viewModel);
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Location,MaxParticipants,Price,RegistrationDeadline,Category,Image,IsPublic")] Activity activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the current user as admin
                    var admin = await _userManager.GetUserAsync(User);
                    if (admin == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        ViewBag.Categories = await GetCategories();
                        return View(activity);
                    }

                    activity.AdminId = admin.Id;
                    activity.Admin = admin;

                    _context.Add(activity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // If we got this far, something failed, log the errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"Activity creation error: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating activity: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while creating the activity.");
            }

            // Repopulate categories before returning
            ViewBag.Categories = await GetCategories();
            return View(activity);
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

                    // Update activity properties
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

                    // Update organizers
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

            if (activity.Participants.Any(p => p.Id == user.Id))
            {
                TempData["Error"] = "You are already participating in this activity";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

            if (activity.MaxParticipants.HasValue && activity.Participants.Count >= activity.MaxParticipants.Value)
            {
                TempData["Error"] = "This activity is full";
                return RedirectToAction(nameof(Details), new { id = activity.Id });
            }

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
        [Authorize(Roles = $"{Role.Member},{Role.BoardMember},{Role.Admin}")]
        public IActionResult SuggestActivity()
        {
            return View();
        }

        // POST: Activities/SuggestActivity
        [HttpPost]
        [Authorize(Roles = $"{Role.Member},{Role.BoardMember},{Role.Admin}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuggestActivity(string suggestionText)
        {
            if (string.IsNullOrWhiteSpace(suggestionText))
            {
                ModelState.AddModelError(string.Empty, "Alle velden zijn verplicht.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var suggestion = new Suggestions
            {
                Name = user.Name,
                Email = user.Email,
                Text = suggestionText,
                CreatedAt = DateTime.Now,
                CreatedById = user.Id
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
                .Where(a => a.EndDate < DateTime.Now)
                .OrderByDescending(a => a.EndDate)
                .ToListAsync();

            return Ok(pastActivities);
        }

        // Add this action to handle quick registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickRegistration([FromBody] QuickRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activities.FindAsync(model.ActivityId);
            if (activity == null)
            {
                return NotFound();
            }

            // Generate a random password
            var password = GenerateRandomPassword();
            
            // Create new user
            var user = new ApplicationUser
            {
                UserName = model.Name.Replace(" ", "").ToLower(), // Simple username generation
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Add to Member role
                await _userManager.AddToRoleAsync(user, Role.Member);
                
                // Add user to activity participants
                activity.Participants.Add(user);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    username = user.UserName,
                    password = password
                });
            }

            return BadRequest(result.Errors);
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Naam is verplicht");
            }

            try
            {
                var category = new Category { Name = name };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return Json(new { id = category.Id, name = category.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Er is een fout opgetreden");
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            });
        }

    }
}