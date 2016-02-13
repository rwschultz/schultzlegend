using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchultzLegendWebSite.Models;
using SchultzLegendWebSite.Utilities;

namespace SchultzLegendWebSite.Filters
{
    public class SchultzLegendFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResultBase;
            if (result != null)
            {
                var model = result.Model as BaseViewModel;
                if (model != null)
                {
                    if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                    {
                        UserLogin user = DBHelper.DBGetUserLogin(filterContext.RequestContext.HttpContext.User.Identity.Name);
                        model.Email = user.Email;
                    }
                }
            }
        }
    }
}