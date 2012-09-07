using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using ColoRAMA_Web.Models.Factories;

namespace ThemeGenerator.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ThemeGenerator(string forecolor, string backcolor, string maincolor, string contrast, string markupLowContrast)
        {
            Response.ContentType = "Text/vssettings";
            ThemeData[] arr = new ThemeData[2];
            ThemeData themeData = null;

            try
            {
                themeData = ColorFactory.GetThemeDataBright(forecolor, backcolor, maincolor, contrast);
            }
            catch (InvalidColorException)
            {
                TempData["Error"] = "Must supply valid color information!";
                return View("Index");
            }
            catch (InvalidContrastException)
            {
                TempData["Error"] = "Must supply valid contrast information!";
                return View("Index");
            }

            arr[0] = themeData;

            if ((markupLowContrast != null) && (bool.Parse(markupLowContrast)))
            {
                ThemeData markupData = ColorFactory.GetThemeDataBright(forecolor, backcolor, maincolor, "50");
                arr[1] = markupData;
                ViewData["theme"] = arr;
                return View();
            }

            arr[0] = themeData;
            arr[1] = themeData;
            ViewData["theme"] = arr;

            return View();
        }



        public JsonResult DemoColors(string forecolor, string backcolor, string maincolor, string contrast, string markupLowContrast)
        {
            ThemeData td = ColorFactory.GetThemeDataBrightHtml(forecolor, backcolor, maincolor, contrast);
            return Json(td, JsonRequestBehavior.AllowGet);
        }
    }
}
