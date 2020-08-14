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
    public class ReviewerGameReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewerGameReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReviewerGameReviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReviewerGameReview.Include(r => r.Game).Include(r => r.Reviewer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ReviewerGameReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewerGameReview = await _context.ReviewerGameReview
                .Include(r => r.Game)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reviewerGameReview == null)
            {
                return NotFound();
            }

            return View(reviewerGameReview);
        }

        // GET: ReviewerGameReviews/Create
        public IActionResult Create()
        {
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title");
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID");
            return View();
        }

        // POST: ReviewerGameReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GameID,ReviewerID,Grade,Summary")] ReviewerGameReview reviewerGameReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviewerGameReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", reviewerGameReview.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", reviewerGameReview.ReviewerID);
            return View(reviewerGameReview);
        }

        // GET: ReviewerGameReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewerGameReview = await _context.ReviewerGameReview.FindAsync(id);
            if (reviewerGameReview == null)
            {
                return NotFound();
            }
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", reviewerGameReview.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", reviewerGameReview.ReviewerID);
            return View(reviewerGameReview);
        }

        // POST: ReviewerGameReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GameID,ReviewerID,Grade,Summary")] ReviewerGameReview reviewerGameReview)
        {
            if (id != reviewerGameReview.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewerGameReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewerGameReviewExists(reviewerGameReview.ID))
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
            ViewData["GameID"] = new SelectList(_context.Game, "ID", "Title", reviewerGameReview.GameID);
            ViewData["ReviewerID"] = new SelectList(_context.Reviewer, "ID", "ID", reviewerGameReview.ReviewerID);
            return View(reviewerGameReview);
        }

        // GET: ReviewerGameReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewerGameReview = await _context.ReviewerGameReview
                .Include(r => r.Game)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reviewerGameReview == null)
            {
                return NotFound();
            }

            return View(reviewerGameReview);
        }

        // POST: ReviewerGameReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewerGameReview = await _context.ReviewerGameReview.FindAsync(id);
            _context.ReviewerGameReview.Remove(reviewerGameReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewerGameReviewExists(int id)
        {
            return _context.ReviewerGameReview.Any(e => e.ID == id);
        }
    }
}
