using AutoSite.Core.Entities;
using AutoSite.Core.Models;
using AutoSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSite.Website.Controllers
{

    public class SitesManagementController : Controller
    {
        private readonly ISiteContentRepository repository;
        private readonly IImageReader reader;

        public SitesManagementController(ISiteContentRepository repository, IImageReader reader)
        {
            this.repository = repository;
            this.reader = reader;
        }

        [HttpGet("Manage/{id}")]
        public IActionResult Index(int id) => View(repository.Get(id));
        
        [HttpPost]
        public IActionResult AddClassItem(ClassItem item)
        {
            if (repository.Add(item) == null) return this.ValidateAndBadRequest();
            return RedirectToAction(nameof(Index), new { id = item.SiteContentId });
        }

        [HttpGet]
        public IActionResult EditClassItem(int id) => View(repository.GetClassItem(id));

        [HttpPost]
        public IActionResult EditClassItem(ClassItem item)
        {
            if (repository.Update(item) == null) return this.ValidateAndBadRequest();
            return RedirectToAction(nameof(Index), new { id = item.SiteContentId });
        }

        [HttpPost]
        public async Task<IActionResult> ImportPropertyItems([FromForm]ImportClassItem item)
        {
            await repository.ImportAsync(item);
            int id = repository.GetIdFromClassItem(item.ClassId);
            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPropertyItem(PropertyItem item)
        {
            if (await repository.AddAsync(item) == null)
                return this.ValidateAndBadRequest();
            int id = repository.GetIdFromPropertyItem(item.Id);
            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpGet]
        public IActionResult Edit(int id) => View(repository.Get(id));

        [HttpPost]
        public IActionResult Edit(SiteContent content)
        {
            SiteContent result = repository.Update(content);
            if (result == null)
                return this.ValidateAndView();
            else return RedirectToAction(nameof(Index), new { result.Id });
        }

        [HttpGet]
        public IActionResult EditPropertyItem(int id) => View(repository.GetPropertyItem(id));

        [HttpPost]
        public IActionResult EditPropertyItem(PropertyItem item)
        {
            if (repository.Update(item) == null) return this.ValidateAndBadRequest();
            return RedirectToAction(nameof(Index), new { id = repository.GetIdFromPropertyItem(item.Id) });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            repository.Delete(id);
            return Redirect("~/");
        }

        [HttpGet]
        public IActionResult DeleteClassItem(int id)
        {
            int siteId = repository.GetIdFromClassItem(id);
            repository.DeleteClass(id);
            return RedirectToAction(nameof(Index), new { id = siteId });
        }

        [HttpGet]
        public IActionResult DeletePropertyItem(int id)
        {
            int siteId = repository.GetIdFromPropertyItem(id);
            repository.DeleteProperty(id);
            return RedirectToAction(nameof(Index), new { id = siteId });
        }

    }
}
