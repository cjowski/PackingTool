namespace PackingTool.Core.Service.PackingList
{
    public interface IPackingListService
    {
        Task<Json.PackingList> GetList(int listID);
        Json.PackingListContent GetListContentFromJson(string jsonList);
        Task<string> GetJson(int listID);
        Task<Output.PackingListDescription[]> GetListDescriptionsForUser(int userID);
        Task<int> SaveListForUser(Json.PackingList list, int userID);
        Task<int> SaveJsonListForUser(string listName, string jsonList, int userID);
        Task UpdateListName(int listID, string newName, int userID);
        Task DeleteList(int listID, int userID);
    }
}
