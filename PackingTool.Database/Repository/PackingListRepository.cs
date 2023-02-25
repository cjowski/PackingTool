using Microsoft.EntityFrameworkCore;

namespace PackingTool.Database.Repository
{
    public class PackingListRepository : Core.Repository.PackingList.IPackingListRepository
    {
        private DbModels.PackingContext _context { get; }

        public PackingListRepository(
            DbModels.PackingContext context
        )
        {
            _context = context;
        }

        public async Task<bool> ListExists(
            int listID    
        )
        {
            return await _context.PackingList
                .AnyAsync(x => x.PackingListId == listID);
        }

        public async Task<string> GetJsonList(
            int listID
        )
        {
            return await _context.PackingList
                .Where(x => x.PackingListId == listID)
                .Select(x => x.Json)
                .SingleAsync();
        }

        public async Task<Tuple<int, string>[]> GetListIDsWithNamesForUser(
            int userID
        )
        {
            return await _context.UserPackingList
                .Where(x =>
                    x.UserId == userID
                    && !x.PackingList.Deleted
                )
                .Select(x =>
                    new Tuple<int, string>(
                        x.PackingListId,
                        x.PackingList.Name
                    )
                )
                .ToArrayAsync();
        }

        public async Task<int> TryGetListIDForUser(
            string listName,
            int userID
        )
        {
            return await _context.UserPackingList
                .Where(x =>
                    x.PackingList.Name == listName
                    && x.UserId == userID
                )
                .Select(x => x.PackingListId)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddListForUser(
            string listName,
            string jsonList,
            int userID
        )
        {
            var packingList = new DbModels.PackingList()
            {
                Name = listName,
                Json = jsonList,
                Deleted = false,
                CreatedDate = DateTime.Now,
                CreatedUserId = userID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = userID
            };

            _context.PackingList.Add(packingList);
            var packingListID = await _context.SaveChangesAsync();

            var userPackingList = new DbModels.UserPackingList()
            {
                UserId = userID,
                PackingListId = packingListID,
                CreatedDate = DateTime.Now,
                CreatedUserId = userID
            };

            _context.UserPackingList.Add(userPackingList);
            await _context.SaveChangesAsync();

            return packingListID;
        }

        public async Task UpdateList(
            int listID,
            string jsonList,
            int userID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Json = jsonList;
            packingList.ModifiedUserId = userID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListName(
            int listID,
            string newName,
            int userID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Name = newName;
            packingList.ModifiedUserId = userID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteList(
            int listID,
            int userID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Deleted = true;
            packingList.ModifiedUserId = userID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
