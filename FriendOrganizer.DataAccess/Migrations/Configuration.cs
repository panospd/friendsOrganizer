using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName, 
                new Friend
                {
                    FirstName = "Thomas",
                    LastName = "Huber"
                },
                new Friend
                {
                    FirstName = "Urs",
                    LastName = "Meier"
                },
                new Friend
                {
                    FirstName = "Erkan",
                    LastName = "Egin"
                },
                new Friend
                {
                    FirstName = "Sara",
                    LastName = "Huber"
                });
        }
    }
}
