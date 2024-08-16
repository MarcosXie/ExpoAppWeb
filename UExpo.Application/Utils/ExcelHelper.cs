using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace UExpo.Application.Utils;

public static class ExcelHelper
{

    public static List<Dictionary<string, object>> ToDictionary(this IFormFile file)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

        using MemoryStream stream = new MemoryStream();

        file.CopyTo(stream);

        using ExcelPackage package = new ExcelPackage(stream);

        ExcelWorksheet sheet = package.Workbook.Worksheets[0];
        int rowCount = sheet.Dimension.Rows;
        int colCount = sheet.Dimension.Columns;

        List<string> headers = new List<string>();

        for (int col = 1; col <= colCount; col++)
            headers.Add(sheet.Cells[1, col].Text);

        for (int row = 2; row <= rowCount; row++)
        {
            Dictionary<string, object> rowDict = new Dictionary<string, object>();

            for (int col = 1; col <= colCount; col++)
            {
                rowDict[headers[col - 1]] = sheet.Cells[row, col].Text;
            }

            result.Add(rowDict);
        }

        return result;
    }
}
