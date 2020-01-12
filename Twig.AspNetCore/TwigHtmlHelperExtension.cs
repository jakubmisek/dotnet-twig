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

            public string Template { get; set; }

            public string BlockName { get; set; }   // optional

            public PhpArray Data { get; set; }

            public TwigContent(Context ctx)
            {
                _ctx = ctx;
            }

            public void WriteTo(TextWriter writer, HtmlEncoder encoder)
            {
                // Pchp.Library.Spl.Autoload.spl_autoload_register(_ctx, ... ) // TODO: autoload, needed when PHP scripts are included

                var loader = new Twig.Loader.FilesystemLoader(_ctx, Path);
                var twig = new Twig.Environment(_ctx, loader, PhpValue.Null);

                var template = twig.load(Template);

                var output = BlockName == null
                    ? template.render(Data)
                    : template.renderBlock(BlockName, Data);

                //
                writer.Write(output);
            }
        }
        
        static TwigContent MakeTwigContent(IHtmlHelper htmlHelper, string path, string template, object model = null)
        {
            return new TwigContent(
                ctx: htmlHelper.ViewContext.HttpContext.GetOrCreateContext()    // PHP context for current HTTP context
            )
            {
                Path = path,
                Template = template,
                Data = model as PhpArray ?? Pchp.Core.Convert.ToArray(model ?? htmlHelper.ViewData),
            };
        }

        /// <summary>
        /// Returns output for the specified Twig template.
        /// </summary>
        public static IHtmlContent Twig(this IHtmlHelper htmlHelper, string path, string template, object model = null)
        {
            return MakeTwigContent(htmlHelper, path, template, model);
        }

        /// <summary>
        /// Returns output for the specified Twig block within a template.
        /// </summary>
        public static IHtmlContent TwigBlock(this IHtmlHelper htmlHelper, string path, string template, string block, object model = null)
        {
            var content = MakeTwigContent(htmlHelper, path, template, model);

            content.BlockName = block ?? throw new ArgumentNullException(nameof(block));

            return content;
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
