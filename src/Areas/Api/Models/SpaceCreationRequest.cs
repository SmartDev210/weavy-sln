using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weavy.Areas.Api.Models
{
    /// <summary>
    /// Customized Space creation request model
    /// </summary>
    public class SpaceCreationRequest
    {
        /// <summary>
        /// unique key of space
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// name of space
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// user id of space creating
        /// </summary>
        public int user { get; set; }
    }
}