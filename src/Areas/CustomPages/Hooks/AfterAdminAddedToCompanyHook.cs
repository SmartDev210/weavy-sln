using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Events;
using Weavy.Core.Models;
using Weavy.Core.Services;

namespace Weavy.Areas.CustomPages.Hooks
{
    /// <summary>
    /// A hook that automatically add users in findparts/mrofinder
    /// </summary>
    [Serializable]
    [Guid("F45BD2FB-910B-4FE1-A2E7-3BEE040E36E4")]
    [Plugin(Description = "A hook that automatically users in findparts/mrofinder")]
    public class AfterAdminAddedToCompanyHook : Hook, IHook<AfterAddMember>
    {
        /// <summary>
        /// when the added member is admin, add tha process that user in findparts/mrofinder
        /// </summary>
        /// <param name="e"></param>
        public void Handle(AfterAddMember e)
        {   
            if (e.Space.Key.StartsWith("company_") && e.Member.Access == Access.Admin)
            {
                var creator = UserService.Get(e.Space.CreatedById, sudo: true);
                

                HttpClient client = new HttpClient();

                _ = client.PostAsJsonAsync($"{ConfigurationService.AppSetting("findparts-url")}/web-api/add-vendor-user", new
                {
                    CreatorEmail = creator.Email,
                    Email = e.Member.Email
                });
            }
        }
    }
}