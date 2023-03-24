using Newtonsoft.Json;
using CoreService = PackingTool.Core.Service.PackingList;

namespace PackingTool.Service.Service.PackingList
{
    public class PackingListService : CoreService.IPackingListService
    {
        private Core.Repository.PackingList.IPackingListRepository _repository { get; }

        public PackingListService(
            Core.Repository.PackingList.IPackingListRepository repository
        )
        {
            _repository = repository;
        }

        public async Task<CoreService.Json.PackingList> GetList(
            int listID
        )
        {
            if (!await _repository.ListExists(listID))
            {
                throw new InvalidDataException(
                    $"Packing list with provided ID: '{listID}' doesn't exist."
                );
            }

            var listDb = await _repository.GetList(listID);
            var list = CoreService.Json.PackingList.FromDatabase(listDb);

            return list;
        }

        public CoreService.Json.PackingListContent GetListContentFromJson(
            string jsonList
        )
        {
            return CoreService.Json.PackingListContent.FromJson(jsonList);
        }

        public async Task<string> GetJson(int listID)
        {
            if (!await _repository.ListExists(listID))
            {
                throw new InvalidDataException(
                    $"Packing list with provided ID: '{listID}' doesn't exist."
                );
            }

            var listDb = await _repository.GetList(listID);
            var listContent = CoreService.Json.PackingListContent.FromJson(listDb.Json);
            var jsonToRead = JsonConvert.SerializeObject(listContent, Formatting.Indented);

            return jsonToRead;
        }

        public async Task<CoreService.Output.PackingListDescription[]> GetListDescriptionsForUser(
            int userID
        )
        {
            var listNames = await _repository.GetListsForUser(userID);
            return listNames
                .Select(x =>
                    new CoreService.Output.PackingListDescription(
                        id: x.ID,
                        name: x.Name
                    )
                )
                .ToArray();
        }

        public async Task<int> SaveListForUser(
            CoreService.Json.PackingList list,
            int userID
        )
        {
            var jsonToSave = JsonConvert.SerializeObject(list.Content, Formatting.None);
            var listID = list.ID > 0
                ? list.ID
                : await _repository.TryGetListIDForUser(list.Name, userID);

            if (listID > 0)
            {
                await _repository.UpdateList(listID, jsonToSave, userID);
            }
            else
            {
                listID = await _repository.AddListForUser(list.Name, jsonToSave, userID);
            }

            return listID;
        }

        public async Task<int> SaveJsonListForUser(
            string listName,
            string jsonList, 
            int userID
        )
        {
            var listContent = CoreService.Json.PackingListContent.FromJson(jsonList);
            var jsonToSave = JsonConvert.SerializeObject(listContent, Formatting.None);
            var listID = await _repository.TryGetListIDForUser(listName, userID);

            if (listID > 0)
            {
                await _repository.UpdateList(listID, jsonToSave, userID);
            }
            else
            {
                listID = await _repository.AddListForUser(listName, jsonToSave, userID);
            }

            return listID;
        }

        public async Task UpdateListName(
            int listID,
            string newName,
            int userID
        )
        {
            await _repository.UpdateListName(listID, newName, userID);
        }

        public async Task DeleteList(int listID, int userID)
        {
            await _repository.DeleteList(listID, userID);
        }
    }
}
