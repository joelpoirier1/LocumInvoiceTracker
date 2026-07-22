using ClosedXML.Excel;
using LocumInvoiceTracker.Models;

namespace LocumInvoiceTracker.Services;

public sealed class ExcelExportService
{
    public byte[] CreateShiftReport(
        string hospitalName,
        DateTime? from,
        DateTime? to,
        IReadOnlyCollection<WorkShift> shifts)
    {
        using var workbook = new XLWorkbook();

        var sheet = workbook.Worksheets.Add("Shift Report");

        sheet.Cell(1, 1).Value = "Locum Shift Report";

        var titleRange = sheet.Range(1, 1, 1, 5);
        titleRange.Merge();
        titleRange.Style.Font.Bold = true;
        titleRange.Style.Font.FontSize = 16;

        sheet.Cell(2, 1).Value = "Hospital";
        sheet.Cell(2, 2).Value = hospitalName;

        sheet.Cell(3, 1).Value = "Date range";
        sheet.Cell(3, 2).Value =
            $"{from?.ToString("yyyy-MM-dd") ?? "All"} to " +
            $"{to?.ToString("yyyy-MM-dd") ?? "All"}";

        string[] headers =
        [
            "Date",
            "Hospital",
            "Hours",
            "Hourly Rate",
            "Total"
        ];

        for (var column = 0; column < headers.Length; column++)
        {
            sheet.Cell(5, column + 1).Value = headers[column];
        }

        var headerRange = sheet.Range(5, 1, 5, headers.Length);
        headerRange.Style.Font.Bold = true;

        var row = 6;

        foreach (var shift in shifts)
        {
            sheet.Cell(row, 1).Value = shift.Date;
            sheet.Cell(row, 1).Style.DateFormat.Format = "yyyy-mm-dd";

            sheet.Cell(row, 2).Value =
                shift.Hospital?.Name ?? hospitalName;

            sheet.Cell(row, 3).Value = shift.HoursWorked;
            sheet.Cell(row, 4).Value = shift.HourlyRate;
            sheet.Cell(row, 5).Value = shift.Total;

            sheet.Cell(row, 4).Style.NumberFormat.Format = "$#,##0.00";
            sheet.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";

            row++;
        }

        sheet.Cell(row, 4).Value = "Total";
        sheet.Cell(row, 4).Style.Font.Bold = true;

        sheet.Cell(row, 5).Value =
            shifts.Sum(shift => shift.Total);

        sheet.Cell(row, 5).Style.Font.Bold = true;
        sheet.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}