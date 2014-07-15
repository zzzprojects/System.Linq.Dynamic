using DynamicLinqWebDocs.Infrastructure;
using DynamicLinqWebDocs.Infrastructure.Data;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Controllers
{
    [RoutePrefix("Expressions")]
    public class ExpressionController : Controller
    {
        IDataRepo _repo = new RealDataRepo();

        [Route]
        public ActionResult Index()
        {
            return View(_repo.GetExpressions());
        }

        [Route("{expressionName}")]
        public ActionResult Expression(string expressionName) 
        {
            return View(_repo.GetExpression(expressionName));
        }

        class ExpressionSitemap : SitemapContributor
        {
            protected internal override IEnumerable<SitemapNode> GetSitemapNodes(UrlHelper urlHelper, HttpContextBase httpContext)
            {
                yield return new SitemapNode(urlHelper.Action("Index", "Expression")) { Priority = 0.8m };

                var repo = new RealDataRepo();

                foreach( var expression in repo.GetExpressions())
                {
                    yield return new SitemapNode(urlHelper.Action("Expression", "Expression", new { expressionName = expression.Name })) { Priority = 0.50m };
                }
            }
        }
	}
}