using Microsoft.AspNetCore.Mvc;
using System.Text;
using PackingListService = PackingTool.Core.Service.PackingList;

namespace PackingTool.WebAPI.Controllers
{
    public class PackingListController : BaseApiController
    {
        private PackingListService.IPackingListService _packingService { get; }

        public PackingListController(
            Core.Service.User.ITokenService tokenService,
            PackingListService.IPackingListService packingService
        ) : base(tokenService)
        {
            _packingService = packingService;
        }

        [HttpGet("list")]
        public async Task<PackingListService.Json.PackingList> GetList(
            int listID
        )
        {
            return await _packingService.GetList(listID);
        }

        [HttpPost("listContentFromJson")]
        public PackingListService.Json.PackingListContent GetListContentFromJson(
            [FromBody] string json
        )
        {
            return _packingService.GetListContentFromJson(json);
        }

        [HttpGet("json")]
        public async Task<string> GetJson(
            int listID
        )
        {
            return await _packingService.GetJson(listID);
        }

        [HttpGet("listDescriptionsForUser")]
        public async Task<PackingListService.Output.PackingListDescription[]> GetListDescriptionsForUser(
            int userID
        )
        {
            return await _packingService.GetListDescriptionsForUser(userID);
        }

        [HttpPost("save")]
        public async Task<int> Save(
            [FromBody] PackingListService.Json.PackingList list,
            int userID
        )
        {
            return await _packingService.SaveListForUser(list, userID, GetRequestedUserID());
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
            return await _packingService.SaveJsonListForUser(listName, jsonList, userID, GetRequestedUserID());
        }

        [HttpPost("updateName")]
        public async Task UpdateName(
            int listID,
            string newName
        )
        {
            await _packingService.UpdateListName(listID, newName, GetRequestedUserID());
        }

        [HttpDelete("delete")]
        public async Task Delete(
            int listID
        )
        {
            await _packingService.DeleteList(listID, GetRequestedUserID());
        }
    }
}
