using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that automatically add users in findparts/mrofinder
    /// </summary>
    [Serializable]
    [Guid("F45BD2FB-910B-4FE1-A2E7-3BEE040E36E4")]
    [Plugin(Description = "A hook that automatically users in findparts/mrofinder")]
    public class AfterAdminAddedToCompanyHook : Hook, IHook<AfterAddMember>
    {
        /// <summary>
        /// when the added member is admin, add tha process that user in findparts/mrofinder
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterAddMember e)
        {   
            if (!e.Space.Key.IsNullOrEmpty() && e.Space.Key.StartsWith("company_") && e.Member.Access == Access.Admin)
            {
                var creator = UserService.Get(e.Space.CreatedById, sudo: true);

                var spaces = SpaceService.Search(new SpaceQuery { MemberId = e.Member.Id, Sudo = true }).ToList();
                foreach (var space in spaces)
                {   
                    if (space.Id != e.Space.Id && !string.IsNullOrEmpty(space.Key) && space.Key.StartsWith("company_"))
                    {   
                        if (space.HasPermission(Permission.Admin, e.Member))
                        {
                            SpaceService.AddMember(e.Space.Id, e.Member.Id, Access.Read, true);

                            var notification = new Notification();
                            notification.CreatedById = e.Member.Id;
                            notification.Html = $@"<span class=""actor"">@{e.Member.GetTitle()}</span> <span class=""context"">can only be assigned to manage 1 company profile.</span>";
                            notification.Text = $@"@{e.Member.GetTitle()} can only be assigned to manage 1 company profile";
                            notification.Link = e.Member;
                            notification.UserId = e.Space.CreatedById;

                            NotificationService.Insert(notification);

                            return;
                        }
                    }
                }

                EntityService.Star(e.Space, e.Member.Id, sudo: true);

                HttpClient client = new HttpClient();

                _ = client.PostAsJsonAsync($"{ConfigurationService.AppSetting("findparts-url")}/web-api/add-vendor-user", new
                {
                    CreatorEmail = creator.Email,
                    Email = e.Member.Email
                });
            }
        }
    }
}