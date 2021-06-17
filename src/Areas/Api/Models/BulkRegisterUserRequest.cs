using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weavy.Areas.Api.Models
{
    /// <summary>
    /// retrive user or create if not exist request by email
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
    }
}