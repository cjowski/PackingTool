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

        public async Task<CoreService.Json.PackingList> GetList(int listID)
        {
            if (!await _repository.ListExists(listID))
            {
                throw new InvalidDataException(
                    $"Packing list with provided ID: '{listID}' doesn't exist."
                );
            }

            var jsonList = await _repository.GetJsonList(listID);
            var list = CoreService.Json.PackingList.FromDatabase(listID, jsonList);

            return list;
        }

        public async Task<string> GetJson(int listID)
        {
            if (!await _repository.ListExists(listID))
            {
                throw new InvalidDataException(
                    $"Packing list with provided ID: '{listID}' doesn't exist."
                );
            }

            var jsonList = await _repository.GetJsonList(listID);
            var list = CoreService.Json.PackingList.FromDatabase(listID, jsonList);
            var jsonToRead = JsonConvert.SerializeObject(list, Formatting.Indented);

            return jsonToRead;
        }

        public async Task<CoreService.Output.PackingListDescription[]> GetListDescriptionsForUser(int userID)
        {
            var listNames = await _repository.GetListIDsWithNamesForUser(userID);
            return listNames
                .Select(x =>
                    new CoreService.Output.PackingListDescription(
                        id: x.Item1,
                        name: x.Item2
                    )
                )
                .ToArray();
        }

        public async Task AddUpdateListForUser(
            string jsonList,
            int userID
        )
        {
            var list = CoreService.Json.PackingList.FromJson(jsonList);
            var jsonToSave = JsonConvert.SerializeObject(list, Formatting.None);
            var listID = list.ID;

            var listExists = list.ID > 0 && await _repository.ListExists(list.ID);
            if (!listExists)
            {
                listID = await _repository.TryGetListIDForUser(list.Name, userID);
                listExists = listID > 0;
            }

            if (listExists)
            {
                await _repository.UpdateList(listID, jsonToSave, userID);
            }
            else
            {
                await _repository.AddListForUser(list.Name, jsonToSave, userID);
            }
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
