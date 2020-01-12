using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pchp.Core;
using Peachpie.AspNetCore.Web;

namespace Microsoft.AspNetCore.Mvc  // implicitly imported namespace in Razor pages
{
    /// <summary>
    /// Extension for <see cref="IHtmlHelper"/>.
    /// </summary>
    public static class TwigHtmlHelperExtension
    {
        /// <summary>
        /// Helper class implementing <see cref="IHtmlContent"/>.
        /// </summary>
        sealed class TwigContent : IHtmlContent
        {
            readonly Context _ctx;

            public string Path { get; set; }

            public string Name { get; set; }

            public PhpArray Data { get; set; }

            public TwigContent(Context ctx)
            {
                _ctx = ctx;
            }

            public void WriteTo(TextWriter writer, HtmlEncoder encoder)
            {
                var loader = new Twig.Loader.FilesystemLoader(_ctx, Path);
                var twig = new Twig.Environment(_ctx, loader, PhpValue.Null);

                var output = twig.render(Name, Data ?? PhpArray.Empty);

                //
                writer.Write(output);
            }
        }

        /// <summary>
        /// Returns output for the specified PHP script.
        /// </summary>
        public static IHtmlContent Twig(this IHtmlHelper htmlHelper, string path, string name, object model = null)
        {
            return new TwigContent(
                ctx: htmlHelper.ViewContext.HttpContext.GetOrCreateContext()    // PHP context for current HTTP context
            )
            {
                Path = path,
                Name = name,
                Data = model as PhpArray ?? Pchp.Core.Convert.ToArray(model ?? htmlHelper.ViewData),
            };
        }

        /// <summary>
        /// Renders specified PHP script.
        /// </summary>
        public static void RenderTwig(this IHtmlHelper htmlHelper, string path, string name, object model = null)
        {
            Twig(htmlHelper, path, name, model)
                .WriteTo(htmlHelper.ViewContext.Writer, null);
        }
    }
}
