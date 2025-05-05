namespace CommentManagement.Application.Contracts.Comment
{
    public class CommentSearchModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public CommentViewModel.CommentStatus? Status { get; set; }
    }
}