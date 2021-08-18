using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Models
{
    /// <summary>
    /// Custom Profile
    /// </summary>
    [Serializable]
    [Guid("95578b7a-f04b-49ad-b611-3dc9508a28a1")]
    public class CustomProfile : ProfileBase
    {
        /// <summary>
        /// LinkedIn Url
        /// </summary>
        [Display(Name = "LinkedIn URL")]
        [DataType(DataType.Url)]
        public string LinkedIn { get; set; }
        /// <summary>
        /// Instagram Handle
        /// </summary>
        [Display(Name = "Instagram Handle")]
        [DataType(DataType.Url)]
        public string Instagram { get; set; }
        /// <summary>
        /// Bio
        /// </summary>
        [Display(Name = "Bio", Description = "A little bit about yourself.")]
        [DataType(DataType.MultilineText)]
        public virtual string Bio { get; set; }

    }
}