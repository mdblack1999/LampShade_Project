using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _0_Framework.Application;
using _01_LampShadeQuery.Contracts.Article;
using _01_LampShadeQuery.Contracts.Comment;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampShadeQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _context;
        private readonly CommentContext _commentContext;

        public ArticleQuery(BlogContext context , CommentContext commentContext)
        {
            _context = context;
            _commentContext = commentContext;
        }

        public List<ArticleQueryModel> LatestArticles()
        {
            var today = DateTime.Now.AddDays(1);
            return _context.Articles.Include(x => x.Category)
                .Where(x => EF.Functions.DateDiffDay(x.PublishDate , today) >= 0)
                .Select(x => new ArticleQueryModel
                {
                    Title = x.Title ,
                    Slug = x.Slug ,
                    Picture = x.Picture ,
                    PictureAlt = x.PictureAlt ,
                    PictureTitle = x.PictureTitle ,
                    PublishDate = x.PublishDate.ToFarsi() ,
                    VisitCount = x.VisitCount
                }).AsNoTracking().ToList();
        }

        public ArticleQueryModel GetArticleDetails(string slug)
        {
            var today = DateTime.Now.AddDays(1);
            var article = _context.Articles.Include(x => x.Category)
                .Where(x => EF.Functions.DateDiffDay(x.PublishDate , today) >= 0)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id ,
                    Title = x.Title ,
                    CategoryName = x.Category.Name ,
                    CategorySlug = x.Category.Slug ,
                    Slug = x.Slug ,
                    CanonicalAddress = x.CanonicalAddress ,
                    Description = x.Description ,
                    Keywords = x.Keywords ,
                    MetaDescription = x.MetaDescription ,
                    ShortDescription = x.ShortDescription ,
                    Picture = x.Picture ,
                    PictureAlt = x.PictureAlt ,
                    PictureTitle = x.PictureTitle ,
                    PublishDate = x.PublishDate.ToFarsi() ,
                    VisitCount = x.VisitCount
                }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

            if (article == null)
                return new ArticleQueryModel();

            if (!string.IsNullOrWhiteSpace(article.Keywords))
                article.KeywordList = article.Keywords
                    .Split(new[] { ',' , '-' , '.' } , StringSplitOptions.RemoveEmptyEntries).ToList();

            var comments = _commentContext.Comments
                .AsNoTracking()
                .Where(x => x.Type == CommentType.ArticleType && x.OwnerRecordId == article.Id)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id ,
                    Name = x.Name ,
                    Message = x.Message ,
                    CreationDate = " " + x.CreationDate.ToFarsiFull() ,
                    ParentId = x.ParentId ,
                    Status = (CommentStatusDto)x.Status
                })
                .Where(x => x.Status == CommentStatusDto.Confirmed)
                .OrderByDescending(x => x.Id)
                .ToList();
            foreach (var comment in comments.Where(comment => comment.ParentId > 0))
            {
                comment.ParentName = comments.FirstOrDefault(x => x.Id == comment.ParentId)?.Name;
            }


            var lookup = comments.ToLookup(c => c.ParentId);
            var ordered = new List<CommentQueryModel>();
            void AddComments(long parentId)
            {
                foreach (var child in lookup[parentId].OrderByDescending(c => c.Id))
                {
                    ordered.Add(child);
                    AddComments(child.Id);
                }
            }
            AddComments(0);

            article.Comments = ordered;

            return article;
        }
    }
}
