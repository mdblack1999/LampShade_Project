using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleCategoryRepository : RepositoryBase<long , ArticleCategory>, IArticleCategoryRepository
    {
        private readonly BlogContext _context;
        public ArticleCategoryRepository(BlogContext context) : base(context)
        {
            _context = context;
        }

        public EditArticleCategory GetDetails(long id)
        {
            return _context.ArticleCategories.Select(x => new EditArticleCategory
            {
                Id = x.Id ,
                Name = x.Name ,
                PictureAlt = x.PictureAlt ,
                PictureTitle = x.PictureTitle ,
                ShowOrder = x.ShowOrder ,
                MetaDescription = x.MetaDescription ,
                Keywords = x.Keywords ,
                Description = x.Description ,
                CanonicalAddress = x.CanonicalAddress ,
                Slug = x.Slug
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ArticleCategoryViewModel> GetArticleCategories()
        {
            return _context.ArticleCategories.Select(x => new ArticleCategoryViewModel
            {
                Id = x.Id ,
                Name = x.Name ,
                ArticlesCount = x.Articles.Count
            }).AsNoTracking().ToList();
        }

        public string GetSlugBy(long id)
        {
            return _context.ArticleCategories.Select(x => new { x.Id , x.Slug }).FirstOrDefault(x => x.Id == id)?.Slug;
        }

        public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
        {
            var query = _context.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryViewModel
                {
                    Id = x.Id ,
                    Name = x.Name ,
                    Description = x.Description ,
                    Picture = x.Picture ,
                    ShowOrder = x.ShowOrder ,
                    CreationDate = x.CreationDate.ToFarsiFull() ,
                    ArticlesCount = x.Articles.Count
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name) || x.Name.Contains(searchModel.Description));

            return query.OrderByDescending(x => x.ShowOrder).AsNoTracking().ToList();
        }
    }
}
