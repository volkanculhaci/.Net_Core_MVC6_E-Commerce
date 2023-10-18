namespace ecommerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; } // Reference to the user
        public List<CartItem> CartItems { get; set; }
        // Add other properties as needed
    }

}
