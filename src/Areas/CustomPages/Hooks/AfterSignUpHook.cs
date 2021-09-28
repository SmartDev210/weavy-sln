using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Services;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that automatically add users to global spaces
    /// </summary>
    [Serializable]
    [Guid("F9EAF019-1ADC-46A0-8A29-3C9FC866ABAF")]
    [Plugin(Description = "A hook that automatically add new users to global spaces")]
    public class AfterSignUpHook : Hook, IHook<AfterInsertUser>
    {
        /// <summary>
        /// A hook that automatically add users to global spaces, join to companies, send message from bryan to new user
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterInsertUser e)
        {
            var globalSpace = SpaceService.GetByKey("global", true);            
            if (globalSpace != null && !globalSpace.IsMember) { SpaceService.AddMember(globalSpace.Id, e.Inserted.Id, Access.Write, true); }

            var almostAnythingSpace = SpaceService.GetByKey("almost-anything", true);
            if (almostAnythingSpace != null && !almostAnythingSpace.IsMember) { SpaceService.AddMember(almostAnythingSpace.Id, e.Inserted.Id, Access.Write, true); }

            var welcomeSpace = SpaceService.GetByKey("welcome", true);
            if (welcomeSpace != null && !welcomeSpace.IsMember) { SpaceService.AddMember(welcomeSpace.Id, e.Inserted.Id, Access.Write, true); }

            var watercoolerSpace = SpaceService.GetByKey("watercooler", true);
            if (watercoolerSpace != null && !watercoolerSpace.IsMember) { SpaceService.AddMember(watercoolerSpace.Id, e.Inserted.Id, Access.Write, true); }

            var techSupport = SpaceService.GetByKey("tech-support", true);
            if (techSupport != null && !techSupport.IsMember) { SpaceService.AddMember(techSupport.Id, e.Inserted.Id, Access.Write, true); }

            var blob = BlobService.Insert(ConfigurationService.AppSetting("shield-log-path"));

            var spaceModel = new Space() { Name = "Welcome", Tags = new List<string>() { "collab" }, CreatedById = e.Inserted.Id, Avatar = blob };
            var privateWelcomeSpace = SpaceService.Insert(spaceModel);


            var bryan = UserService.GetByEmail("support@findparts.aero");
            if (bryan != null)
            {
                SpaceService.AddMember(privateWelcomeSpace.Id, bryan.Id, Access.Admin, sudo: true);
                var conversation = ConversationService.Insert(new Conversation() { CreatedById = bryan.Id }, new int[] { e.Inserted.Id });

                var message = $@"<p>Hi,</p>"
                    + $@"<p>Welcome to AvDB,</p>"
                    + $@"<p>Use AvDB to:</p>"
                    + $@"<p>Drop-In video chat, co-author files, threaded messaging, search jobs, planes, parts, repairs.</p>"
                    + $@"<p>Enjoy the Free App.</p>"
                    + $@"<p>@bryan</p>";

                MessageService.Insert(new Message
                {
                    CreatedById = bryan.Id,
                    Html = message
                }, conversation, sudo: true);
            }
            var hana = UserService.GetByEmail("hishibashi@unomaha.edu");
            if (hana != null)
            {
                SpaceService.AddMember(privateWelcomeSpace.Id, hana.Id, Access.Admin, sudo: true);
            }
            var rohan = UserService.GetByEmail("rohand9619@gmail.com");
            if (rohan != null)
            {
                SpaceService.AddMember(privateWelcomeSpace.Id, rohan.Id, Access.Admin, sudo: true);
            }

            /*
            var searchResult = SpaceService.Search(new SpaceQuery { Sudo = true });
            var iter = searchResult.GetEnumerator();
            while(iter.MoveNext())
            {
                if (!string.IsNullOrEmpty(iter.Current.Key) && iter.Current.Key.StartsWith("company_"))
                {
                    SpaceService.AddMember(iter.Current.Id, e.Inserted.Id, Access.Read, sudo: true);
                }
            }
            */
        }
    }

}