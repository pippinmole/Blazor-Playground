using AspNetCore.Identity.Mongo.Model;

namespace BlazorServerTest.Data.Users;

public sealed class ApplicationUser : MongoUser<Guid> {
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public ApplicationUser() : base() { }
    public ApplicationUser(string? firstName, string? lastName, string email) : base(firstName + lastName) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static readonly ApplicationUser NoUser = new ApplicationUser("Unknown", "Unknown", "unknown@unknown.com");
}

public class ApplicationRole : MongoRole<Guid> {
    public ApplicationRole() : base() {}

    public ApplicationRole(string roleName) : base(roleName) {}
}