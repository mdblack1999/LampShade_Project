using System.Collections.Generic;
using _0_Framework.Application;

namespace CommentManagement.Application.Contracts.Comment
{
    public interface ICommentApplication
    {
        OperationResult Add(AddComment command);
        OperationResult ChangeStatus(long id , CommentViewModel.CommentStatus status);
        List<CommentViewModel> Search(CommentSearchModel searchModel);
    }
}
