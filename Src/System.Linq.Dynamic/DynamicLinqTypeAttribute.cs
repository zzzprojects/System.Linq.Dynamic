using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public class DynamicLinqTypeAttribute : Attribute
    {
    }
}
