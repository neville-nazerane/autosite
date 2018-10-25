using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Services
{
    public interface ICustomVisionReader
    {

        Task<string> GetPrediction(IFormFile image);

    }
}
