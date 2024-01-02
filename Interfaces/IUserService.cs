using TaskProject.Models;
using System.Collections.Generic;

namespace TaskProject.Interfaces
{
    public interface IUserService
    {

        List<User> GetAll();
        User Get(long userId);
        void Add( User User);
        void Delete(long userId, int id);
        void Update( User user);
        int Count();
    
    }
}