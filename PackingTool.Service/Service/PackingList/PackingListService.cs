using Newtonsoft.Json;
using CoreService = PackingTool.Core.Service.PackingList;

namespace PackingTool.Service.Service.PackingList
{
    public class PackingListService : CoreService.IPackingListService
    {
        public async Task AddUpdateList(
            string jsonList
        )
        {
            var list = CoreService.Json.PackingList.FromJson(jsonList, false);
            var jsonToSave = JsonConvert.SerializeObject(list, Formatting.Indented);
        }
    }
}
