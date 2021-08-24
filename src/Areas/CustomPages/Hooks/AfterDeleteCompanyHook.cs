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
    [Guid("D61EBF0D-AF79-457C-8FF0-E527476F16DA")]
    [Plugin(Description = "A hook that sends notification to bryan about company deleted")]
    public class AfterDeleteCompanyHook : Hook, IHook<AfterDeleteSpace>
    {
        /// <summary>
        /// A hook that sends notification to bryan about company trashed
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterDeleteSpace e)
        {
            if (e.Deleted.Key.IsNullOrEmpty() || !e.Deleted.Key.StartsWith("company_"))
                return;

            List<User> users = new List<User>();

            var bryan = UserService.GetByEmail("support@findparts.aero");
            if (bryan != null) users.Add(bryan);
            var elliot = UserService.GetByEmail("smartdev210@outlook.com");
            if (elliot != null) users.Add(elliot);

            foreach (var user in users)
            {
                var notification = new Notification();
                notification.CreatedById = e.Deleted.CreatedById;
                var creator = UserService.Get(e.Deleted.CreatedById);

                notification.Html = $@"<span class=""actor"">@{creator.GetTitle()}</span> <span class=""subject"">deleted</span> his/her <span class=""context"">company({e.Deleted.Id}) - {e.Deleted.GetTitle()}</span> permanently";
                notification.Text = $@"@{creator.GetTitle()} deleted his/her company({e.Deleted.Id}) - {e.Deleted.GetTitle()} permanently";
                notification.Link = creator;
                notification.UserId = user.Id;
                
                NotificationService.Insert(notification);
            }
        }
    }

}