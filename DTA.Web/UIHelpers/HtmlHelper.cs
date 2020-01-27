using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTA.Web
{
    public static class HtmlHelperImage
    {
        public static IHtmlString Image(this HtmlHelper helper, string src, string alt,string width="270",string height="340")
        {
            TagBuilder img = new TagBuilder("img");
            img.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            img.Attributes.Add("alt", alt);
            img.Attributes.Add("width", width);
            img.Attributes.Add("height", height);

            return new MvcHtmlString(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}