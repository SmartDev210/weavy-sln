using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Areas.Apps.Models;
using Weavy.Areas.CustomPages.Models;
using Weavy.Core;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Core.Utils;
using Weavy.Web.Controllers;
using Weavy.Web.Models;

namespace Weavy.Areas.CustomPages.Controllers
{
    /// <summary>
    /// customized space - company controller
    /// </summary>
    [Authorize]
    public class CompaniesController : WeavyController
    {
        /// <summary>
        /// company page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("companies/{id}")]
        public ActionResult Get(int id)
        {
            var space = GetSpace(id, sudo: true);
           

            return View("~/Areas/CustomPages/Views/Companies/Get.cshtml", space);
        }
        /// <summary>
        /// Customized space (company) create page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("companies/new")]
        public ActionResult Create()
        {
            var space = SpaceService.GetByKey($"company_{User.Id}");
            if (space != null)
                return RedirectToAction("Edit");

            var viewModel = new CompanyViewModel();
            return View("~/Areas/CustomPages/Views/Companies/NewCompany.cshtml", viewModel);
        }
        /// <summary>
        /// Customized space (company) create page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("companies/edit")]
        public ActionResult Edit()
        {
            var companySpace = SpaceService.GetByKey($"company_{User.Id}");
            if (companySpace == null)
            {
                companySpace = SpaceService.Search(new SpaceQuery { MemberId = User.Id, Sudo = true }).Where(x => !string.IsNullOrEmpty(x.Key) && x.Key.StartsWith("company_") && x.HasPermission(Permission.Admin, User)).FirstOrDefault();
            }

            if (companySpace == null)
                RedirectToAction("Create");

            var viewModel = new CompanyViewModel()
            {
                Avatar = companySpace.Avatar,
                Name = companySpace.Name,
                Teamname = companySpace.Teamname,
                Description = companySpace.Description,
                Tags = companySpace.Tags,
                Location = companySpace["Location"]?.ToString(),
                Website = companySpace["Website"]?.ToString(),
                Email = companySpace["Email"]?.ToString(),
                Phone = companySpace["Phone"]?.ToString(),
                Certs = companySpace["Certs"]?.ToString()
            };
            return View("~/Areas/CustomPages/Views/Companies/NewCompany.cshtml", viewModel);
        }
        /// <summary>
        /// insert company
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("companies/create")]
        public ActionResult Insert(CompanyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/CustomPages/Views/Companies/NewCompany.cshtml", viewModel);
            }

            var space = SpaceService.GetByKey($"company_{User.Id}");
            
            if (space == null)
                space = new Weavy.Core.Models.Space();

            space.Name = viewModel.Name;
            space.Avatar = viewModel.Avatar;
            space.Description = viewModel.Description;
            space.Teamname = viewModel.Teamname;
            space.Tags = viewModel.Tags;
            space["Location"] = viewModel.Location;
            space["Website"] = viewModel.Website;
            space["Email"] = viewModel.Email;
            space["Phone"] = viewModel.Phone;
            space["Certs"] = viewModel.Certs;
            

            space.CreatedById = User.Id;
            space.Key = $"company_{User.Id}";

            if (space.Id == 0)
                space = SpaceService.Insert(space);
            else space = SpaceService.Update(space);

            EntityService.Star(space, User.Id, true);

            return RedirectToAction("Get", new { id = space.Id });
        }
        /// <summary>
        /// create group chat with company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("companies/{id:int}/message")]
        public ActionResult CompanyMessage(int id)
        {
            var space = SpaceService.Get(id, sudo: true);
            if (space == null) return HttpNotFound();
            
            var adminMembers = SpaceService.GetMembers(id, new Core.Models.MemberQuery { Admin = true });
            if (adminMembers.Count() == 0) return HttpNotFound();
            List<int> members = adminMembers.Select(x => x.Id).ToList();

            if (members.Contains(User.Id))
                return HttpNotFound();

            var conversation = ConversationService.Insert(new Core.Models.Conversation
            {
                CreatedById = User.Id,
                Name = "Group chat with " + space.Name,
            }, members);

            return Redirect($"/messenger/{conversation.Id}");
        }

        /// <summary>
        /// get private channel with company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("companies/{id:int}/private-channel")]
        public ActionResult CompanyPrivateChannel(int id)
        {
            var space = SpaceService.Get(id, sudo: true);
            if (space == null) return HttpNotFound();
            var admins = SpaceService.GetMembers(id, new Core.Models.MemberQuery { Admin = true });
            if (admins.Any(x => x.Id == User.Id))
                return HttpNotFound();

            var privateSpace = SpaceService.GetByKey($"private_channel_{User.Id}_{space.Id}");
            if (privateSpace == null)
            {
                privateSpace = new Core.Models.Space
                {
                    Key = $"private_channel_{User.Id}_{space.Id}",
                    CreatedById = User.Id,
                    Name = $"Private channel for {space.Name} & {User.GetTitle()}",
                    Description = $"Private channel for {space.Name} and {User.GetTitle()}",
                    Tags = new string[] {"collab"}
                };

                privateSpace = SpaceService.Insert(privateSpace);
                foreach (var admin in admins)
                {
                    SpaceService.AddMember(privateSpace.Id, admin.Id, Core.Models.Access.Write);
                }

                var postPlugin = PluginService.GetApp<Posts>();
                var postApp = AppService.New(postPlugin.Id);
                postApp.Name = "Posts";

                AppService.Insert(postApp, privateSpace);

                var taskPlugin = PluginService.GetApp<Tasks>();
                var taskApp = AppService.New(taskPlugin.Id);
                taskApp.Name = "Tasks";

                AppService.Insert(taskApp, privateSpace);

                var filesPlugin = PluginService.GetApp<Files>();
                var filesApp = AppService.New(filesPlugin.Id);
                filesApp.Name = "Files";

                AppService.Insert(filesApp, privateSpace);

                var commentsPlugin = PluginService.GetApp<Comments>();
                var commentsApp = AppService.New(commentsPlugin.Id);
                commentsApp.Name = "Comments";

                AppService.Insert(commentsApp, privateSpace);
            }
            
            return Redirect($"/spaces/{privateSpace.Id}");
        }
    }
}