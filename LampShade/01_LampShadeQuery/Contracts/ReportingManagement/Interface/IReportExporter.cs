using System.Collections.Generic;

namespace _01_LampShadeQuery.Contracts.ReportingManagement.Interface
{
    public interface IReportExporter
    {
        byte[] ExportToExcel<T>(IEnumerable<T> data, string fileName);
    }
}
