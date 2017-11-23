using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using DemoWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace DemoWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DemoModel _model;
        private readonly IStringLocalizer<HomeController> _localizer;
        private const double MaxModelLength = 10.0;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _model = new DemoModel {DecimalValue = 142.99m};
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            _model.Lang = HttpContext.GetMyCulture();
            // IStringLocalizer uses CurrentUICulture by default
            _model.Message = _localizer["Message"];
            ViewData["CurrentUICulture"] = CultureInfo.CurrentUICulture;
            return View(_model);
        }

        [HttpPost]
        public IActionResult AcceptForm([Bind("Number")] DemoModel newModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Content("OK");
        }
        
        public ActionResult Coords([Bind("First", "Second")] DemoModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (inputModel.Length() > MaxModelLength)
            {
                string content = _localizer["WrongLength"] + ": " + inputModel.Length();
                return Content(content);
            }
            return Content(_localizer["OkLength"] + ": " + inputModel.Length());
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
