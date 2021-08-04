using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Models
{
    public class CompanyViewModel
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>        
        [Display(Name = "Company Name", Description = "Name of the company.")]
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the team name (used for @mentions).
        /// </summary>        
        [Display(Name = "Handle", Description = "Used to @mention all members at once.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Invalid handle. Valid characters are [a-zA-Z0-9_].")]
        [StringLength(32)]
        public string Teamname { get; set; }
        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        [Display(Name = "Company Description", Description = "A short description of the company.")]
        [StringLength(512)]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the location text.
        /// </summary>
        [Display(Name = "Location", Description = "Address of the company.")]
        [StringLength(512)]
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the website url.
        /// </summary>
        [Display(Name = "Website", Description = "Website of the company.")]
        [StringLength(512)]
        public string Website { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Display(Name = "Email", Description = "Email of the company.")]
        [StringLength(512)]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        [Display(Name = "Phone", Description = "Phone number of the company.")]
        [StringLength(512)]
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the certs
        /// </summary>
        [Display(Name = "Certs", Description = "Certs of the company.")]
        [StringLength(512)]
        public string Certs { get; set; }
        /// <summary>
        /// Gets or sets the list of tags for the space.
        /// </summary>
        [Display(Name = "Services, Capabilities & Tags", Description = "Some keywords for the search index.")]
        [UIHint("Tags")]
        public IList<string> Tags { get; set; }
        /// <summary>
        /// Gets or sets the avatar image.
        /// </summary>        
        [Display(Name = "Company picture", Description = "Recommended dimensions of at least 256x256 pixels.", Prompt = "Select picture")]
        [Image(EntityType.Space)]
        [JsonIgnore]
        public Blob Avatar { get; set; }
        
        
    }
}