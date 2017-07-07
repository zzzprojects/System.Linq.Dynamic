using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DynamicLinqWebDocs.Infrastructure
{
    public static class HtmlHelpers
    {
        private static string FormatText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var lines = value.Split(new[] { '\n' }, StringSplitOptions.None);

            if (lines.Length > 1)
            {
                /*
                    Search for the minimum left padding across all the lines.
                */
                var paddingLeft = int.MaxValue;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var count = 0;

                    for (var i = 0; i < line.Length; ++i, ++count)
                    {
                        if (!char.IsWhiteSpace(line[i])) break;
                    }

                    if (paddingLeft > count) paddingLeft = count;
                }
                if (paddingLeft > 0)
                {
                    var builder = new StringBuilder(value.Length - (lines.Length * paddingLeft));
                    for (var i = 0; i < lines.Length; ++i)
                    {
                        var line = lines[i];

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            if (i == 0 || i == lines.Length - 1) continue;

                            builder.AppendLine();
                        }
                        else
                        {
                            builder.AppendLine(line.Substring(paddingLeft));
                        }
                    }

                    value = builder.ToString();
                }
            }
            else
            {
                value = value.TrimStart();
            }

            return value;
        }

        private static HtmlString FormatMarkdown(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;

            var md = new MarkdownSharp.Markdown();

            var result = md.Transform(FormatText(value));

            return new HtmlString(result);
        }

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
            if (String.IsNullOrWhiteSpace(value)) return null;

            var pre = new TagBuilder("pre");

            pre.AddCssClass("sunlight-highlight-csharp");

            pre.SetInnerText(FormatText(value));

            return new HtmlString(pre.ToString());
        }

        public static HtmlString FormatInlineCodeList(this HtmlHelper helper, params string[] items)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-inline");

            var sb = new StringBuilder();

            foreach (var item in items)
            {
                sb.AppendFormat("<li><code>{0}</code></li>", FormatText(item));
            }

            ul.InnerHtml = sb.ToString();

            return new HtmlString(ul.ToString());
        }

        public static HtmlString IsActive(this HtmlHelper html, string url)
        {
            return IsActive(html, url, false);
        }

        public static HtmlString IsActive(this HtmlHelper html, string url, bool startsWith)
        {
            var request = html.ViewContext.RequestContext.HttpContext.Request.Url;

            bool isActive;

            if (startsWith)
            {
                isActive = request.AbsolutePath.StartsWith(url, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                isActive = request.AbsolutePath.Equals(url, StringComparison.InvariantCultureIgnoreCase);
            }

            return isActive ? new HtmlString("active") : null;
        }

        public static HtmlString OpenNote(this HtmlHelper helper)
        {
            return new HtmlString("<div class=\"panel panel-default\"><div class=\"panel-heading\">Note</div><div class=\"panel-body\">");
        }

        public static HtmlString CloseNote(this HtmlHelper helper)
        {
            return new HtmlString("</div></div>");
        }
    }
}