﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameReview.Data;
using GameReview.Models;
using GameReview.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.Formatters;
using Markdig;

using Microsoft.AspNetCore.Authorization;

namespace GameReview.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GamesController(ApplicationDbContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Game.Include(i =>i.Reviews).ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new Review();
            viewModel = await _context.Review
                .Include(i => i.Game)
                .Include(i => i.Reviewer)
                .Where(i =>i.GameID == id)
                .Where(i =>i.ReviewerID == 1)
                .FirstOrDefaultAsync();               
            
            if (viewModel == null)
            {
                return NotFound();
            }

            //コメント欄はmarkdown⇒htmlへ変換する
            var pipline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            //Nullの時はエラーになる
            viewModel.Comment = Markdig.Markdown.ToHtml(viewModel.Comment ?? "", pipline);
            viewModel.ProsPoints = Markdig.Markdown.ToHtml(viewModel.ProsPoints ?? "", pipline);
            viewModel.ConsPoints = Markdig.Markdown.ToHtml(viewModel.ConsPoints ?? "", pipline);

            return View(viewModel);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameReviewVM gameReviewVM)
        {
            if (ModelState.IsValid)
            {

                gameReviewVM.Game.ImagePath = await SaveImageFileAsync(gameReviewVM.ImageFile);

                _context.Add(gameReviewVM.Game);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameReviewVM);
        }


        private async Task<string> SaveImageFileAsync(IFormFile postedFile)
        {
            string result = "";
            
            if (postedFile != null && postedFile.Length > 0)
            {
                // アップロードされたファイル名を取得。ブラウザが IE 
                // の場合 postedFile.FileName はクライアント側でのフ
                // ルパスになることがあるので Path.GetFileName を使う
                string filename =
                              DateTime.Now.ToString("yyyyMMddhhmmssfff") +  Path.GetFileName(postedFile.FileName);

                // アプリケーションルートの物理パスを取得。Core では
                // Server.MapPath は使えないので以下のようにする
                string contentRootPath =
                                _hostingEnvironment.ContentRootPath;
                //あらかじめ「UploadedFiles」フォルダを作っとく
                string filePath = Path.Combine(contentRootPath, "wwwroot", "UploadedFiles" , filename);

                using (var stream =
                            new FileStream(filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }

                result = filename + " (" + postedFile.ContentType +
                         ") - " + postedFile.Length +
                         " bytes アップロード完了";

                //物理パスではなく最終的にhtmlで表示する用の相対パスを返す
                return "\\UploadedFiles\\" + filename;
            }
            else
            {
                result = "ファイルアップロードに失敗しました";
            }

            return "";

        }


        // GET: Games/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            //常にIDが１
            var reviewer = await _context.Reviewer.Where(i => i.ID == 1).FirstOrDefaultAsync();
            var reviewerID = 1; //とりあえず

            var review = await _context.Review.Where(i => i.GameID == game.ID).Where(i =>i.ReviewerID==reviewerID).FirstOrDefaultAsync();

            var vm = new GameReviewVM();
            vm.Game = game;
            vm.Review = review;
            vm.Reviewer = reviewer;

            return View(vm);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, GameReviewVM vm)
        {
            if (id != vm.Game.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.ImageFile != null) {
                        vm.Game.ImagePath = await SaveImageFileAsync(vm.ImageFile);
                    }
                        _context.Update(vm.Game);

                    //ReviewにGameIDとReviewerIDを設定(わざわざ?)
                    vm.Review.GameID = vm.Game.ID;
                    vm.Review.ReviewerID = vm.Reviewer.ID;

                    var tmpReview = await _context.Review.Where(i => i.ReviewerID == vm.Reviewer.ID)
                                                .Where(i => i.GameID == vm.Game.ID)
                                                .AsNoTracking().FirstOrDefaultAsync();
                    
                    if(tmpReview == null) //既にレビューが登録済み
                    {
                        _context.Add(vm.Review);
                    }
                    else
                    {
                       vm.Review.ID = tmpReview.ID;
                        
                        _context.Update(vm.Review);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(vm.Game.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //画面遷移しない
                //return RedirectToAction(nameof(Index));
                return View(vm);
            }
            return View(vm);
        }

        // GET: Games/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.ID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.ID == id);
        }
    }
}
