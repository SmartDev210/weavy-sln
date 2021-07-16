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

            var welcomeSpace = SpaceService.GetByKey("welcome", true);
            if (welcomeSpace != null && !welcomeSpace.IsMember) { SpaceService.AddMember(welcomeSpace.Id, e.User.Id, Access.Write, true); }

            var watercoolerSpace = SpaceService.GetByKey("watercooler", true);
            if (watercoolerSpace != null && !watercoolerSpace.IsMember) { SpaceService.AddMember(watercoolerSpace.Id, e.User.Id, Access.Write, true); }

            var techSupport = SpaceService.GetByKey("tech-support", true);
            if (techSupport != null && !techSupport.IsMember) { SpaceService.AddMember(techSupport.Id, e.User.Id, Access.Write, true); }
        }
    }

}