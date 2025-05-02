using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampShadeQuery.Contracts.Article;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampShadeQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _context;

        public ArticleQuery(BlogContext context)
        {
            _context = context;
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

            if (article != null && !string.IsNullOrWhiteSpace(article.Keywords))
                article.KeywordList = article.Keywords.Split(new[] { ',' , '-' , '.' }).ToList();

            return article;
        }
    }
}
