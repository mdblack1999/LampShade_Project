using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_LampShadeQuery.Contracts.ReportingManagement.Interface
{
    public interface IReportApplication
    {
        ExportResult ExportOrderReportsExcel(ReportFilterDto filter);
        ExportResult ExportProductReportsExcel(ReportFilterDto filter);
        ExportResult ExportCategoryReportsExcel(ReportFilterDto filter);
        ExportResult ExportUserReportsExcel(ReportFilterDto filter);
        ExportResult ExportArticleReportsExcel(ReportFilterDto filter);
        ExportResult ExportCustomerDiscountReportsExcel(ReportFilterDto filter);
        ExportResult ExportColleagueDiscountReportsExcel(ReportFilterDto filter);
        ExportResult ExportInventoryReportsExcel(ReportFilterDto filter);
        ExportResult ExportCommentReportsExcel(ReportFilterDto filter);
    }
} 