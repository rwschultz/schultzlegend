using System.Collections.Generic;
using System.Web.Mvc;
using SchultzLegendWebSite.Utilities;
using SchultzLegendWebSite.Filters;

namespace SchultzLegendWebSite.Controllers
{
    [SchultzLegendFilter]
    public class BaseController : Controller
    {
    }
}