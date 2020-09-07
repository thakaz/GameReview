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
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Review.Include(r => r.Game).Include(r => r.Reviewer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Review = await _context.Review
                .Include(r => r.Game)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Review == null)
            {
                return NotFound();
            }

            return View(Review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title");
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GameID,ReviewerID,Grade,Summary")] Review Review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", Review.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", Review.ReviewerID);
            return View(Review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Review = await _context.Review.FindAsync(id);
            if (Review == null)
            {
                return NotFound();
            }
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", Review.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", Review.ReviewerID);
            return View(Review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GameID,ReviewerID,Grade,Summary")] Review Review)
        {
            if (id != Review.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(Review.ID))
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
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", Review.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", Review.ReviewerID);
            return View(Review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Review = await _context.Review
                .Include(r => r.Game)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Review == null)
            {
                return NotFound();
            }

            return View(Review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Review = await _context.Review.FindAsync(id);
            _context.Review.Remove(Review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ID == id);
        }
    }
}
