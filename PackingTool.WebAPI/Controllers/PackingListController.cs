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

        [HttpGet("List")]
        public async Task<CoreService.Json.PackingList> GetList(
            int listID
        )
        {
            return await _packingService.GetList(listID);
        }

        [HttpGet("Json")]
        public async Task<string> GetJson(
            int listID
        )
        {
            return await _packingService.GetJson(listID);
        }

        [HttpGet("ListDescriptionsForUser")]
        public async Task<CoreService.Output.PackingListDescription[]> GetListDescriptionsForUser(
            int userID
        )
        {
            return await _packingService.GetListDescriptionsForUser(userID);
        }

        [HttpPost("SaveListFromJson")]
        public async Task SaveListFromJson(
            [FromBody] string jsonList,
            int userID
        )
        {
            await _packingService.AddUpdateListForUser(jsonList, userID);
        }

        [HttpPost("SaveListFromJsonFile")]
        public async Task SaveListFromJsonFile(
            IFormFile file,
            int userID
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
            await _packingService.AddUpdateListForUser(jsonList, userID);
        }

        [HttpPost("UpdateListName")]
        public async Task UpdateListName(
            int listID,
            string newName,
            int userID
        )
        {
            await _packingService.UpdateListName(listID, newName, userID);
        }

        [HttpDelete("DeleteList")]
        public async Task DeleteList(
            int listID,
            int userID
        )
        {
            await _packingService.DeleteList(listID, userID);
        }
    }
}
