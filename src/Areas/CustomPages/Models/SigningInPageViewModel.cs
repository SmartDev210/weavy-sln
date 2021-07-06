using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weavy.Areas.CustomPages.Models
{
    public class SigningInPageViewModel
    {
        public string JwtToken { get; set; }
        public string Path { get; set; }
    }
}