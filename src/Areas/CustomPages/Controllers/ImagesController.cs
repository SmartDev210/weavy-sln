using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Core.Services;

namespace Weavy.Areas.CustomPages.Controllers
{
    public class ImagesController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("avatar-images/{id:int}/avatar-{size:int}")]
        public ActionResult PublicAvatarImage(int id, int size)
        {
            var avatar = BlobService.Get(id, sudo: true);
            
            if (avatar == null)
            {
                return File("/img/space.svg", "image/svg+xml");
            } else
            {   
                var stream = new MemoryStream();
                
                BlobService.DownloadTo(avatar, stream);
                return File(stream.ToArray(), avatar.MediaType);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("people-images/{id:int}/avatar-{size:int}")]
        public ActionResult PeopleAvatar(int id, int size)
        {
            var user = UserService.Get(id, sudo: true);
            if (user.Avatar == null)
            {
                return File("/img/user.svg", "image/svg+xml");
            }
            else
            {
                var stream = new MemoryStream();

                BlobService.DownloadTo(user.Avatar, stream);
                return File(stream.ToArray(), user.Avatar.MediaType);
            }
        }
    }
}