namespace PackingTool.Core.Repository.PackingList
{
    public interface IPackingListRepository
    {
        Task<bool> ListExists(int listID);
        Task<string> GetJsonList(int listID);
        Task<Tuple<int, string>[]> GetListIDsWithNamesForUser(int userID);
        Task<int> TryGetListIDForUser(string listName, int userID);
        Task<int> AddListForUser(string listName, string jsonList, int userID);
        Task UpdateList(int listID, string jsonList, int userID);
        Task UpdateListName(int listID, string newName, int userID);
        Task DeleteList(int listID, int userID);
    }
}
