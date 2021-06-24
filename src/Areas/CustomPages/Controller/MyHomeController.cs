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
        public ActionResult Index()
        {
            var joined = SpaceService.GetVisited(10);

            var pods = SpaceService.Search(new SpaceQuery { Tag = "pods", Top = 10 });
            var gigs = SpaceService.Search(new SpaceQuery { Tag = "gigs", Top = 10 });
            var pubs = SpaceService.Search(new SpaceQuery { Top = 10 });

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
            MyHomeViewModel viewModel = new MyHomeViewModel
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
        /// Marketplace homepage with categories
        /// </summary>
        /// <returns></returns>
        [Route("clubhouse-aviation-marketplace")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ClubhouseAviationMarketplace()
        {   
            var pubs = SpaceService.Search(new SpaceQuery { Top = 20 });
            
            ClubhouseAviationHomePageViewModel viewModel = new ClubhouseAviationHomePageViewModel
            {
                PubSpaces = pubs.ToList()
            };

            return View("~/Areas/CustomPages/Views/MyHome/ClubhouseAviatonMarketplace.cshtml", viewModel);
        }
    }
}