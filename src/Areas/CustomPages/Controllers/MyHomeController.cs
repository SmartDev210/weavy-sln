using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.Apps.Models;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;
using Weavy.Web.Controllers;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// My Custom controller for customized homepage and some other pages
    /// </summary>
    public class MyHomeController : WeavyController
    {
        /// <summary>
        /// Original home page
        /// </summary>
        /// <returns></returns>
        // GET: aviation-marketplace
        [Route("aviation-marketplace")]        
        public ActionResult AviationMarketplace()
        {

            return RedirectToActionPermanent("ClubhouseAviationMarketplace");

            var joined = SpaceService.GetVisited(10);

            var pods = SpaceService.Search(new SpaceQuery { Text = "tag:pods", Top = 10, Sudo = true, MemberId = User.Id });
            var gigs = SpaceService.Search(new SpaceQuery { Text = "tag:gigs", Top = 10, Sudo = true, MemberId = User.Id });
            var pubs = SpaceService.Search(new SpaceQuery { Top = 10, Sudo = true, MemberId = User.Id });

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

            return RedirectToActionPermanent("ClubhouseAviationMarketplace");

            var joined = SpaceService.Search(new SpaceQuery { Text= "tag:aviation", Top = 100, Sudo = true, MemberId = User.Id }).ToList();

            // var aviationClubs = SpaceService.Search(new SpaceQuery { Top = 100, Sudo = true }).Where(x => !x.IsMember).ToList();

            AviationClubhouseHomePageViewModel viewModel = new AviationClubhouseHomePageViewModel
            {
                JoinedSpaces = joined,
                //AviationSpaces = aviationClubs
            };
            
            return View("~/Areas/CustomPages/Views/MyHome/AviationClubhouse.cshtml", viewModel);
        }
        /// <summary>
        /// Marketplace homepage with categories
        /// </summary>
        /// <returns></returns>
        [Route("aviation-influencer-marketplace")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ClubhouseAviationMarketplace()
        {
            var joined = SpaceService.Search(new SpaceQuery { Text="tag:collab", Top = 100, Sudo = true, MemberId = User.Id });
            var pubs = SpaceService.Search(new SpaceQuery { Text = "tag:master", Top = 100, Sudo = true });

            ClubhouseAviationMarketplaceHomePageViewModel viewModel = new ClubhouseAviationMarketplaceHomePageViewModel
            {
                Joined = joined.ToList(),
                PubSpaces = pubs.ToList(),
                ModeratorSpace = SpaceService.GetByKey(ConfigurationService.AppSetting("moderator-space"), sudo: true)
            };

            var sharedJsonPath = ConfigurationService.AppSetting("SharedJsonPath");
            if (System.IO.File.Exists($"{sharedJsonPath}stats-0.json"))
            {
                var content = System.IO.File.ReadAllText($"{sharedJsonPath}stats-0.json");
                viewModel.Portal0Stat = JsonConvert.DeserializeObject<VendorListItemStats>(content);
            }
            else
            {
                viewModel.Portal0Stat = new VendorListItemStats();
            }

            if (System.IO.File.Exists($"{sharedJsonPath}stats-1.json"))
            {
                var content = System.IO.File.ReadAllText($"{sharedJsonPath}stats-1.json");
                viewModel.Portal1Stat = JsonConvert.DeserializeObject<VendorListItemStats>(content);
            } else
            {
                viewModel.Portal1Stat = new VendorListItemStats();
            }

            return View("~/Areas/CustomPages/Views/MyHome/ClubhouseAviatonMarketplace.cshtml", viewModel);
        }
        /// <summary>
        /// return filtered vendors page
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [Route("filter-companies")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult FilterCompanies(string tags)
        {
            List<Space> result = new List<Space>();
            if (tags.IsNullOrWhiteSpace())
            {
                result = SpaceService.Search(new SpaceQuery { Text="tag:master", Top = 30, Sudo = true, MemberId = User.Id }).ToList();
            } else {
                var searchText = "tag:master | ";
                foreach (var tag in tags.Split(','))
                {
                    searchText += $"tag:{tag} | ";
                }
                searchText = searchText.RemoveTrailing(" | ");
                result = SpaceService.Search(new SpaceQuery { Top = 100, Text = searchText, Sudo = true, MemberId = User.Id}).ToList();
            }   
            
            return PartialView("~/Areas/CustomPages/Views/MyHome/Partials/_FilteredSpaces.cshtml", result);
        }
        /// <summary>
        /// return filtered vendors page
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [Route("filter-aviation-companies")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult FilterAviationCompanies(string tags)
        {
           
            var searchText = "tag:aviation";
            if (!tags.IsNullOrWhiteSpace())
            {
                foreach (var tag in tags.Split(','))
                {
                    searchText += $" | tag:{tag}";
                }
             
            }
            var joined = SpaceService.Search(new SpaceQuery { Top = 100, Text = searchText, Sudo = true, MemberId = User.Id }).ToList();
            // var aviationClubs = SpaceService.Search(new SpaceQuery { Top = 100, Text = searchText, Sudo = true }).Where(x => !x.IsMember).ToList();
            AviationClubhouseHomePageViewModel viewModel = new AviationClubhouseHomePageViewModel
            {
                JoinedSpaces = joined,
                // AviationSpaces = aviationClubs
            };

            return PartialView("~/Areas/CustomPages/Views/MyHome/Partials/_FilteredAviationCompanies.cshtml", viewModel);
        }

        [Route("experts")]
        [HttpGet]
        public ActionResult Experts()
        {
            return RedirectToActionPermanent("ClubhouseAviationMarketplace");

            return View("~/Areas/CustomPages/Views/MyHome/Experts.cshtml");
        }
    }
}