using DynamicLinqWebDocs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Controllers
{

    public class SiteController : Controller
    {

        [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            return SitemapGenerator.GetSitemapXml(Url, HttpContext);
        }

    }
}