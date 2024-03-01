using System.Data.Common;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Nlog.context.Contracts;
using Nlog.helpers;
using Nlog.model;

namespace Nlog.context.Implementation;

public class UserRepository : IUserRepository
{
    public bool Register(User entity)
    {
        var connectionOptions = new DbContextOptionsBuilder<NlogDbContext>()
            .UseMySql(constants.ConnectionString,
                ServerVersion.AutoDetect(constants.ConnectionString))
            .Options;
            
        using (var ctx = new NlogDbContext(options: connectionOptions))
        {
            ctx.Users.Add(entity);
            ctx.SaveChanges();
            return true;
        }
    }

    public User Login(User entity)
    {
        var connectionOptions = new DbContextOptionsBuilder<NlogDbContext>()
            .UseMySql(constants.ConnectionString,
                ServerVersion.AutoDetect(constants.ConnectionString))
            .Options;

        User cur_user = null;
        using (var ctx = new NlogDbContext(options: connectionOptions))
        {
            cur_user = ctx.Users.Where(x => x.Email == entity.Email).FirstOrDefault();
            return cur_user;
        }
    }

    public User Read(int Id)
    {
        var connectionOptions = new DbContextOptionsBuilder<NlogDbContext>()
            .UseMySql(constants.ConnectionString,
                ServerVersion.AutoDetect(constants.ConnectionString))
            .Options;

        User curUser = null;
        using (var ctx = new NlogDbContext(options: connectionOptions))
        {
            curUser = ctx.Users.Where(x => x.Id == Id).FirstOrDefault();
            return curUser;
        }
    }
}