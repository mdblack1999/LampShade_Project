using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;

namespace CommentManagement.Infrastructure.EFCore.Repository
{
    public class CommentRepository : RepositoryBase<long , Comment>, ICommentRepository
    {
        private readonly CommentContext _context;
        public CommentRepository(CommentContext context) : base(context)
        {
            _context = context;
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            searchModel ??= new CommentSearchModel();
            var query = _context.Comments
                .Select(x => new CommentViewModel
                {
                    Id = x.Id ,
                    Name = x.Name ,
                    Email = x.Email ,
                    Website = x.Website ,
                    Message = x.Message ,
                    OwnerRecordId = x.OwnerRecordId ,
                    Type = x.Type ,
                    CommentDate = x.CreationDate.ToFarsiFull() ,
                    Status = (CommentViewModel.CommentStatus)x.Status ,

                });
            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
                query = query.Where(x => x.Email.Contains(searchModel.Email));

            if (searchModel.Status.HasValue)
                query = query.Where(x => x.Status == searchModel.Status.Value);

            query = searchModel.Type switch
            {
                "product" => query.Where(x => x.Type == 1),
                "article" => query.Where(x => x.Type == 2),
                _ => query
            };

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public List<CommentViewModel> GetAllComment()
        {
            var query = _context.Comments
                .Select(x => new CommentViewModel
                {
                    Id = x.Id ,
                    Name = x.Name ,
                    Email = x.Email ,
                    Website = x.Website ,
                    Message = x.Message ,
                    OwnerRecordId = x.OwnerRecordId ,
                    Type = x.Type ,
                    CommentDate = x.CreationDate.ToFarsiFull() ,
                    Status = (CommentViewModel.CommentStatus)x.Status ,

                }).OrderByDescending(x => x.Id).ToList();
            foreach (var item in query)
            {
                if (item.Type == 1)
                    item.OwnerName = "Product";
                else if(item.Type ==2)
                    item.OwnerName = "article";
            }
            return query;
        }
    }
}
