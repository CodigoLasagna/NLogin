using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Nlog.model;

public class User
{
    [Key]
    public int? Id { get; set; }

    public string? Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? n_numb { get; set; }
    public string? p_key { get; set; }
}