namespace Code_API_Mobile.ModelDTO
{
    public class ExpenseRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }

}
