using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Models
{
    /// <summary>
    /// Clubhouse aviation homepage with marketplace categories
    /// </summary>
    public class ClubhouseAviationMarketplaceHomePageViewModel
    {
        /// <summary>
        /// List of public spaces
        /// </summary>
        public IEnumerable<Space> PubSpaces { get; set; }
    }
}