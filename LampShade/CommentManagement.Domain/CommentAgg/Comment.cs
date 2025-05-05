using _0_Framework.Domain;

namespace CommentManagement.Domain.CommentAgg
{
    public class Comment : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }
        public string Website { get; private set; }
        public long OwnerRecordId { get; private set; }
        public int Type { get; private set; }
        public long ParentId { get; private set; }
        public Comment Parent { get; private set; }
        public CommentStatus Status { get; private set; }
        

        public Comment(string name , string email , string website , string message , long ownerRecordId , int type ,
            long parentId)
        {
            Name = name;
            Email = email;
            Website = website;
            Message = message;
            OwnerRecordId = ownerRecordId;
            Type = type;
            ParentId = parentId;
            Status = CommentStatus.Pending;
        }
        
        public enum CommentStatus
        {
            Pending = 0,
            Confirmed = 1,
            Canceled = 2,
            Spam = 3
        }

        public void Confirm() => Status = CommentStatus.Confirmed;
        public void Cancel() => Status = CommentStatus.Canceled;
        public void Spam() => Status = CommentStatus.Spam;
        public void Pending() => Status = CommentStatus.Pending;
    }
}
