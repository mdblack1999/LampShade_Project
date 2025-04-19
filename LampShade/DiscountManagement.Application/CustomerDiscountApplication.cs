using _0_Framework.Application;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDsicountRepository _customerDsicountRepository;

        public CustomerDiscountApplication(ICustomerDsicountRepository csutoemrDsicountRepository)
        {
            _customerDsicountRepository = csutoemrDsicountRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            var operation = new OperationResult();
            if (_customerDsicountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId , command.DiscountRate , startDate ,
                endDate , command.Reason);
            _customerDsicountRepository.Create(customerDiscount);
            _customerDsicountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditCustomerDiscount command)
        {
            var operation = new OperationResult();
            var customerDiscount = _customerDsicountRepository.Get(command.Id);
            if (customerDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_customerDsicountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate &&  x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            customerDiscount.Edit(command.ProductId,command.DiscountRate,startDate,endDate,command.Reason);

            _customerDsicountRepository.SaveChanges();
            return operation.Succeeded();

        }

        public EditCustomerDiscount GetDetails(long id)
        {
           return _customerDsicountRepository.GetDetails(id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
           return _customerDsicountRepository.Search(searchModel);
        }
    }
}
