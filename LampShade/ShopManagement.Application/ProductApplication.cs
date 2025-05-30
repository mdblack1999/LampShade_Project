﻿using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using System.Collections.Generic;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductApplication(IProductRepository productRepository , IFileUploader fileUploader , IProductCategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _fileUploader = fileUploader;
            _categoryRepository = categoryRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            var operation = new OperationResult();
            if (_productRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();

            var categorySlug = _categoryRepository.GetSlugBy(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture , path);

            var product = new Product(command.Name , command.Code ,
               command.ShortDescription , command.Description , picturePath ,
               command.PictureAlt , command.PictureTitle , command.CategoryId , slug ,
               command.Keywords , command.MetaDescription);

            _productRepository.Create(product);
            _productRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditProduct command)
        {
            var operation = new OperationResult();
            var product = _productRepository.GetProductWithCategory(command.Id);
            if (product == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var path = $"{product.Category.Slug}//{slug}";

            var picturePath = _fileUploader.Upload(command.Picture , path);

            product.Edit(command.Name , command.Code ,
               command.ShortDescription , command.Description , picturePath ,
               command.PictureAlt , command.PictureTitle , command.CategoryId , slug
               , command.Keywords , command.MetaDescription);


            _productRepository.SaveChanges();
            return operation.Succeeded();
        }

        public EditProduct GetDetails(long id)
        {
            return _productRepository.GetDetails(id);

        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        #region InStock Old
        //public OperationResult IsStock(long id)
        //{
        //    var operation = new OperationResult();
        //    var product = _productRepository.Get(id);
        //    if (product == null)
        //        return operation.Failed(ApplicationMessages.RecordNotFound);
        //    //در انبار موجود شود
        //    product.InStock();
        //    _productRepository.SaveChanges();
        //    return operation.Succeeded();
        //}

        //public OperationResult NotInStock(long id)
        //{
        //    var operation = new OperationResult();
        //    var product = _productRepository.Get(id);
        //    if (product == null)
        //        return operation.Failed(ApplicationMessages.RecordNotFound);
        //    //عدم  موجودی در انبار
        //    product.NotInStock();
        //    _productRepository.SaveChanges();
        //    return operation.Succeeded();
        //}
        #endregion

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return _productRepository.Search(searchModel);
        }
    }
}
