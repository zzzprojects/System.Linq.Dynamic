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
            return FormatMarkdown(value, false);
        }

        public static HtmlString FormatMarkdown(this HtmlHelper helper, string format, params object[] args)
        {
            return FormatMarkdown(String.Format(format, args), false);
        }

        public static HtmlString FormatMarkdownCodeBlock(this HtmlHelper helper, string value)
        {
            return FormatMarkdown(value, convertToCode: true);
        }

        static HtmlString FormatMarkdown(string value, bool convertToCode)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;

            var md = new MarkdownSharp.Markdown();

            if (convertToCode) value = ConvertToCode(value);

            var result = md.Transform(value);

            return new HtmlString(result);
        }

        static string ConvertToCode(string value)
        {
            var sb = new StringBuilder();

            if (value.Length > 0) sb.Append("    ");

            for( int i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);

                //only add space if we're not at the end
                if (value[i] == '\n' && i != value.Length - 1)
                {
                    sb.Append("    ");
                }
            }

            return sb.ToString();
        }

    }
}