using Microsoft.AspNetCore.Mvc;
using System.Text;
using CoreService = PackingTool.Core.Service.PackingList;

namespace PackingTool.WebAPI.Controllers
{
    public class PackingListController : BaseApiController
    {

        private CoreService.IPackingListService _packingService { get; }

        public PackingListController(
            CoreService.IPackingListService packingService
        )
        {
            _packingService = packingService;
        }

        [HttpPost("AddUpdateListFromJsonFile")]
        public async Task Add(
            IFormFile file
        )
        {
            if (!file.ContentType.EndsWith("/json"))
            {
                throw new InvalidDataException(
                    $"Invalid file type: {file.ContentType}. Only JSON files are supported."
                );
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var jsonList = Encoding.UTF8.GetString(stream.ToArray());
            await _packingService.AddUpdateList(jsonList);
        }
    }
}
