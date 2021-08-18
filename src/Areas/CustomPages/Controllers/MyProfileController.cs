using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// profile conroller to handle button actions on profile page
    /// </summary>
    [Authorize]
    public class MyProfileController : WeavyController
    {
        /// <summary>
        /// direct call with user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpGet]
        [Route("direct-call/{id:int}")]
        public ActionResult DirectCall(int id)
        {
            var bot = UserService.GetByEmail("bot@mail.back-channel.com");
            var target = UserService.Get(id, sudo: true);

            if (bot != null && target != null)
            {
                var conversation = ConversationService.Insert(new Conversation
                {
                    CreatedById = bot.Id,
                }, new int[] { target.Id });

                var callLink = "";
                callLink = $@"<a href=""/video-call/direct-call-{User.Id}-{target.Id}"" target=""_blank""><small class=""text-muted"">Please click here to have a video call with @{target.Username}</small></a>";

                var message = $@"<p>{target.GetTitle()} wants to have a video call with you."
                    + $@"<p>{callLink}</p>";

                MessageService.Insert(new Message
                {
                    CreatedById = bot.Id,
                    Html = message
                }, conversation, sudo: true);

                return RedirectToAction("Index", "VideoCall", new { roomName = $"direct-call-{User.Id}-{target.Id}" });
            }
            return HttpNotFound();
        }
        /// <summary>
        /// direct message with user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("direct-message/{id:int}")]
        public ActionResult DirectMessage(int id)
        {
            List<int> members = new List<int> { id };
           
            var conversation = ConversationService.Insert(new Conversation(), members);

            return RedirectToAction<ConversationController>(x => x.Get(conversation.Id));
        }
        /// <summary>
        /// direct message with user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("direct-space/{id:int}")]
        public ActionResult DirectSpace(int id)
        {
            var privateSpace = SpaceService.GetByKey($"direct_space_{User.Id}_{id}");
            if (privateSpace == null)
                privateSpace = SpaceService.GetByKey($"direct_space_{id}_{User.Id}");

            if (privateSpace == null)
            {
                privateSpace = new Core.Models.Space
                {
                    Key = $"direct_space_{User.Id}_{id}",
                    CreatedById = User.Id,
                    Name = $"Private channel for {UserService.Get(id).GetTitle()} & {User.GetTitle()}",
                    Description = $"Private channel for {UserService.Get(id).GetTitle()} and {User.GetTitle()}",
                };

                privateSpace = SpaceService.Insert(privateSpace);
                SpaceService.AddMember(privateSpace.Id, id, Core.Models.Access.Admin);
            }

            return Redirect($"/spaces/{privateSpace.Id}");
        }
    }
}