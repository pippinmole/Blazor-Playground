using System.Globalization;

namespace BlazorServerTest.Data.Users; 

public class SafeApplicationUser {
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool EmailConfirmed { get; set; }

    public string FullName() => $"{FirstName} {LastName}";
    public string FirstNameFormat() =>  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FirstName.ToLower());
}