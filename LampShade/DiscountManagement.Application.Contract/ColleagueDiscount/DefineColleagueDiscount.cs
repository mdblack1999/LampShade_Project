using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscountManagement.Application.Contract.ColleagueDiscount
{
    public class DefineColleagueDiscount
    {
        [Range(1,10000000,ErrorMessage =ValidationMessages.IsInvalid)]
        public long ProductId { get; set; }
        [Range(1 , 99 , ErrorMessage = ValidationMessages.IsInvalid)]
        public int DiscountRate { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
