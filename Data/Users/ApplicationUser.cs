using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace BlazorServerTest.Data.Users;

[CollectionName("user_info")]
public class ApplicationUser : MongoIdentityUser<Guid> {
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ApplicationUser() : base() { }
    public ApplicationUser(string firstName, string lastName, string email) : base("", email) {
        FirstName = firstName;
        LastName = lastName;
    }

    public static readonly ApplicationUser NoUser = new ApplicationUser("Unknown", "Unknown", "unknown@unknown.com");
}

public class ApplicationRole : MongoIdentityRole<Guid> {
    public ApplicationRole() : base() {}

    public ApplicationRole(string roleName) : base(roleName) {}
}