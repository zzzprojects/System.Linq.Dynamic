using DynamicLinqWebDocs.Infrastructure;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Controllers
{
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Info")]
        public ActionResult Info()
        {
            return View();
        }

        class HomeSitemap : SitemapContributor
        {
            protected internal override IEnumerable<SitemapNode> GetSitemapNodes(UrlHelper urlHelper, HttpContextBase httpContext)
            {
                yield return new SitemapNode(urlHelper.Action("Index", "Home")) { Priority = 1 };
                yield return new SitemapNode(urlHelper.Action("Info", "Home")) { Priority = 1 };
            }
        }
    }
}