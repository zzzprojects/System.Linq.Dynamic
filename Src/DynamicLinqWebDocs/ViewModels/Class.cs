using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicLinqWebDocs.ViewModels
{
    public class Class
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }

        public IList<Models.Method> Methods { get; set; }
    }
}