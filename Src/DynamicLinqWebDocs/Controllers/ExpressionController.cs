using DynamicLinqWebDocs.Infrastructure.Data;
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
	}
}