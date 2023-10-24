using System.ComponentModel.DataAnnotations.Schema;
using ApiTemplate.Domain.Models;
using ApiTemplate.Domain.Users.ValueObjects;

namespace ApiTemplate.Domain.Users;

public class User : AggregateRoot<UserId>
{
    private User(UserId id, string firstName, string lastName, string email, string password) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
    
    //Used for Json serialization
    private User() : base()
    {
    }
    
    private readonly List<RefreshToken> _refreshTokens = new();
    
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public RefreshToken? ActiveRefreshToken => _refreshTokens.Find(rt => !rt.Expired);
    [NotMapped]
    public override UserId CreatedBy { get; set; } = null!;

    [NotMapped]
    public override User CreatedByUser { get; set; } = null!;

    [NotMapped]
    public override UserId UpdatedBy { get; set; } = null!;

    [NotMapped]
    public override User UpdatedByUser { get; set; } = null!;

    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new User(UserId.CreateUnique(), firstName, lastName, email, password);
    }
}