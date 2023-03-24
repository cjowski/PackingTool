namespace PackingTool.Core.Repository.PackingList
{
    public interface IPackingListRepository
    {
        Task<bool> ListExists(int listID);
        Task<Output.PackingListDb> GetList(int listID);
        Task<Output.PackingListDb[]> GetListsForUser(int userID);
        Task<int> TryGetListIDForUser(string listName, int userID);
        Task<int> AddListForUser(string listName, string jsonList, int userID, int requestedUserID);
        Task UpdateList(int listID, string jsonList, int requestedUserID);
        Task UpdateListName(int listID, string newName, int requestedUserID);
        Task DeleteList(int listID, int requestedUserID);
    }
}
