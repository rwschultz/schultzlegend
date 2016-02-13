using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchultzLegendWebSite.Models;
using SchultzLegendWebSite.Utilities;

namespace SchultzLegendWebSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            BaseViewModel model = new BaseViewModel();
            model.CurrentPage = new Page("Home", "/");
            return View(model);
        }
    }
}