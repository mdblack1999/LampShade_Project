using System.Collections.Generic;
using System.IO;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using ClosedXML.Excel;

namespace _01_LampShadeQuery.Contracts.ReportingManagement.Export
{
    public class ReportExporter : IReportExporter
    {
        public byte[] ExportToExcel<T>(IEnumerable<T> data, string fileName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            // Add headers
            var properties = typeof(T).GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // Add data
            var row = 2;
            foreach (var item in data)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(row, i + 1).Value = properties[i].GetValue(item)?.ToString();
                }
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}