using _0_Framework.Application;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustoemrDsicountRepository _custoemrDsicountRepository;

        public CustomerDiscountApplication(ICustoemrDsicountRepository csutoemrDsicountRepository)
        {
            _custoemrDsicountRepository = csutoemrDsicountRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            var operation = new OperationResult();
            if (_custoemrDsicountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId , command.DiscountRate , startDate ,
                endDate , command.Reason);
            _custoemrDsicountRepository.Create(customerDiscount);
            _custoemrDsicountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditCustomerDiscount command)
        {
            var operation = new OperationResult();
            var customerDiscount = _custoemrDsicountRepository.Get(command.Id);
            if (customerDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_custoemrDsicountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate &&  x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            customerDiscount.Edit(command.ProductId,command.DiscountRate,startDate,endDate,command.Reason);

            _custoemrDsicountRepository.SaveChanges();
            return operation.Succedded();

        }

        public EditCustomerDiscount GetDetails(long id)
        {
           return _custoemrDsicountRepository.GetDetails(id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
           return _custoemrDsicountRepository.Search(searchModel);
        }
    }
}
