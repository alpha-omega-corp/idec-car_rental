using System.Runtime.Serialization;

namespace CarRental.Domain;

[DataContract]
public class Login(string email, string password)
{
    [DataMember] public string Email { get; set; } = email;
    [DataMember] public string Password { get; set; } = password;
}