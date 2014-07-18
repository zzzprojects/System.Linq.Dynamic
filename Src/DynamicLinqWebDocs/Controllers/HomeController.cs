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
            this.SetMetaDescription("A living branch of the Microsoft Dyamic LINQ library, allowing developers to construct LINQ queries using string expressions instead of lambda expressions.");
            this.AddMetaKeywords("Dynamic LINQ", "Entity Framework", "Query", "Queries");

            return View();
        }

        [Route("Info")]
        public ActionResult Info()
        {
            this.SetMetaDescription("Project information including source code, bug tracking, NuGet package, and LinkedIn profiles for developers.");
            this.AddMetaKeywords("GitHub", "NuGet", "LinkedIn");

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