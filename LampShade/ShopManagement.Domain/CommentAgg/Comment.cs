using System;
using _0_Framework.Domain;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Domain.CommentAgg
{
    public class Comment : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }
        public CommentStatus Status { get; private set; }
        public long ProductId { get; private set; }
        public Product Product { get; private set; }

        public Comment(string name , string email , string message , long productId)
        {
            Name = name; 
            Email = email;
            Message = message;
            ProductId = productId;

            Status = CommentStatus.Pending;
            CreationDate = DateTime.Now;
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
    }
}
