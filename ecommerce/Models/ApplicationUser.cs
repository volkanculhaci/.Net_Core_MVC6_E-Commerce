using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Additional properties
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string UserId { get; set; } // Foreign key for the user

    // Other properties from IdentityUser, e.g., UserName, Email, etc.
}
