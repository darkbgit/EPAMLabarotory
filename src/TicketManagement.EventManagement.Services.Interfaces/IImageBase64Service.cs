namespace TicketManagement.Core.EventManagement.Services.Interfaces
{
    public interface IImageBase64Service
    {
        /// <summary>
        /// Save image from base64 string to wwwroot folder.
        /// </summary>
        /// <returns>URL to saved image.</returns>
        public Task<string> SaveImgFromBase64(string base64String);
    }
}
