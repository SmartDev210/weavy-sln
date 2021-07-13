using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.CustomPages.Helpers;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;
using Weavy.Web.Utils;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// jitsi video call controller
    /// </summary>
    public class SpacesController : AppController
    {
        /// <summary>
        /// add member to space and delete converstaion
        /// </summary>
        /// <param name="spaceId"></param>
        /// <param name="userId"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("spaces/{spaceId}/join-member/{userId}/{conversationId}")]
        public ActionResult JoinMember(int spaceId, int userId, int conversationId)
        {
            
            var space = SpaceService.Get(spaceId, sudo: true);
            var user = UserService.Get(userId);
            
            if (space == null || user == null) return HttpNotFound();

            JoinMemberViewModel viewModel = new JoinMemberViewModel
            {
                Join = true,
                UserName = user.GetTitle(),
                SpaceName = space.Name
            };
            var admins = SpaceService.GetMembers(spaceId, new Core.Models.MemberQuery { Admin = true, Sudo = true }).ToList();
            if (User.IsAdmin || admins.Any(y => y.Access == Core.Models.Access.Admin && y.Id == User.Id))
            {
                SpaceService.AddMember(spaceId, userId);
                ConversationService.Delete(conversationId, sudo: true);
                viewModel.Success = true;
            } else
            {   
                viewModel.Success = false;
            }
            return View("~/Areas/CustomPages/Views/Spaces/joinMember.cshtml", viewModel);
        }
        /// <summary>
        /// delete conversation
        /// </summary>
        /// <param name="spaceId"></param>
        /// <param name="userId"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("spaces/{spaceId}/deny-member/{userId}/{conversationId}")]
        public ActionResult DenyMember(int spaceId, int userId, int conversationId)
        {
            var space = SpaceService.Get(spaceId, sudo: true);
            var user = UserService.Get(userId);
            if (space == null || user == null) return HttpNotFound();
            JoinMemberViewModel viewModel = new JoinMemberViewModel
            {
                Join = false,
                Success = true,
                UserName = user.GetTitle(),
                SpaceName = space.Name
            };

            ConversationService.Delete(conversationId, sudo: true);
            
            return View("~/Areas/CustomPages/Views/Spaces/joinMember.cshtml", viewModel);
        }
    }
}