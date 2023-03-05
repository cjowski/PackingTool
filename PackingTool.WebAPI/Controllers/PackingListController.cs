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

        [HttpGet("list")]
        public async Task<CoreService.Json.PackingList> GetList(
            int listID
        )
        {
            return await _packingService.GetList(listID);
        }

        [HttpGet("json")]
        public async Task<string> GetJson(
            int listID
        )
        {
            return await _packingService.GetJson(listID);
        }

        [HttpGet("listDescriptionsForUser")]
        public async Task<CoreService.Output.PackingListDescription[]> GetListDescriptionsForUser(
            int userID
        )
        {
            return await _packingService.GetListDescriptionsForUser(userID);
        }

        [HttpPost("save")]
        public async Task<int> Save(
            [FromBody] CoreService.Json.PackingList list,
            int userID
        )
        {
            return await _packingService.SaveListForUser(list, userID);
        }

        [HttpPost("saveFromJsonFile")]
        public async Task<int> SaveFromJsonFile(
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

            var listName = Path.GetFileNameWithoutExtension(file.FileName);
            var jsonList = Encoding.UTF8.GetString(stream.ToArray());
            return await _packingService.SaveJsonListForUser(listName, jsonList, userID);
        }

        [HttpPost("updateName")]
        public async Task UpdateName(
            int listID,
            string newName,
            int userID
        )
        {
            await _packingService.UpdateListName(listID, newName, userID);
        }

        [HttpDelete("delete")]
        public async Task Delete(
            int listID,
            int userID
        )
        {
            await _packingService.DeleteList(listID, userID);
        }
    }
}
