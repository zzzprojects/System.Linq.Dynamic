using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Controllers
{
    /// <summary>
    /// Temporary perminent redirect for all /classes urls since it was renamed to library.
    /// </summary>
    [RoutePrefix("Classes")]
    public class ClassController : Controller
    {
        [Route("{*values}")]
        
        public ActionResult Any()
        {
            var url = Request.Url.ToString();

            var newUrl = url.Replace("classes", "library");

            return RedirectPermanent(newUrl);
        }
    }
}