using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InteractiveCV.Data;
using InteractiveCV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InteractiveCV.Controllers
{
    //adding the ability to edit only to authorised users. In this case, logged in.
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProjectsController (ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context; // injecting DB
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }


        //GET: ViewProject/5
        [AllowAnonymous]
        public async Task<IActionResult> ViewProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(p => p.goals).Include(e => e.updates).SingleOrDefaultAsync(item => item.ID == id);

            if (project == null)
            {
                return NotFound();
            }

            if (!project.isActive && !_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(ShallNotPass));
            }

            return View(project);
        }

        //GET Projects/EditDescription/5
        public async Task<IActionResult> EditDescription (int id)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.ID == id);

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDescription ([Bind("ID, Title, imageUrl, Description, isActive, Color, Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //updating db
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!DatabaseExists(1, project.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewProject), new { id = project.ID });
            }
            return View(project);
        }

        //Get Projects/CreateGoal/5
        public ActionResult CreateGoal(int id)
        {
            ViewData["ProjectID"] = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGoal([Bind("projectGoalID, goalText, projectID")] ProjectGoal goal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProject), new { id = goal.projectID }); // redirecting back to the project view with the ID that the goal belongs to
            }
            return View(goal);
        }


        // GET: Projects/CreateUpdateEntry/5
        // controller/action/projectID/updateNumber
        public IActionResult CreateUpdateEntry(int id, int updateNumber)
        {
            ViewData["ProjectID"] = id;
            ViewData["UpdateNumber"] = updateNumber;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpdateEntry([Bind("projectUpdateID, updateText, projectID, updateNumber")] ProjectUpdate update)
        {
            if (ModelState.IsValid)
            {
                _context.Add(update);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProject), new { id = update.projectID });
            }
            return View(update);
        }

        //GET: Projects/EditGoal/5/2000
        // controller/action/id(current project ID)/projectGoalID(current project goal ID), one project can have many unique goals
        [HttpGet("[controller]/[action]/{id}/{projectGoalID}")]
        public async Task<IActionResult> EditGoal(int id, int projectGoalID)
        {
            var goals = await _context.ProjectGoals.SingleOrDefaultAsync(g => g.projectID == id && g.projectGoalID == projectGoalID);
            if (goals == null)
            {
                return NotFound();
            }
            return View(goals);
        }

        [HttpPost("[controller]/[action]/{id}/{projectGoalID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGoal ([Bind ("projectGoalID, goalText, projectID")] ProjectGoal goal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //updating db
                    _context.Update(goal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!DatabaseExists(1, goal.projectGoalID)) {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewProject), new { id = goal.projectID });
            }
            return View(goal);
        }

        // Get: Projects/EditUpdateEnrt/5/1/5
        // controller/action/id/updateNumber/projectUpdateID
        [HttpGet("[controller]/[action]/{id}/{updateNumber}/{projectUpdateID}")]
        public async Task<IActionResult> EditUpdateEntry (int id, int updateNumber, int projectUpdateID)
        {
            var updates = await _context.ProjectUpdates
                .SingleOrDefaultAsync(u => u.projectID == id && u.projectUpdateID == projectUpdateID);

            if (updates == null)
            {
                return NotFound();
            }

            return View(updates);
        }

        [HttpPost("[controller]/[action]/{id}/{updateNumber}/{projectUpdateID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUpdateEntry ([Bind ("projectUpdateID, projectID, updateNumber, updateText")] ProjectUpdate update)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(update);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatabaseExists(2, update.projectUpdateID))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewProject), new { id = update.projectID });
            }
            return View(update);
        }

        //GET: Projects/DeleteGoal/5/4501
        // controller/action/projectID/projectGoalID
        // delete project's unique goal.
        [HttpGet("[controller]/[action]/{id}/{projectGoalID}")]
        public async Task<IActionResult> DeleteGoal (int id, int projectGoalID)
        {
            ViewData["ProjectTitle"] = _context.Projects.Where(t => t.ID == id).Single().Title;

            var goal = await _context.ProjectGoals.SingleOrDefaultAsync(g => g.projectID == id && g.projectGoalID == projectGoalID);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        [HttpPost("[controller]/[action]/{id}/{projectGoalID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGoal ([Bind ("projectGoalID, projectID, goalText")] ProjectGoal goal)
        {
            if (ModelState.IsValid)
            {
                //no exception is thrown if the item does not exist
                _context.ProjectGoals.Remove(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProject), new { id = goal.projectID });
            }
            return View(goal);
        }

        //GET: Projects/DeleteUpdateEntry/5/5321
        // controller/action/projectID/projectUpdateID
        // deletes project's unique update. it does not matter to which update number it belongs
        [HttpGet("[controller]/[action]/{id}/{projectUpdateID}")]
        public async Task<IActionResult> DeleteUpdateEntry (int id, int projectUpdateID)
        {
            var update = await _context.ProjectUpdates.SingleOrDefaultAsync(u => u.projectID == id && u.projectUpdateID == projectUpdateID);

            if (update == null)
            {
                return NotFound();
            }

            return View(update);
        }

        [HttpPost("[controller]/[action]/{id}/{projectUpdateID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUpdateEntry ([Bind ("projectUpdateID, projectID, updateNumber, updateText")] ProjectUpdate update)
        {
            if (ModelState.IsValid)
            {
                _context.ProjectUpdates.Remove(update);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProject), new { id = update.projectID });
            }

            return View(update);
        }

        //GET: Projects/Create/5
        public IActionResult Create()
        {
            return View();
        }



        public IActionResult Project()
        {
            return RedirectToAction(nameof(Template));
        }

        public IActionResult Template()
        {
            return View();
        }

        //IF users want to do pesky things
        [AllowAnonymous]
        public IActionResult ShallNotPass()
        {
            return View();
        }

        //private support method to check if the project goal exists in the DB
        // 0 for Project table, 1 for ProjectGoal table, 2 for ProjectUpdate table
        private bool DatabaseExists (int table, int ID)
        {
            switch (table)
            {
                case 0:
                    return _context.Projects.Any(i => i.ID == ID);
                case 1:
                    return _context.ProjectGoals.Any(i => i.projectGoalID == ID);
                case 2:
                    return _context.ProjectUpdates.Any(i => i.projectUpdateID == ID);
                default:
                    return false; // if the program came here something inserted a bad value
            }
        }
    }
}