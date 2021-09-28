using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that automatically add users to company space when it's being created
    /// </summary>
    [Serializable]
    [Guid("3E789260-81CA-4AC9-83D1-E22F989AE65C")]
    [Plugin(Description = "A hook that automatically add users to company space when it's being created. And also create a vendor in findparts/mrofinder")]
    public class AfterCompanyInsertedHook : Hook, IHook<AfterInsertSpace>
    {
        /// <summary>
        /// A hook that automatically add users to company
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterInsertSpace e)
        {
            
            var creator = UserService.Get(e.Inserted.CreatedById, sudo: true);
            if (e.Inserted.Key.IsNullOrEmpty() || !e.Inserted.Key.StartsWith("company_")) return;

            /*
            var searchResult = UserService.Search(new UserQuery { });
            var iter = searchResult.GetEnumerator();
            while (iter.MoveNext())
            {
                var admins = SpaceService.GetMembers(e.Inserted.Id, new MemberQuery { Admin = true });
                if (!admins.Any(x => x.Id == iter.Current.Id))
                {
                    try
                    {
                        EntityService.Unfollow<Space>(e.Inserted, iter.Current.Id, sudo: true);
                    } catch(Exception)
                    {

                    }
                    
                    SpaceService.AddMember(e.Inserted.Id, iter.Current.Id, Access.Read, sudo: true);
                }
            }
            */
            HttpClient client = new HttpClient();
            
            _ = client.PostAsJsonAsync($"{ConfigurationService.AppSetting("findparts-url")}/web-api/update-vendor", new
            {
                UserEmail = creator.Email,
                Name = e.Inserted.Name,
                Description = e.Inserted.Description,
                Location = e.Inserted["Location"],
                Website = e.Inserted["Website"],
                Email = e.Inserted["Email"],
                CompanyId = e.Inserted.Id
            });
        }
    }

}