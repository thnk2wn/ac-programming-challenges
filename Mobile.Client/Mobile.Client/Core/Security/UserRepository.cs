using System.Collections.Generic;
using System.Linq;

namespace AvantCredit.Uploader.Core.Security
{
    class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public UserRepository()
        {
            // just a couple fake users for now
            _users.Add(new User
            {
                EmailAddress = "demo@domain.com",
                UserId = 123,
                FirstName =  "Demo",
                LastName = "User",
                Salt = "Nx8cn9Bjl4qwKtNrVW3MFA==",
                Password = "7uPxLBd/6FrmCyfiEqfNQpY+UBjLPRdzIjsXYDFAhQQ="
            });

            _users.Add(new User
            {
                EmailAddress = "guest@domain.com",
                FirstName = "Guest",
                LastName = "User",
                UserId = 345,
                Salt = "E3H2UG+7H0kRnWNLvOzkxg==",
                Password = "ZtD61iUAN5uDTV0VlJqWEfvka77I75wcvvWO9kpH1Xk="
            });
        }

        public User GetByEmailAddress(string emailAddress)
        {
            var user = _users.SingleOrDefault(u => u.EmailAddress == emailAddress);
            return user;
        }
    }
}