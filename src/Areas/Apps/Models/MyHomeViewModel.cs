using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Areas.Apps.Models
{
    public class MyHomeViewModel
    {
        public MyHomeViewModel()
        {
            JoinedSpaces = new List<Space>();
            PodsSpaces = new List<Space>();
            GigsSpaces = new List<Space>();
            PubSpaces = new List<Space>();
            Notifications = new List<Notification>();
            Stars = new List<Entity>();
        }
        public IEnumerable<Space> JoinedSpaces { get; set; }
        public IEnumerable<Space> PodsSpaces { get; set; }
        public IEnumerable<Space> GigsSpaces { get; set; }
        public IEnumerable<Space> PubSpaces { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
        public IEnumerable<Entity> Stars { get; set; }
    }
}