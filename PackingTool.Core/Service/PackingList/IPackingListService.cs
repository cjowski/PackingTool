namespace PackingTool.Core.Service.PackingList
{
    public interface IPackingListService
    {
        Task<Json.PackingList> GetList(int listID);
        Task<string> GetJson(int listID);
        Task<Output.PackingListDescription[]> GetListDescriptionsForUser(int userID);
        Task AddUpdateListForUser(string jsonList, int userID);
        Task UpdateListName(int listID, string newName, int userID);
        Task DeleteList(int listID, int userID);
    }
}
