namespace finance_management_backend.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}