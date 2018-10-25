using AutoSite.Core.Entities;
using AutoSite.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSite.Website.Controllers
{

    [ApiController, Route("api/sites")]
    public class SitesApiController : Controller
    {
        private readonly ISiteContentRepository repository;

        public SitesApiController(ISiteContentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{name}")]
        public ActionResult<SiteContent> Index(string name)
        {
            var result = repository.GetByName(name);
            if (result == null) return NotFound();
            return result;
        }

        [HttpGet("type/{id}")]
        public ActionResult<string> GetType(int id)
        {
            var e = (PropertyTypes) id;
            return e.ToString();
        }
    }
}
