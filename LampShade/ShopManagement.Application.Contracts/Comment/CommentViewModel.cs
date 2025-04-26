namespace ShopManagement.Application.Contracts.Comment
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string CommentDate { get; set; }
        public CommentStatus Status { get; set; }


        public enum CommentStatus
        {
            Pending = 0,
            Confirmed = 1,
            Canceled = 2,
            Spam = 3
        }
    }
}