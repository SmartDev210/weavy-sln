using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Models
{
    public class VendorListItemStats
    {
        public int VendorCount { get; set; } = 0;
        public int PartCount { get; set; } = 0;
        public int AchievementCount { get; set; } = 0;
    }
    /// <summary>
    /// Clubhouse aviation homepage with marketplace categories
    /// </summary>
    public class ClubhouseAviationMarketplaceHomePageViewModel
    {
        /// <summary>
        /// List of public spaces
        /// </summary>
        public IEnumerable<Space> PubSpaces { get; set; }
        public IEnumerable<Space> Joined { get; set; }
        public Space ModeratorSpace { get; set; }
        public VendorListItemStats Portal0Stat { get; set; }
        public VendorListItemStats Portal1Stat { get; set; }
    }
}