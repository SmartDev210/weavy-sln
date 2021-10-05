using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Helpers
{
    public static class CustomExtenstions
    {
        public static MvcHtmlString PublicAvatarImg(this IHasAvatar avatar, int size = 32)
        {
            StringBuilder sb = new StringBuilder();
            if (avatar.Avatar != null)
            {
                sb.Append($@"<img alt="""" class=""img-{size} avatar"" src=""/avatar-images/{avatar.Avatar.Id}/avatar-{size * 2}"">");
            } else
            {
                sb.Append($@"<img alt="""" class=""img-{size} avatar"" src=""/img/space.svg"">");
            }
            
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString PublicFacepile(this IEnumerable<User> users, int size = 32, bool presence = false)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var user in users)
            {
                if (presence == true)
                {
                    sb.Append($@"<a href=""/people/{user.Id}"" title=""{user.GetTitle()}"">");
                    sb.Append($@"<div class=""img-{size}"" data-active=""{user.Id}"">");
                    sb.Append($@"<img alt=""{user.GetTitle()}"" class=""img-{size} avatar"" src=""/people-images/{user.Id}/avatar-{size * 2}"">");
                    sb.Append($@"</div>");
                    sb.Append($@"</a>");
                } else
                {
                    sb.Append($@"<img alt=""{user.GetTitle()}"" class=""img-{size} avatar"" src=""/people-images/{user.Id}/avatar-{size * 2}"">");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}