using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Models
{
    /// <summary>
    /// Custom home page view model
    /// </summary>
    public class AviationMarketplaceHomePageViewModel
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AviationMarketplaceHomePageViewModel()
        {
            JoinedSpaces = new List<Space>();
            PodsSpaces = new List<Space>();
            GigsSpaces = new List<Space>();
            PubSpaces = new List<Space>();
            Notifications = new List<Notification>();
            Stars = new List<Entity>();
        }
        /// <summary>
        /// List of Joined spaces
        /// </summary>
        public IEnumerable<Space> JoinedSpaces { get; set; }
        /// <summary>
        /// List of spaces with Tag "Pods"
        /// </summary>
        public IEnumerable<Space> PodsSpaces { get; set; }
        /// <summary>
        /// List of spaces with Tag "Gigs"
        /// </summary>
        public IEnumerable<Space> GigsSpaces { get; set; }
        /// <summary>
        /// List of public spaces
        /// </summary>
        public IEnumerable<Space> PubSpaces { get; set; }
        /// <summary>
        /// List of notifications
        /// </summary>
        public IEnumerable<Notification> Notifications { get; set; }
        /// <summary>
        /// List of stars
        /// </summary>
        public IEnumerable<Entity> Stars { get; set; }
    }
}