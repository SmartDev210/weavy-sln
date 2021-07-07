using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weavy.Areas.CustomPages.Models
{
    public class JoinMemberViewModel
    {
        public bool Success { get; set; }
        public bool Join { get; set; }
        public string SpaceName { get; set; }
        public string UserName { get; set; }
    }
}