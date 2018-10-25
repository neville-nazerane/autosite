using AutoSite.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Services
{
    public interface IEntityUnderstanding
    {

        Task<LuisResult> GetAsync(string query);

    }
}
