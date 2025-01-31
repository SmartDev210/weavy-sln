﻿using System;
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

            string[] masterSpaces = new string[0];
            if (!string.IsNullOrEmpty(ConfigurationService.AppSetting("master-spaces")))
                masterSpaces = ConfigurationService.AppSetting("master-spaces").Split(',');

            foreach (var masterSpace in masterSpaces)
            {
                var space = SpaceService.GetByKey(masterSpace, true);
                if (space != null && !space.IsMember) { 
                    SpaceService.AddMember(space.Id, e.Inserted.Id, Access.Write, true);
                    EntityService.Unfollow(space, e.Inserted.Id, true);
                }
            }

            var blob = BlobService.Insert(ConfigurationService.AppSetting("shield-log-path"));

            var spaceModel = new Space() { Name = "My First Space", Tags = new List<string>() { "collab" }, CreatedById = e.Inserted.Id, Avatar = blob };
            var privateWelcomeSpace = SpaceService.Insert(spaceModel);

            var postPlugin = PluginService.GetApp<Posts>();
            var postApp = AppService.New(postPlugin.Id);
            postApp.Name = "Posts";
            postApp = AppService.Insert(postApp, privateWelcomeSpace, sudo: true);

            string[] signupMembers = new string[0];
            if (!string.IsNullOrEmpty(ConfigurationService.AppSetting("signup-members")))
                signupMembers = ConfigurationService.AppSetting("signup-members").Split(',');

           
            List<int> memberIds = new List<int>();
            foreach (var signupMember in signupMembers)
            {
                var member = UserService.GetByEmail(signupMember, true);
                if (member != null)
                {   
                    SpaceService.AddMember(privateWelcomeSpace.Id, member.Id, Access.Admin, sudo: true);
                    memberIds.Add(member.Id);
                }
            }
            memberIds.Add(e.Inserted.Id);

            var bryan = UserService.GetByEmail(ConfigurationService.AppSetting("bryan-email"), sudo: true);
            if (bryan != null)
            {
                var post = new Post
                {
                    CreatedById = bryan.Id,                    
                    Text = $"Welcome to buddy.aero - feel free to ask us any question(s).{Environment.NewLine}{Environment.NewLine}Best Regards,{Environment.NewLine}{Environment.NewLine}Bryan and Hana",
                };
                PostService.Insert(post, postApp, sudo: true);
                if (memberIds.Contains(bryan.Id))
                    memberIds.Remove(bryan.Id);

                var conversation = ConversationService.Insert(new Conversation() { Name = "Welcome", CreatedById = bryan.Id }, memberIds);

                var message =  $@"Welcome to buddy.aero - feel free to ask us any question(s).<br/><br/>"
                    + $@"Best Regards,<br/>"
                    + $@"Bryan and Hana</p>";

                MessageService.Insert(new Message
                {
                    CreatedById = bryan.Id,
                    Html = message
                }, conversation, sudo: true);
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