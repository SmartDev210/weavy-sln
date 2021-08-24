using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that sends notification to bryan about company trashed
    /// </summary>
    [Serializable]
    [Guid("55089743-7AAE-4A14-AD4D-3C9D7FD1DF6A")]
    [Plugin(Description = "A hook that sends notification to bryan about company trashed")]
    public class AfterTrashCompanyHook : Hook, IHook<AfterTrashSpace>
    {
        /// <summary>
        /// A hook that sends notification to bryan about company trashed
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterTrashSpace e)
        {
            if (e.Trashed.Key.IsNullOrEmpty() || !e.Trashed.Key.StartsWith("company_"))
                return;

            List<User> users = new List<User>();

            var bryan = UserService.GetByEmail("support@findparts.aero");
            if (bryan != null) users.Add(bryan);
            var elliot = UserService.GetByEmail("smartdev210@outlook.com");
            if (elliot != null) users.Add(elliot);

            foreach (var user in users)
            {
                var notification = new Notification();
                notification.CreatedById = e.Trashed.CreatedById;
                var creator = UserService.Get(e.Trashed.CreatedById);

                notification.Html = $@"<span class=""actor"">@{creator.GetTitle()}</span> <span class=""subject"">trashed</span> his/her <span class=""context"">company({e.Trashed.Id}) - {e.Trashed.GetTitle()}</span>";
                notification.Text = $@"@{creator.GetTitle()} trashed his/her company({e.Trashed.Id}) - {e.Trashed.GetTitle()}";
                notification.Link = creator;
                notification.UserId = user.Id;
                
                NotificationService.Insert(notification);
            }
        }
    }

}