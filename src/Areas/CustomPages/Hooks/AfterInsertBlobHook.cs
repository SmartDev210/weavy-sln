using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Repos;
using Weavy.Core.Services;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that alert if any user uses much spaces
    /// </summary>
    [Serializable]
    [Guid("A54532DA-1921-439C-8EE1-1144796E949D")]
    [Plugin(Description = "A hook that alert if any user uses much spaces")]
    public class AfterInsertBlobHook : Hook, IHook<AfterInsertBlob>
    {
        /// <summary>
        /// handler
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterInsertBlob e)
        {
            var creatorId = e.Inserted.CreatedById;

            var searchResult = BlobRepo.Search(new BlobQuery { CreatedById = creatorId, Sudo = true });

            long size = 0;
            foreach (var blob in searchResult)
            {
                size += blob.Size;
            }
            if (size > 1024 * 1024 * 1024)
            {
                List<User> users = new List<User>();

                var bryan = UserService.GetByEmail("support@findparts.aero");
                if (bryan != null) users.Add(bryan);
                var elliot = UserService.GetByEmail("smartdev210@outlook.com");
                if (elliot != null) users.Add(elliot);
                
                foreach (var user in users)
                {
                    var notification = new Notification();
                    notification.CreatedById = e.Inserted.CreatedById;
                    var creator = UserService.Get(e.Inserted.CreatedById);

                    notification.Html = $@"<span class=""actor"">@{creator.GetTitle()}</span> is utilizing over 1GB of blob data";
                    notification.Text = $@"@{creator.GetTitle()} is utilizing over 1GB of blob data";
                    notification.Link = creator;
                    notification.UserId = user.Id;

                    NotificationService.Insert(notification);
                }
            }
        }
    }
}