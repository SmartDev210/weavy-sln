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
    [Guid("3E789260-81CA-4AC9-83D1-E22F989AE65C")]
    [Plugin(Description = "A hook that automatically add users to company space")]
    public class AfterCompanyInsertedHook : Hook, IHook<AfterInsertSpace>
    {
        /// <summary>
        /// A hook that automatically add users to company
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterInsertSpace e)
        {
            if (!e.Inserted.Key.StartsWith("company_")) return;

            var searchResult = UserService.Search(new UserQuery {  });
            var iter = searchResult.GetEnumerator();
            while(iter.MoveNext())
            {
                var admins = SpaceService.GetMembers(e.Inserted.Id, new MemberQuery { Admin = true });
                if (!admins.Any(x => x.Id == iter.Current.Id))
                {
                    SpaceService.AddMember(e.Inserted.Id, iter.Current.Id, Access.Read, sudo: true);
                }
            }
        }
    }

}