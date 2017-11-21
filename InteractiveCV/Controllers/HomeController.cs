using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InteractiveCV.Models;
using InteractiveCV.Data;
using InteractiveCV.Services;
using Microsoft.Extensions.Logging;

namespace InteractiveCV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailSerivce;

        private readonly ILogger _logger;

        public HomeController(ApplicationDbContext context, ILoggerFactory logger, IEmailService emailService)
        {
            _context = context;
            _emailSerivce = emailService;
            _logger = logger.CreateLogger("Logger.Controller");
        }

        [HttpGet]
        public IActionResult Index()
        {
            //currently only the home screens shows the first popup. Plan to update this to show on first application login.
            var projects = _context.Projects.ToList();

            if (HttpContext.Request.Cookies["AnnoyingPopup"] == null)
            {
                ViewData["isPopup"] = true;
            } else
            {
                ViewData["isPopup"] = false;
            }

            ViewData["env"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return View(projects);
        }

        public IActionResult PopupClick()
        {
            
            HttpContext.Response.Cookies.Append(
            "AnnoyingPopup",
            "true",
            new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.Add(TimeSpan.FromDays(30))
            });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendNotification ()
        {
            return RedirectToAction(nameof(Contact));
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
