using DynamicLinqWebDocs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace DynamicLinqWebDocs.Infrastructure.Data
{

    public class DynLINQDoc
    {
        public List<Class> Classes { get; set; }

        public List<Expression> Expressions { get; set; }
    }


    class RealDataRepo : IDataRepo 
    {
        static DynLINQDoc _doc;

        static RealDataRepo()
        {
            var serializer = new XmlSerializer(typeof(DynLINQDoc));

            var filePath = HostingEnvironment.MapPath(@"~/App_Data/DynLINQDoc.xml");

            using( var file = File.Open(filePath, FileMode.Open))
            {
                _doc = (DynLINQDoc)serializer.Deserialize(file);
            }
        }

        public IEnumerable<Class> GetClasses()
        {
            return _doc.Classes;
        }

        public Class GetClass(string className)
        {
            return _doc.Classes
                .Where(x => className.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        public Method GetMethod(string className, string methodName, Frameworks framework, out Class @class, int overload)
        {
            @class = GetClass(className);
            if (@class == null) return null;

            if (overload < 0) return null;

            IEnumerable<Method> methodFinder = @class.Methods
                .Where(x => methodName.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

            if (framework == Frameworks.NotSet)
            {
                methodFinder = methodFinder.OrderByDescending(x => x.Frameworks);
            }
            else
            {
                methodFinder = methodFinder.Where(x => x.Frameworks.HasFlag(framework));
            }

            if( overload > 0 ) methodFinder = methodFinder.Skip(overload);

            return methodFinder.FirstOrDefault();
        }

        public Expression GetExpression(string expressionName)
        {
            return _doc.Expressions
                .Where(x => expressionName.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        public IEnumerable<Expression> GetExpressions()
        {
            return _doc.Expressions;
        }
    }
}