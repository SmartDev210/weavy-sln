using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.Apps.Models;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// My Custom controller for customized homepage and some other pages
    /// </summary>
    public class MyHomeController : AppController
    {
        /// <summary>
        /// Original home page
        /// </summary>
        /// <returns></returns>
        // GET: aviation-marketplace
        [Route("aviation-marketplace")]        
        public ActionResult AviationMarketplace()
        {
            var joined = SpaceService.GetVisited(10);

            var pods = SpaceService.Search(new SpaceQuery { Tag = "pods", Top = 10, Sudo = true });
            var gigs = SpaceService.Search(new SpaceQuery { Tag = "gigs", Top = 10, Sudo = true });
            var pubs = SpaceService.Search(new SpaceQuery { Top = 10, Sudo = true });

            var notifications = NotificationService.Search(new NotificationQuery
            {
                OrderBy = "Id DESC",
                SearchRead = null,
                Top = PageSizes.First(),
                
            }).ToList();
            var stars = EntityService.Search(new EntityQuery
            {
                StarredById = User.Id
            }).ToList();
            AviationMarketplaceHomePageViewModel viewModel = new AviationMarketplaceHomePageViewModel
            {
                JoinedSpaces = joined,
                PodsSpaces = pods.Where(x => x.Tags.Any(y => y.ToLower() == "pods")),
                GigsSpaces = gigs.Where(x => x.Tags.Any(y => y.ToLower() == "gigs")),
                PubSpaces = pubs.ToList(),
                Notifications = notifications,
                Stars = stars
            };

            return View("~/Areas/CustomPages/Views/MyHome/Index.cshtml", viewModel);
        }
        /// <summary>
        /// Aviation clubhouse homepage
        /// </summary>
        /// <returns></returns>
        // GET: aviation-clubhouse
        [Route("aviation-clubhouse")]
        public ActionResult AviationClubhouse()
        {            
            var joined = SpaceService.Search(new SpaceQuery { Tag = "aviation", Top = 100, Sudo = true, MemberId = User.Id }).Where(x => x.Tags.Any(y => y.ToLower() == "aviation")).ToList();

            var aviationClubs = SpaceService.Search(new SpaceQuery { Tag = "aviation", Top = 100, Sudo = true }).Where(x => !x.IsMember).ToList();

            AviationClubhouseHomePageViewModel viewModel = new AviationClubhouseHomePageViewModel
            {
                JoinedSpaces = joined,
                AviationSpaces = aviationClubs.Where(x => x.Tags.Any(y => y.ToLower() == "aviation")).ToList()
            };
            
            return View("~/Areas/CustomPages/Views/MyHome/AviationClubhouse.cshtml", viewModel);
        }
        /// <summary>
        /// Marketplace homepage with categories
        /// </summary>
        /// <returns></returns>
        [Route("clubhouse-aviation-marketplace")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ClubhouseAviationMarketplace()
        {   
            var pubs = SpaceService.Search(new SpaceQuery { Top = 20, Sudo = true });
            
            ClubhouseAviationMarketplaceHomePageViewModel viewModel = new ClubhouseAviationMarketplaceHomePageViewModel
            {
                PubSpaces = pubs.ToList()
            };

            return View("~/Areas/CustomPages/Views/MyHome/ClubhouseAviatonMarketplace.cshtml", viewModel);
        }
    }
}