﻿using Microsoft.EntityFrameworkCore;
using Output = PackingTool.Core.Repository.PackingList.Output;

namespace PackingTool.MsSqlDatabase.Repository
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

        public async Task<Output.PackingListDb> GetList(
            int listID
        )
        {
            return await _context.PackingList
                .Where(x => x.PackingListId == listID)
                .Select(x =>
                    new Output.PackingListDb(
                        x.PackingListId,
                        x.Name,
                        x.Json
                    )
                )
                .SingleAsync();
        }

        public async Task<Output.PackingListDb[]> GetListsForUser(
            int userID
        )
        {
            return await _context.UserPackingList
                .Where(x =>
                    x.UserId == userID
                    && !x.PackingList.Deleted
                )
                .Select(x =>
                    new Output.PackingListDb(
                        x.PackingListId,
                        x.PackingList.Name,
                        x.PackingList.Json
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
                    && !x.PackingList.Deleted
                    && x.UserId == userID
                )
                .Select(x => x.PackingListId)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddListForUser(
            string listName,
            string jsonList,
            int userID,
            int requestedUserID
        )
        {
            var packingList = new DbModels.PackingList()
            {
                Name = listName,
                Json = jsonList,
                Deleted = false,
                CreatedDate = DateTime.Now,
                CreatedUserId = requestedUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = requestedUserID
            };

            _context.PackingList.Add(packingList);
            await _context.SaveChangesAsync();

            var userPackingList = new DbModels.UserPackingList()
            {
                UserId = userID,
                PackingListId = packingList.PackingListId,
                CreatedDate = DateTime.Now,
                CreatedUserId = requestedUserID
            };

            _context.UserPackingList.Add(userPackingList);
            await _context.SaveChangesAsync();

            return packingList.PackingListId;
        }

        public async Task UpdateList(
            int listID,
            string jsonList,
            int requestedUserID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Json = jsonList;
            packingList.ModifiedUserId = requestedUserID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListName(
            int listID,
            string newName,
            int requestedUserID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Name = newName;
            packingList.ModifiedUserId = requestedUserID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteList(
            int listID,
            int requestedUserID
        )
        {
            var packingList = await _context.PackingList
                .SingleAsync(x => x.PackingListId == listID);
            packingList.Deleted = true;
            packingList.ModifiedUserId = requestedUserID;
            packingList.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
