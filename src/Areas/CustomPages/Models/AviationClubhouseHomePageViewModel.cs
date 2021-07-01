using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weavy.Core.Models;

namespace Weavy.Areas.CustomPages.Models
{
    /// <summary>
    /// aviation clubhouse homepage viewmodel
    /// </summary>
    public class AviationClubhouseHomePageViewModel
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AviationClubhouseHomePageViewModel()
        {
            JoinedSpaces = new List<Space>();
            AviationSpaces = new List<Space>();
        }
        /// <summary> 
        /// joined aviation space list
        /// </summary>
        public IEnumerable<Space> JoinedSpaces { get; set; }
        /// <summary>
        /// unjoined aviation space list
        /// </summary>
        public IEnumerable<Space> AviationSpaces { get; set; }
    }
}