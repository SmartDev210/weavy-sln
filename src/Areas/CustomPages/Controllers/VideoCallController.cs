using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.CustomPages.Helpers;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;
using Weavy.Web.Utils;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// jitsi video call controller
    /// </summary>
    public class VideoCallController : WeavyController
    {
        /// <summary>
        /// open jitsi call room
        /// </summary>
        /// <param name="roomName">jitsi room id</param>
        /// <returns></returns>
        // GET: video-call/{roomName
        [HttpGet]
        [Route("video-call/{roomName}")]
        public ActionResult Index(string roomName)
        {   
            // Read private key from file.
            var rsaPrivateKey = JaaSJwtBuilder.ReadPrivateKeyFromFile(ConfigurationService.AppSetting("JitsiPrivateKeyPath"));

            // Create new JaaSJwtBuilder and setup the claims and sign using the private key.
            var token = JaaSJwtBuilder.Builder()
                                .WithDefaults()
                                .WithApiKey(ConfigurationService.AppSetting("JitsiApiKey"))
                                .WithUserName(User.Username)
                                .WithUserEmail(User.Email)
                                .WithUserAvatar(User.Avatar != null ? User.Avatar.Link : "")
                                .WithAppID(ConfigurationService.AppSetting("JitsiAppId"))
                                .SignWith(rsaPrivateKey);

            var viewModel = new VideoCallViewModel()
            {
                Token = token,
                RoomName = $"{ConfigurationService.AppSetting("JitsiAppId")}/{roomName}"
            };

            return View("~/Areas/CustomPages/Views/VideoCall/Index.cshtml", viewModel);
        }        
    }
}