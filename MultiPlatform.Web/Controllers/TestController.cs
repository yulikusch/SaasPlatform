using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiPlatform.Application.Entities;
using MultiPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MultiPlatform.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TestController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> CreateBlog(string title)
        {
            var blog = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = title
            };

            _db.BlogPosts.Add(blog);
            await _db.SaveChangesAsync();

            return Content("Blog dibuat: " + title);
        }

        [HttpGet]
        public async Task<IActionResult> ListBlog()
        {
            var blogs = await _db.BlogPosts
                .Select(x => x.Title)
                .ToListAsync();

            return Json(blogs);
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}