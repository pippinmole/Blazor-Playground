using BlazorServerTest.Data.Users;

namespace BlazorServerTest.Data; 

public class UserContext {
    public ApplicationUser User { get; set; }
    public SafeApplicationUser SafeUser { get; set; }
    public string Test { get; set; }
}