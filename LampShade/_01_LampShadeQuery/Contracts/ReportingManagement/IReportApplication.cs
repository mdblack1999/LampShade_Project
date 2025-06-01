using System;
using System.Collections.Generic;

namespace _01_LampShadeQuery.Contracts.ReportingManagement
{
    public interface IReportApplication
    {
        ExportResult ExportGenericReport<T>(Func<ReportFilterDto, List<T>> fetchMethod, ReportFilterDto filter, string reportName);
    }
} 