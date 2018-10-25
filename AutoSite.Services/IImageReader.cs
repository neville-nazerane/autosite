using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Services
{
    public interface IImageReader
    {

        Task<string[]> GetTextAsync(IFormFile image, bool isTable = false);

    }
}
