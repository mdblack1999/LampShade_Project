using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleRepository : RepositoryBase<long , Article>, IArticleRepository
    {
        private readonly BlogContext _context;

        public ArticleRepository(BlogContext context) : base(context)
        {
            _context = context;
        }

        public EditArticle GetDetails(long id)
        {
            return _context.Articles.Select(x => new EditArticle
            {
                Id = x.Id ,
                CanonicalAddress = x.CanonicalAddress ,
                Description = x.Description ,
                ShortDescription = x.ShortDescription ,
                PictureTitle = x.PictureTitle ,
                PictureAlt = x.PictureAlt ,
                Slug = x.Slug ,
                Keywords = x.Keywords ,
                MetaDescription = x.MetaDescription ,
                PublishDate = x.PublishDate.ToFarsi() ,
                Title = x.Title

            }).FirstOrDefault(x => x.Id == id);
        }

        public Article GetWitCategory(long id)
        {
            return _context.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            var query = _context.Articles.Select(x => new ArticleViewModel
            {
                Id = x.Id ,
                Picture = x.Picture ,
                PublishDate = x.PublishDate.ToFarsi() ,
                Category = x.Category.Name ,
                ShortDescription = x.ShortDescription ,
                Title = x.Title
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
                query = query.Where(x => x.Title.Contains(searchModel.Title));

            if (searchModel.CategoryId > 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);

            return query.OrderByDescending(x => x.Id).AsNoTracking().ToList();
        }
    }
}
