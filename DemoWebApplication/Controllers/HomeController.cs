﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using DemoWebApplication.Model;
using Microsoft.Extensions.Localization;

namespace DemoWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private DemoModel model;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            model = new DemoModel();
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            model.Lang = HttpContext.GetMyCulture();

            // IStringLocalizer uses CurrentUICulture by default
            model.Message = _localizer["Message"];
            ViewData["CurrentUICulture"] = CultureInfo.CurrentUICulture;
            return View(model);
        }

        [HttpPost]
        public IActionResult AcceptForm(DemoModel newModel)
        {
            if (ModelState.IsValid)
            {
                return Content("OK");
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult GetCulture()
        {
            return Content(CultureInfo.CurrentCulture.Name);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
