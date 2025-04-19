using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscountManagement.Application.Contract.CustomerDiscount
{
    public class DefineCustomerDiscount
    {
        [Range(1 , 99 , ErrorMessage = ValidationMessages.IsInvalid)]
        public long ProductId { get; set; }

        [Range(1 , 99 , ErrorMessage = ValidationMessages.IsInvalid)]
        public int DiscountRate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string StartDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string EndDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Reason { get; set; }

        public List<ProductViewModel> Products { get; set; }
    }
}
