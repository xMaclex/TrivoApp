using UserApi.DTOs;
using UserApi.Models;

namespace UserApi.Services;

public class UserServices
{
    private static List<User> users = new();

    public List<User> GetAll()
    {
        return users;
    }
    public User Add(CreateUserDto dto)
    {
        var user = new User
        {
            Id = users.Count + 1,
            Name = dto.Name,
            Email = dto.Email
        };

        users.Add(user);
        return user;
    }

    public User Update(int id, CreateUserDto dto)
    {
        var user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
            return null;

        user.Name = dto.Name;
        user.Email = dto.Email;

        return user;
    }

    public bool Delete(int id)
    {
        var user = users.FirstOrDefault(x => x.Id == id);

        if(user == null)
        return false;

        users.Remove(user);
        return true;
    }
}