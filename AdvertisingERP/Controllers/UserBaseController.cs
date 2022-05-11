using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdvertisingERP.Controllers
{
    public class UserBaseController : Controller
    {
        // GET: UserBase
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // code involving this.Session // edited to simplify
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            // If the browser session or authentication session has expired...
            if (session.IsNewSession || Session["UserID"] == null)
            {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                     new { action = "Index", Controller = "Home" }));
            }
            else
            {
                //var data=(List<Login>) Session["LoginUser"];
                //if (data[0].UserType == 1)
                //{
                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                //      new { action = "Index", Controller = "Admin" }));
                //}
                //else if (data[0].UserType == 3)
                //{
                //    filterContext.Result = new RedirectResult("http://www.google.com", true);                 
                //}
                //else
                //{
                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                //      new { action = "Index", Controller = "Home" }));
                //}
            }
            base.OnActionExecuting(filterContext); // re-added in edit
        }
    }
}