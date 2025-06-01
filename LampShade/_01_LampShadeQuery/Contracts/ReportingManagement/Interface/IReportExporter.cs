using System.Collections.Generic;

namespace _01_LampShadeQuery.Contracts.ReportingManagement.Interface
{
    public interface IReportExporter
    {
        ExportResult ExportToExcel<T>(List<T> data, string fileName);
    }
} 