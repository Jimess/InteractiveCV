using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InteractiveCV.Data;
using InteractiveCV.Models;
using Microsoft.AspNetCore.Authorization;

namespace InteractiveCV.Controllers
{
    [Authorize]
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Skills
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            var result = from s in _context.Skills select s;

            if (searchString != null)
            {
                result = result.Where(skill => skill.Name.Contains(searchString));
            }

            return View(await result.Include(l => l.Links).ToListAsync());
        }

        // GET: Skills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skills = await _context.Skills
                .SingleOrDefaultAsync(m => m.ID == id);
            if (skills == null)
            {
                return NotFound();
            }

            return View(skills);
        }

        // GET: Skills/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Experience,RelatedExperience")] Skills skills)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skills);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skills);
        }

        // GET: Skills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skills = await _context.Skills.SingleOrDefaultAsync(m => m.ID == id);
            if (skills == null)
            {
                return NotFound();
            }
            return View(skills);
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Experience,RelatedExperience")] Skills skills)
        {
            if (id != skills.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skills);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillsExists(skills.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(skills);
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skills = await _context.Skills
                .SingleOrDefaultAsync(m => m.ID == id);
            if (skills == null)
            {
                return NotFound();
            }

            return View(skills);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skills = await _context.Skills.SingleOrDefaultAsync(m => m.ID == id);
            _context.Skills.Remove(skills);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //don't need async here
        // GET: Skills/AddLink/5/
        // controller/action/skillID
        // adds a link to a specific link
        [HttpGet("[controller]/[action]/{skillsID}")]
        public async Task<IActionResult> AddLink(int skillsID)
        {

            //ViewBag to be able to use multiple models
            ViewBag.Projects = await _context.Projects.FromSql("SELECT * FROM Project").ToListAsync();
            //var info = await _context.Projects.FromSql("SELECT * FROM Projects").ToListAsync();

            ViewData["SkillsID"] = skillsID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLink([Bind("SkillLinkID, skillsID, Text, projectNum, isExternal, externalLink")] SkillLink skillsLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skillsLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(skillsLink);
        }

        private bool SkillsExists(int id)
        {
            return _context.Skills.Any(e => e.ID == id);
        }
    }
}
