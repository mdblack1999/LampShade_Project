﻿using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository , IFileUploader fileUploader)
        {
            _productCategoryRepository = productCategoryRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_productCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();

            var picturePath = $"{command.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture , picturePath);

            var productCategory = new ProductCategory(command.Name , command.Description , pictureName ,
                command.PictureAlt , command.PictureTitle , command.Keywords , command.MetaDescription , command.SubText , slug);

            _productCategoryRepository.Create(productCategory);
            _productCategoryRepository.SaveChanges();

            return operation.Succeeded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);

            if (productCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();

            var picturePath = $"{command.Slug}";
            var fileName = _fileUploader.Upload(command.Picture , picturePath);

            productCategory.Edit(command.Name , command.Description , fileName , command.PictureAlt
                , command.PictureTitle , command.Keywords , command.MetaDescription , command.SubText , slug);

            _productCategoryRepository.SaveChanges();
            return operation.Succeeded();

        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();
            var category = _productCategoryRepository.Get(id);
            if (category == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            category.Remove();
            _productCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();
            var category = _productCategoryRepository.Get(id);
            if (category == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            category.Restore();
            _productCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }



        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return _productCategoryRepository.GetProductCategories();
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }
    }
}
