using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
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
                    IsPublic = viewModel.IsPublic
                };

                // Add organizers
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        [HttpPost]
        public async Task<IActionResult> JoinActivity(int id)
        {
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
                return BadRequest("You are already participating in this activity");
            }

            // Check if activity is full
            if (activity.MaxParticipants.HasValue && activity.Participants.Count >= activity.MaxParticipants.Value)
            {
                return BadRequest("This activity is full");
            }

            // Check if registration deadline has passed
            if (activity.RegistrationDeadline.HasValue && activity.RegistrationDeadline.Value < DateTime.Now)
            {
                return BadRequest("Registration deadline has passed");
            }

            activity.Participants.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = activity.Id });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveActivity(int id)
        {
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
                return BadRequest("You are not participating in this activity");
            }

            activity.Participants.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = activity.Id });
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities
                .Include(a => a.Participants)
                .Include(a => a.Organisers)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (activity == null)
            {
                return NotFound();
            }

            // Clear relationships first
            activity.Participants.Clear();
            activity.Organisers.Clear();
            
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
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
