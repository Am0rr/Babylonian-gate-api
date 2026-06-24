using BG.Domain.Enums;

namespace BG.Domain.Entities.Identity;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    protected User() { }

    public User(string firstName, string lastName, string email, string passwordHash, UserRole role = UserRole.Client)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void ChangeFirstName(string newFirstName) => FirstName = newFirstName;
    public void ChangeLastName(string newLastName) => LastName = newLastName;
    public void ChangeEmail(string newEmail) => Email = newEmail;
    public void ChangeRole(UserRole newRole) => Role = newRole;
}