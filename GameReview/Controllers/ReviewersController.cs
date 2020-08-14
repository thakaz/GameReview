using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameReview.Data;
using GameReview.Models;

namespace GameReview.Controllers
{
    public class ReviewersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviewers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reviewer.ToListAsync());
        }

        // GET: Reviewers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewer = await _context.Reviewer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reviewer == null)
            {
                return NotFound();
            }

            return View(reviewer);
        }

        // GET: Reviewers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reviewers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Reviewer reviewer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviewer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reviewer);
        }

        // GET: Reviewers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewer = await _context.Reviewer.FindAsync(id);
            if (reviewer == null)
            {
                return NotFound();
            }
            return View(reviewer);
        }

        // POST: Reviewers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Reviewer reviewer)
        {
            if (id != reviewer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewerExists(reviewer.ID))
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
            return View(reviewer);
        }

        // GET: Reviewers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewer = await _context.Reviewer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reviewer == null)
            {
                return NotFound();
            }

            return View(reviewer);
        }

        // POST: Reviewers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewer = await _context.Reviewer.FindAsync(id);
            _context.Reviewer.Remove(reviewer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewerExists(int id)
        {
            return _context.Reviewer.Any(e => e.ID == id);
        }
    }
}
