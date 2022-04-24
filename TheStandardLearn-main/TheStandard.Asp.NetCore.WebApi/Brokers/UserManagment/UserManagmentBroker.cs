using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.UserManagment
{

    public class UserManagmentBroker : IUserManagmentBroker
    {
        //private readonly UserManager<User> userManagment;

        //public UserManagmentBroker(UserManager<User> userManagment) =>
        //    this.userManagment = userManagment;

        public Guid GetCurrentlyLoggedIn() =>
            Guid.NewGuid();
        //public ValueTask<User> InsertUserAsync(User user, string password)
        //{
        //    throw new NotImplementedException();
        //}
        //public IQueryable<User> SelectAllUsers()
        //{
        //    throw new NotImplementedException();
        //}
        //public ValueTask<User> SelectUserByIdAsync(Guid userId)
        //{
        //    throw new NotImplementedException();
        //}
        //public ValueTask<User> UpdateUserAsync(User user)
        //{
        //    throw new NotImplementedException();
        //}
        //public ValueTask<User> DeleteUserAsync(User user)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
