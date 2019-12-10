using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().SingleOrDefaultAsync(f => f.Id == friendId);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            using (var context = _contextCreator())
            {
                var friendFromDb = await context.Friends.SingleAsync(f => f.Id == friend.Id);

                friendFromDb.FirstName = friend.FirstName;
                friendFromDb.LastName = friend.LastName;
                friendFromDb.Email = friend.Email;

                await context.SaveChangesAsync();
            }
        }
    }
}
