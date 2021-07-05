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
    [Plugin(Description = "A hook that automatically add users to global spaces")]
    public class AfterSignInHook : Hook, IHook<AfterSignIn>
    {
        /// <summary>
        /// A hook that automatically add users to global spaces
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterSignIn e)
        {
            var globalSpace = SpaceService.GetByKey("global", true);            
            if (globalSpace != null && !globalSpace.IsMember) { SpaceService.AddMember(globalSpace.Id, e.User.Id, Access.Write, true); }

            var almostAnythingSpace = SpaceService.GetByKey("almost-anything", true);
            if (almostAnythingSpace != null && !almostAnythingSpace.IsMember) { SpaceService.AddMember(almostAnythingSpace.Id, e.User.Id, Access.Write, true); }

            var chWelcomeSpace = SpaceService.GetByKey("ch-welcome", true);
            if (chWelcomeSpace != null && !chWelcomeSpace.IsMember) { SpaceService.AddMember(chWelcomeSpace.Id, e.User.Id, Access.Write, true); }

            var foodSpace = SpaceService.GetByKey("food", true);
            if (foodSpace != null && !foodSpace.IsMember) { SpaceService.AddMember(foodSpace.Id, e.User.Id, Access.Write, true); }

            var funnyShtSpace = SpaceService.GetByKey("funny-sht", true);
            if (funnyShtSpace != null && !funnyShtSpace.IsMember) { SpaceService.AddMember(funnyShtSpace.Id, e.User.Id, Access.Write, true); }

            var humanConnectionSpace = SpaceService.GetByKey("human-connection", true);
            if (humanConnectionSpace != null && !humanConnectionSpace.IsMember) { SpaceService.AddMember(humanConnectionSpace.Id, e.User.Id, Access.Write, true); }

            var wellnessSpace = SpaceService.GetByKey("wellness", true);
            if (wellnessSpace != null && !wellnessSpace.IsMember) { SpaceService.AddMember(wellnessSpace.Id, e.User.Id, Access.Write, true); }
        }
    }

}