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
using Microsoft.AspNetCore.Identity;
using GameReview.Authorization;



namespace GameReview.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GamesController(ApplicationDbContext context,IWebHostEnvironment hostEnvironment,
                                UserManager<IdentityUser> UserManager,
                                IAuthorizationService AuthorizationService,
                                RoleManager<IdentityRole> RoleManager)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
            _userManager = UserManager;
            _authorizationService = AuthorizationService;
            _roleManager = RoleManager;
        }

        // GET: Games
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Game.Include(i =>i.Reviews).ToListAsync());
        }

        // GET: Games/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole = await _roleManager.FindByNameAsync(Constants.ReviewAdministratorsRole);
            //var adminId = _userManager.
            var adminUserId = _context.UserRoles.Where(c => c.RoleId == adminRole.Id).FirstOrDefault().UserId;

            var viewModel = new Review();
            viewModel = await _context.Review
                .Include(i => i.Game)
                .Where(i =>i.GameID == id)
                .Where(i =>i.ReviewerID == _userManager.GetUserId(User) || i.ReviewerID == adminUserId)
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(GameReviewVM gameReviewVM)
        {
            if (!ModelState.IsValid)
            {
                return View(gameReviewVM);
            }

            gameReviewVM.Game.ImagePath = await SaveImageFileAsync(gameReviewVM.ImageFile);

            _context.Add(gameReviewVM.Game);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
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
        [Authorize(Roles = "ReviewAdministrators")]
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

            var review = await _context.Review.Where(i => i.GameID == game.ID).Where(i =>i.ReviewerID==_userManager.GetUserId(User)).FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            var vm = new GameReviewVM();
            vm.Game = game;
            vm.Review = review;

            return View(vm);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ReviewAdministrators")]
        public async Task<IActionResult> Edit(int id, GameReviewVM vm)
        {
            if (id != vm.Game.ID)
            {
                return NotFound();
            }

            // 更新日設定
            var now = DateTime.Now;
            vm.Review.updated_at = now;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                if (vm.ImageFile != null) {
                    vm.Game.ImagePath = await SaveImageFileAsync(vm.ImageFile);
                }
                _context.Update(vm.Game);
                    
                //ReviewにGameIDを設定(わざわざ?)
                vm.Review.GameID = vm.Game.ID;
                vm.Review.ReviewerID = _userManager.GetUserId(User);    

                var tmpReview = await _context.Review.Where(i => i.ReviewerID == vm.Review.ReviewerID)
                                            .Where(i => i.GameID == vm.Game.ID)
                                            .AsNoTracking().FirstOrDefaultAsync();

                //既にレビューが登録済みかどうか判定
                if (tmpReview == null) 
                {
                    vm.Review.created_at = now;
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
           TempData["Message"] = "せいこうしました";
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
