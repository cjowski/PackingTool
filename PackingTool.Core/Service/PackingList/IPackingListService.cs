namespace PackingTool.Core.Service.PackingList
{
    public interface IPackingListService
    {
        Task AddUpdateList(string jsonList);
    }
}
