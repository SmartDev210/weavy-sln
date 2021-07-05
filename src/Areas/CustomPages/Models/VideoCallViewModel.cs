using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weavy.Areas.CustomPages.Models
{
    /// <summary>
    /// jitsi video call view model
    /// </summary>
    public class VideoCallViewModel
    {
        /// <summary>
        /// jitsi jwt token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// jisti video call room name
        /// </summary>
        public string RoomName { get; set; }

    }
}