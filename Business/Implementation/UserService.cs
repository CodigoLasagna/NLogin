using System.Numerics;
using Business.Contracts;
using Nlog.context.Contracts;
using Nlog.model;

namespace Business.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public string Register(User entity)
    {
        if (entity == null) return null;
        BigInteger publicKey, privateKey, n;
        Security.GenerateKeys(out publicKey, out privateKey, out n);
        //Console.WriteLine("Mensaje original: " + entity.Password);
        //Console.WriteLine("Private key: " + privateKey.ToString());
        byte[] encrypted_pass = Security.EncryptText(entity.Password, publicKey, n);
        entity.Password = BitConverter.ToString(encrypted_pass).Replace("-", "");
        entity.n_numb = n.ToString();
        //Console.WriteLine("Encriptado: " + entity.Password);
        //string decryptedText = Security.DecryptText(Security.returnBytes(entity.Password), BigInteger.Parse(priv_key), BigInteger.Parse(entity.n_numb));
        //Console.WriteLine("Mensaje descifrado: " + decryptedText);
        //Console.WriteLine(entity.n_numb);
        //Console.WriteLine(n);
        if (_userRepository.Register(entity))
            return privateKey.ToString();
        return null;
    }

    public string Login(User entity, string key)
    {
        if (entity == null) return "-2";
        User cur_user = _userRepository.Login(entity);
        if (cur_user == null) return "0";
        BigInteger n = BigInteger.Parse(cur_user.n_numb);
        BigInteger p_key = BigInteger.Parse(key);
        string decryptedPass = Security.DecryptText(Security.returnBytes(cur_user.Password), p_key, n);
        if (decryptedPass.Equals(entity.Password))
        {
            return cur_user.Username;
        }

        return "-1";
    }

    public User Read(int Id)
    {
        if (Id <= 0) return null;
        return _userRepository.Read(Id);
    }
}