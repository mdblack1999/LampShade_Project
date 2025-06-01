using System.Collections.Generic;
using _01_LampShadeQuery.Contracts.ReportingManagement.Dto;

namespace _01_LampShadeQuery.Contracts.ReportingManagement.Interface
{
    public interface IReportRepository
    {
        List<OrderReportDto> FetchOrderReports(ReportFilterDto filter);
        List<ProductReportDto> FetchProductReports(ReportFilterDto filter);
        List<CategoryReportDto> FetchCategoryReports(ReportFilterDto filter);
        List<UserReportDto> FetchUserReports(ReportFilterDto filter);
        List<ArticleReportDto> FetchArticleReports(ReportFilterDto filter);
        List<CustomerDiscountReportDto> FetchCustomerDiscountReports(ReportFilterDto filter);
        List<ColleagueDiscountReportDto> FetchColleagueDiscountReports(ReportFilterDto filter);
        List<InventoryReportDto> FetchInventoryReports(ReportFilterDto filter);
        List<CommentReportDto> FetchCommentReports(ReportFilterDto filter);
    }
} 