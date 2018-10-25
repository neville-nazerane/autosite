using AutoSite.Core.Entities;
using AutoSite.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSite.Website.Controllers
{

    public class SitesController : Controller
    {
        private readonly ISiteContentRepository repository;

        public SitesController(ISiteContentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index() => View(repository.Get());

        [HttpPost]
        public IActionResult Add(SiteContent site)
        {
            var added = repository.Add(site);
            return Redirect("~/manage/" + added.Id);
        }

    }
}
