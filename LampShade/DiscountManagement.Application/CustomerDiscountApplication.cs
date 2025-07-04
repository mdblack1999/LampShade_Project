﻿using _0_Framework.Application;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository)
        {
            _customerDiscountRepository = customerDiscountRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            var operation = new OperationResult();
            if (_customerDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGregorianDateTime();
            var endDate = command.EndDate.ToGregorianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId , command.DiscountRate , startDate ,
                endDate , command.Reason);
            _customerDiscountRepository.Create(customerDiscount);
            _customerDiscountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditCustomerDiscount command)
        {
            var operation = new OperationResult();
            var customerDiscount = _customerDiscountRepository.Get(command.Id);
            if (customerDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_customerDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate &&  x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGregorianDateTime();
            var endDate = command.EndDate.ToGregorianDateTime();
            customerDiscount.Edit(command.ProductId,command.DiscountRate,startDate,endDate,command.Reason);

            _customerDiscountRepository.SaveChanges();
            return operation.Succeeded();

        }

        public EditCustomerDiscount GetDetails(long id)
        {
           return _customerDiscountRepository.GetDetails(id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
           return _customerDiscountRepository.Search(searchModel);
        }

        public List<CustomerDiscountViewModel> GetAllCustomerDiscount()
        {
            return _customerDiscountRepository.GetAllCustomerDiscount();
        }
    }
}
