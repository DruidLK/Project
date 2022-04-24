using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.UserManagment
{
    public interface IUserManagmentBroker
    {
        Guid GetCurrentlyLoggedIn();

        //ValueTask<User> InsertUserAsync(User user, string password);
        //IQueryable<User> SelectAllUsers();
        //ValueTask<User> SelectUserByIdAsync(Guid userId);
        //ValueTask<User> UpdateUserAsync(User user);
        //ValueTask<User> DeleteUserAsync(User user);
    }
}
