using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Weavy.Core;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;
using Weavy.Web.Api.Controllers;
using Weavy.Web.Api.Models;

namespace Weavy.Areas.Api.Controllers
{
    /// <summary>
    /// api controller for flag content
    /// </summary>
    [RoutePrefix("api")]
    public class FlagController : WeavyApiController
    {
        /// <summary>
        /// flag content
        /// </summary>
        /// <param name="id"> if of entity</param>
        /// <param name="entityType"> entity typpe </param>
        /// <returns></returns>
        [HttpPost]       
        [Route("flag/{id:int}/{entityType}")]
        public IHttpActionResult Flag(int id, EntityType entityType)
        {
            var adminList = RoleService.GetMembers(-1, new UserQuery { Sudo = true }).ToList(); // global system administrators 
         
            
            IEntity entity = null;
            switch (entityType)
            {
                case EntityType.Comment:
                    var comment = CommentService.Get(id, sudo: true);
                    if (comment == null) return Ok();
                    entity = comment;
                    break;
                case EntityType.Content:
                    var content = ContentService.Get(id, sudo: true);
                    if (content == null) return Ok();
                    entity = content;
                    break;
                case EntityType.Attachment:
                    var attachment = AttachmentService.Get(id, sudo: true);
                    if (attachment == null) return Ok();
                    entity = attachment;
                    break;
                case EntityType.Post:
                    var post = PostService.Get(id, sudo: true);
                    if (post == null) return Ok();
                    entity = post;
                    break;
                case EntityType.User:
                    var user = UserService.Get(id, sudo: true);
                    if (user == null) return Ok();
                    entity = user;
                    break;
                case EntityType.Space:
                    var space = SpaceService.Get(id, sudo: true);
                    if (space == null) return Ok();
                    entity = space;
                    break;
                case EntityType.Conversation:
                    var conversation = ConversationService.Get(id, sudo: true);
                    if (conversation == null) return Ok();
                    entity = conversation;
                    break;
                default:
                    return Ok();
            }
            foreach (var admin in adminList)
            {   
                var notification = new Notification();
                notification.CreatedById = User.Id;
                notification.Html = $@"<span class=""actor"">@{User.GetTitle()}</span> <span class=""subject"">flagged</span> {entityType} <span class=""context"">{entity.GetTitle()}</span>";
                notification.Text = $@"@{User.GetTitle()} flaged “{entity.GetTitle()}”";
                notification.Link = entity;
                notification.UserId = admin.Id;

                NotificationService.Insert(notification);
            }

            return Ok();
        }
    }
}