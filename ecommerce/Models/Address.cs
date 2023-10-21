using Microsoft.AspNetCore.Identity;

namespace ecommerce.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string UserId { get; set; } // Foreign key for the user

        // Navigation property to link Address to the User entity
        public IdentityUser User { get; set; }
    }
}
