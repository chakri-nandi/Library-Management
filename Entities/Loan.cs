namespace LibraryManagement.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; } = null;
        public DateTime LendAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }
    }
}
