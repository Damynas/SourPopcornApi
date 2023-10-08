using Domain.Abstractions.Abstracts;

namespace Domain.Users.Entities
{
    public class User(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
    {
        public required string Username { get; set; }

        public required string PasswordHash { get; set; }

        public required string DisplayName { get; set; }

        public required IList<string> Roles { get; set; }
    }
}
