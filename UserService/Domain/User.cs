using System.Runtime.Serialization;

namespace CarRental.Domain;

[DataContract]
public class User(long? id, string name, string email, string password)
{
    [DataMember] public long? Id { get; set; } = id;
    [DataMember] public string Name { get; set; } = name;
    [DataMember] public string Email { get; set; } = email;
    [DataMember] public string Password { get; set; } = password;
}