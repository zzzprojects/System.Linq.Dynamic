using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Infrastructure
{
    public static class HtmlHelpers
    {
        public static HtmlString FormatMarkdown(this HtmlHelper helper, string value)
        {
            return FormatMarkdown(value);
        }

        public static HtmlString FormatMarkdown(this HtmlHelper helper, string format, params object[] args)
        {
            return FormatMarkdown(String.Format(format, args));
        }

        public static HtmlString FormatCodeBlock(this HtmlHelper helper, string value)
        {
            var pre = new TagBuilder("pre");

            pre.AddCssClass("sunlight-highlight-csharp");

            pre.SetInnerText(value);

            return new HtmlString(pre.ToString());
        }

        static HtmlString FormatMarkdown(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;

            var md = new MarkdownSharp.Markdown();

            //if (convertToCode) value = ConvertToCode(value);

            var result = md.Transform(value);

            return new HtmlString(result);
        }

        //static string ConvertToCode(string value)
        //{
        //    var sb = new StringBuilder();

        //    if (value.Length > 0) sb.Append("    ");

        //    for( int i = 0; i < value.Length; i++)
        //    {
        //        sb.Append(value[i]);

        //        //only add space if we're not at the end
        //        if (value[i] == '\n' && i != value.Length - 1)
        //        {
        //            sb.Append("    ");
        //        }
        //    }

        //    return sb.ToString();
        //}

    }
}