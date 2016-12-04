using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PermissionTest
{
    public static class Extensions
    {
        /// <summary>
        /// The strip HTML reg ex
        /// </summary>
        static readonly Regex StripHtmlRegEx = new Regex(@"<(.|\n)*?>");

        /// <summary>
        /// Strips the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>System.String.</returns>
        public static string StripHtml(this string html)
        {
            return String.IsNullOrEmpty(html) ? null : StripHtmlRegEx.Replace(html, "").Replace("&nbsp;", " ");
        }

        /// <summary>
        /// Safe Trim String
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>String</returns>
        public static string SafeTrim(this string text)
        {
            return (text ?? string.Empty).Trim();
        }
    }
}
