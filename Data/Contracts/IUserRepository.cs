using Nlog.model;

namespace Nlog.context.Contracts;

public interface IUserRepository
{
    bool Register(User entity);
    User Login(User entity);
    User Read(int Id);
}