using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.Exceptions;

namespace TicketManagement.Core.EventManagement.Services
{
    internal sealed class ImageBase64Service : IImageBase64Service
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageBase64Service(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> SaveImgFromBase64(string base64String)
        {
            var fileExtension = GetFileExtensionFromBase64String(base64String);

            var imagesFolder = _configuration["EventImagesFolder"];

            var randomFileName = Path.GetRandomFileName();

            randomFileName = Path.GetFileNameWithoutExtension(randomFileName) + "." + fileExtension;

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, imagesFolder, randomFileName);

            var pureBase64String = GetPureBase64String(base64String);

            var bytes = Convert.FromBase64String(pureBase64String);

            await using (var fileStream = File.Create(filePath))
            {
                await fileStream.WriteAsync(bytes);
            }

            var imageUrl = Path.Combine(imagesFolder, randomFileName);

            return imageUrl;
        }

        private static string GetFileExtensionFromBase64String(string base64String)
        {
            var regexForExtension = new Regex(@"data:image\/([a-zA-Z0-9-.+]+).*,.*/", RegexOptions.IgnoreCase);
            var matches = regexForExtension.Match(base64String);

            if (!matches.Success)
            {
                throw new ServiceException("Incorrect base64 string");
            }

            var fileExtension = matches.Groups[1].Value;

            return fileExtension;
        }

        private static string GetPureBase64String(string base64String)
        {
            var regexForPureBase64 = new Regex(@"base64,(.*)", RegexOptions.IgnoreCase);
            var matchesForPureBase64 = regexForPureBase64.Match(base64String);

            if (!matchesForPureBase64.Success)
            {
                throw new ServiceException("Incorrect base64 string");
            }

            var posValue = matchesForPureBase64.Groups[1].Index;

            var pureBase64String = base64String[posValue..];

            return pureBase64String;
        }
    }
}
