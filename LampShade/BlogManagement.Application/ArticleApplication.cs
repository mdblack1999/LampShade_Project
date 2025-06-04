using System.Collections.Generic;
using _0_Framework.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.Extensions.Configuration;

namespace BlogManagement.Application
{
    public class ArticleApplication : IArticleApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IArticleRepository _articleRepository;
        private readonly IConfiguration _configuration;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly string _siteUrl;

        public ArticleApplication(IArticleRepository articleRepository , IFileUploader fileUploader , IArticleCategoryRepository articleCategoryRepository , IConfiguration configuration)
        {
            _articleRepository = articleRepository;
            _fileUploader = fileUploader;
            _articleCategoryRepository = articleCategoryRepository;
            _configuration = configuration;
            _siteUrl = configuration["Payment:siteUrl"] ?? string.Empty;
        }

        public OperationResult Create(CreateArticle command)
        {
            var operation = new OperationResult();

            if (_articleRepository.Exists(x => x.Title == command.Title))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var categorySlug = _articleCategoryRepository.GetSlugBy(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var pictureName = _fileUploader.Upload(command.Picture , path);
            var publishDate = command.PublishDate.ToGregorianDateTime().Date;

            string canonical = command.CanonicalAddress?.Trim();
            if (!string.IsNullOrWhiteSpace(canonical))
            {
                if (!canonical.StartsWith("http://") && !canonical.StartsWith("https://"))
                {
                    canonical = $"{_siteUrl.TrimEnd('/')}/{canonical.TrimStart('/')}";
                }
            }

            var article = new Article(command.Title , command.ShortDescription , command.Description , pictureName ,
                command.PictureAlt , command.PictureTitle , publishDate , slug , command.Keywords ,
                command.MetaDescription , canonical , command.CategoryId);

            _articleRepository.Create(article);
            _articleRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditArticle command)
        {
            var operation = new OperationResult();
            var article = _articleRepository.GetWitCategory(command.Id);

            if (article == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_articleRepository.Exists(x => x.Title == command.Title && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var path = $"{article.Category.Slug}/{slug}";
            var pictureName = _fileUploader.Upload(command.Picture , path);
            var publishDate = command.PublishDate.ToGregorianDateTime().Date;

            string canonical = command.CanonicalAddress?.Trim();
            if (!string.IsNullOrWhiteSpace(canonical))
            {
                if (!canonical.StartsWith("http://") && !canonical.StartsWith("https://"))
                {
                    canonical = $"{_siteUrl.TrimEnd('/')}/{canonical.TrimStart('/')}";
                }
            }

            article.Edit(command.Title , command.ShortDescription , command.Description , pictureName ,
                command.PictureAlt , command.PictureTitle , publishDate , slug , command.Keywords , command.MetaDescription ,
                canonical , command.CategoryId);

            _articleRepository.SaveChanges();
            return operation.Succeeded();
        }

        public EditArticle GetDetails(long id)
        {
            return _articleRepository.GetDetails(id);
        }

        public void IncreaseVisitCount(long id)
        {
            var article = _articleRepository.GetForUpdate(id);
            if (article == null) return;
            article.IncreaseVisitCount();
            _articleRepository.SaveChanges();
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            return _articleRepository.Search(searchModel);
        }

        public List<ArticleViewModel> GetAllArticles()
        {
            return _articleRepository.GetAllArticles();
        }
    }
}
