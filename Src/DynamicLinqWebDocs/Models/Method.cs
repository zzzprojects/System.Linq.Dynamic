using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace DynamicLinqWebDocs.Models
{
    public class Method
    {
        public Method()
        {
            Frameworks = Frameworks.All;
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "isStatic")]
        public bool IsStatic { get; set; }

        [XmlAttribute(AttributeName = "isExtensionMethod")]
        public bool IsExtensionMethod { get; set; }

        public List<Argument> Arguments { get; set; }

        [XmlAttribute(AttributeName = "returnType")]
        public string ReturnType { get; set; }

        public string Description { get; set; }

        public string ReturnDescription { get; set; }

        public string Remarks { get; set; }

        public List<Example> Examples { get; set; }

        [XmlAttribute(AttributeName = "frameworks")]
        public Frameworks Frameworks { get; set; }

    }
}