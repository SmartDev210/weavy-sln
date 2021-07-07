using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.CustomPages.Helpers;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;
using Weavy.Web.Utils;
using Weavy.Web.Owin;
using Weavy.Web.Models;
using Microsoft.Owin.Security;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// my account controller to login from findparts
    /// </summary>
    public class MyAccountController : AppController
    {
       
        [HttpGet]
        [AllowAnonymous]
        [Route("signing-in")]
        public ActionResult Index(string jwt, string path)
        {
            SigningInPageViewModel viewModel = new SigningInPageViewModel { JwtToken = jwt, Path = path };
            return View("~/Areas/CustomPages/Views/Account/SigningIn.cshtml", viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("admin/sign-in")]
        public ActionResult AdminSignIn()
        {
            
            SignInModel viewModel = new SignInModel()
            {
                Path = "/"
            };
            
            return View("~/Views/Account/AdminSignIn.cshtml", viewModel);
        }

    }
}