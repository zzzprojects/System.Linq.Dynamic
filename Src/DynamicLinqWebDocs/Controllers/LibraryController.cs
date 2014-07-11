using DynamicLinqWebDocs.Infrastructure.Data;
using DynamicLinqWebDocs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Controllers
{
    [RoutePrefix("Library")]
    public class LibraryController : Controller
    {
        IDataRepo _repo = new RealDataRepo();

        [Route]
        public ActionResult Index()
        {
            return View(_repo.GetClasses());
        }

        [Route("{classname}")]
        public ActionResult Class(string className)
        {
            var @class = _repo.GetClass(className);
            if (@class == null) return HttpNotFound();

            var viewModel = new Class()
            {
                Name = @class.Name,
                Namespace = @class.Namespace,
                Description = @class.Description,
                Remarks = @class.Remarks,
                Methods = @class.Methods,
                Properties = @class.Properties,
                IsInterface = @class.IsInterface 
            };

            return View(viewModel);
        }

        [Route("{className}/{methodName}/{framework:Enum(DynamicLinqWebDocs.Models.Frameworks)?}")]
        //
        // GET: /Docs/
        public ActionResult Method(string className, string methodName, Models.Frameworks framework = Models.Frameworks.NotSet, int o = 0)
        {
            Models.Class @class;

            var formattedMethodName = methodName.Replace('(', '<').Replace(')', '>');

            var method = _repo.GetMethod(className, formattedMethodName, framework, out @class, o);
            if (method == null) return HttpNotFound();

            var viewModel = new Method()
            {
                Namespace = @class.Namespace,
                Class = @class.Name,
                Name = method.Name,
                Arguments = method.Arguments,
                IsStatic = method.IsStatic,
                IsExtensionMethod = method.IsExtensionMethod,
                ReturnType = method.ReturnType,
                Remarks = method.Remarks,
                Description = method.Description,
                Examples = method.Examples,
                ReturnDescription = method.ReturnDescription,
                Frameworks = method.Frameworks,
                HasParamsArgument = method.HasParamsArgument
            };

            return View(viewModel);
        }

        [Route("{className}/Property-{propertyName}/{framework:Enum(DynamicLinqWebDocs.Models.Frameworks)?}")]
        public ActionResult Property(string className, string propertyName, Models.Frameworks framework = Models.Frameworks.NotSet)
        {
            Models.Class @class;

            var property = _repo.GetProperty(className, propertyName, framework, out @class);
            if (property == null) return HttpNotFound();

            var viewModel = new Property()
            {
                Namespace = @class.Namespace,
                Class = @class.Name,
                Name = property.Name,
                IsStatic = property.IsStatic,
                ValueType = property.ValueType,
                Remarks = property.Remarks,
                Description = property.Description,
                Examples = property.Examples,
                ValueTypeDescription = property.ValueTypeDescription,
                Frameworks = property.Frameworks,
                Accessors = property.Accessors
            };

            return View(viewModel);
        }
	}
}