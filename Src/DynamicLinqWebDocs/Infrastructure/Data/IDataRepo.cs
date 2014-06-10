using DynamicLinqWebDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicLinqWebDocs.Infrastructure.Data
{
    interface IDataRepo
    {
        IEnumerable<Class> GetClasses();

        Class GetClass(string className);

        Method GetMethod(string className, string methodName, Frameworks framework, out Class @class, int overload = 0);


        IEnumerable<Expression> GetExpressions();

        Expression GetExpression(string expressionName);
    }
}
