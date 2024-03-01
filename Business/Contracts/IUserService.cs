using Nlog.model;

namespace Business.Contracts;

public interface IUserService
{
    string Register(User entity);
    string Login(User entity, string key);
    User Read(int Id);
}