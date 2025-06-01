using System.Collections.Generic;
using _0_Framework.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Application
{
    public class CommentApplication : ICommentApplication
    {
        private readonly ICommentRepository _commentRepository;

        public CommentApplication(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public OperationResult Add(AddComment command)
        {
            var operation = new OperationResult();
            var comment = new Comment(command.Name , command.Email , command.Website , command.Message ,
                command.OwnerRecordId , command.Type , command.ParentId);

            _commentRepository.Create(comment);
            _commentRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult ChangeStatus(long id, CommentViewModel.CommentStatus status)
        {
            var operation = new OperationResult();
            var comment = _commentRepository.Get(id);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            switch (status)
            {
                case CommentViewModel.CommentStatus.Pending:
                    comment.Pending();
                    break;
                case CommentViewModel.CommentStatus.Confirmed:
                    comment.Confirm();
                    break;
                case CommentViewModel.CommentStatus.Canceled:
                    comment.Cancel();
                    break;
                case CommentViewModel.CommentStatus.Spam:
                    comment.Spam();
                    break;
                default:
                    return operation.Failed(ApplicationMessages.InValidStatus);
            }

            _commentRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            return _commentRepository.Search(searchModel);
        }

        public List<CommentViewModel> GetAllComment()
        {
            return _commentRepository.GetAllComment();
        }
    }
}
