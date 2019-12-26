namespace finance_management_backend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IconId { get; set; }
        public Icon Icon { get; set; }
    }
}