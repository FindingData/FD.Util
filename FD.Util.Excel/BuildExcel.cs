using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FD.Util.Excel
{
    public class BuildExcel
    {
        public BuildExcel()
        {
            this.workbook = new HSSFWorkbook();
            currentSheet = (HSSFSheet)this.workbook.CreateSheet("sheet1");
            //this.workbook.CreateSheet("sheet2");
            //this.workbook.CreateSheet("sheet3");
        }

        public BuildExcel(Stream fileStream)
        {
            workbook = new HSSFWorkbook(fileStream);
            this.currentSheet = (HSSFSheet)this.workbook.GetSheetAt(0);

        }

        /// <summary>
        /// NPOI文档流
        /// </summary>
        private HSSFWorkbook workbook = null;

        /// <summary>
        /// 当前操作页
        /// </summary>
        private HSSFSheet currentSheet = null;

        /// <summary>
        /// 当前单元
        /// </summary>
        //private HSSFCell currentCell = null;

        /// <summary>
        /// 单元样式
        /// </summary>
        //private ICellStyle cellStyle = null;


        #region 页操作
        /// <summary>
        /// 选择操作页
        /// </summary>
        /// <param name="sheetName"></param>
        public void SelectSheet(string sheetName)
        {
            currentSheet = (HSSFSheet)workbook.GetSheet(sheetName);
        }

        public void SetSheetName(int index, string sheetName)
        {
            workbook.SetSheetName(index, sheetName);
        }

        /// <summary>
        /// 自适应宽高
        /// </summary>
        /// <param name="sheetName"></param>
        public void AutoSizeColumn()
        {
            int noOfColumns = currentSheet.GetRow(0).LastCellNum;
            for (int columnNum = 0; columnNum <= noOfColumns; columnNum++)
            {
                int columnWidth = currentSheet.GetColumnWidth(columnNum) / 256;//获取当前列宽度  
                for (int rowNum = 1; rowNum <= currentSheet.LastRowNum; rowNum++)//在这一列上循环行  
                {
                    IRow currentRow = currentSheet.GetRow(rowNum);
                    ICell currentCell = currentRow.GetCell(columnNum);
                    if (currentCell != null)
                    {
                        int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度  
                        if (columnWidth < length + 1)
                        {
                            columnWidth = length + 1;
                        }//若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符  
                    }
                }
                currentSheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
        }
        #endregion

        #region--get stream
        /// <summary>
        /// 获取Excel文件流
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            stream.Position = 0;
            return stream;
        }
        #endregion

        #region--sava

        public void SaveAs(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        #endregion

        #region --insert text
        public void InsertText(string text, int row, int col)
        {
            IRow r = CellUtil.GetRow(row, currentSheet);
            if (r == null)
                r = currentSheet.CreateRow(row);
            ICell cell = CellUtil.CreateCell(r, col, text, CreateStyle());
        }

        public void Replace(string what, string replacement)
        {
            ICell cell = FindFirstCell(currentSheet, what);
            if (cell != null)
            {
                cell.SetCellValue(replacement);
            }
        }
        #endregion


        #region--GetBookmarks，GetAllMarks，DelBookmarks
        /// <summary>
        /// 获取所有书签
        /// </summary>
        /// <returns></returns>
        public List<string> GetBookmarks()
        {
            return FindAllText(currentSheet, @"《([^》]+)》"); ;
        }

        #endregion

        #region --insert table

        public void InsertTable(DataTable table, int row, int col)
        {
            ICell cell = GetCell(row, col);
            if (cell != null)
            {
                int rowCount = table.Rows.Count;
                int rowIndex = cell.RowIndex;
                InsertTable(table, rowIndex, true);
            }
        }


        public void ReplaceInsertTable(string what, DataTable table)
        {
            ICell cell = FindFirstCell(currentSheet, what);
            if (cell != null)
            {
                int rowCount = table.Rows.Count;
                int rowIndex = cell.RowIndex;
                ShiftRows(currentSheet, rowIndex + 1, rowIndex + rowCount, rowCount - 1);//keep what row
                InsertTable(table, rowIndex, false);
            }
        }

        public void InsertTable(DataTable table, int rowIndex, bool hasHeader)
        {
            InsertTable(table, rowIndex, hasHeader, GetThinBDRStyle());
        }

        private void InsertTable(DataTable table, int rowIndex, bool hasHeader, ICellStyle style)
        {
            var sheet = currentSheet;
            if (hasHeader)
            {
                var headerRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in table.Columns)
                {
                    var headerCell = headerRow.CreateCell(column.Ordinal);
                    headerCell.SetCellValue(column.ColumnName);
                    headerCell.CellStyle = style;
                }
                rowIndex++;
            }
            foreach (DataRow row in table.Rows)
            {
                var dataRow = sheet.CreateRow(rowIndex++);

                foreach (DataColumn column in table.Columns)
                {
                    var cell = dataRow.CreateCell(column.Ordinal);
                    cell.SetCellValue(row[column].ToString());
                    cell.CellStyle = style;
                }
            }
        }

        #endregion

        #region--find cell / find all
        private ICell FindFirstCell(ISheet sheet, string text)
        {
            for (int rowIndex = 0; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    ICell cell = row.GetCell(cellIndex);
                    if (cell.StringCellValue.Equals(text))
                    {
                        return cell;
                    }
                }
            }
            return null;
        }

        private List<string> FindAllText(ISheet sheet, string pattern)
        {
            List<string> labels = new List<string>();
            Regex labelRegex = new Regex(pattern);
            for (int rowIndex = 0; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    ICell cell = row.GetCell(cellIndex);
                    string strValue = cell.StringCellValue;
                    if (labelRegex.IsMatch(strValue))
                    {
                        MatchCollection matchCollection = labelRegex.Matches(strValue);
                        foreach (Match match in matchCollection)
                        {
                            labels.Add(match.Value);
                        }
                    }
                }
            }
            return labels;
        }

        #endregion

        #region--merge region
        public void MergedRegion(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            currentSheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
        }
        #endregion

        #region -- set border
        public void SetBorder(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            for (int rowIndex = firstRow; rowIndex < lastRow; rowIndex++)
            {
                var row = HSSFCellUtil.GetRow(rowIndex, currentSheet);
                for (int cellIndex = firstCol; cellIndex < lastCol; cellIndex++)
                {
                    var cell = HSSFCellUtil.GetCell(row, cellIndex);
                    cell.CellStyle = GetThinBDRStyle();
                }
            }
        }

        public void SetFont(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            for (int rowIndex = firstRow; rowIndex < lastRow; rowIndex++)
            {
                var row = HSSFCellUtil.GetRow(rowIndex, currentSheet);
                for (int cellIndex = firstCol; cellIndex < lastCol; cellIndex++)
                {
                    var cell = HSSFCellUtil.GetCell(row, cellIndex);
                    ICellStyle style = workbook.CreateCellStyle(); //务必创建新的CellStyle,否则所有单元格同样式
                    IFont font = workbook.CreateFont();
                    font.Boldweight = short.MaxValue;
                    style.SetFont(font);
                    cell.CellStyle = style;
                }
            }
        }

        private void SetBorderLeft(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            HSSFRegionUtil.SetBorderLeft(BorderStyle.Thin, new CellRangeAddress(firstRow, lastRow, firstCol, lastCol), currentSheet, workbook);
        }

        private void SetBorderRight(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            HSSFRegionUtil.SetBorderRight(BorderStyle.Thin, new CellRangeAddress(firstRow, lastRow, firstCol, lastCol), currentSheet, workbook);
        }

        private void SetBorderBottom(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            HSSFRegionUtil.SetBorderTop(BorderStyle.Thin, new CellRangeAddress(firstRow, lastRow, firstCol, lastCol), currentSheet, workbook);
        }

        private void SetBorderTop(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            HSSFRegionUtil.SetBorderTop(BorderStyle.Thin, new CellRangeAddress(firstRow, lastRow, firstCol, lastCol), currentSheet, workbook);
        }

        private ICellStyle GetThinBDRStyle()
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        private void SetCellBorder(ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {

            for (int rowIndex = 0; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    ICell cell = row.GetCell(cellIndex);
                    cell.CellStyle = GetThinBDRStyle();
                }
            }
        }
        #endregion

        #region--set style
        private ICellStyle CreateStyle()
        {
            ICellStyle style = workbook.CreateCellStyle();
            return style;
        }

        /// <summary>
        /// 设置单元居中
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void SetCellCenter(int row, int col)
        {
            ICell cell = GetCell(row, col);
            ICellStyle style = cell.CellStyle;
            style.Alignment = HorizontalAlignment.Center;
            cell.CellStyle = style;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontHeight"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void SetCellFont(int fontHeight, int row, int col)
        {
            ICell cell = GetCell(row, col);
            ICellStyle style = cell.CellStyle;
            IFont font = workbook.CreateFont();
            font.FontHeight = (short)fontHeight;
            style.SetFont(font);
            cell.CellStyle = style;

        }

        public void SetCellBlod(int row, int col)
        {
            ICell cell = GetCell(row, col);
            ICellStyle style = cell.CellStyle;
            IFont font = workbook.CreateFont();
            font.Boldweight = short.MaxValue;
            style.SetFont(font);
            cell.CellStyle = style;
        }
        #endregion

        #region-- get row/cell
        /// <summary>
        /// 获取单元对象
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private ICell GetCell(int row, int col)
        {
            IRow curRow = GetRow(row);
            return CellUtil.GetCell(curRow, col);
        }

        /// <summary>
        /// 获取行对象
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private IRow GetRow(int row)
        {
            return CellUtil.GetRow(row, currentSheet);
        }
        #endregion

        #region--debug
        [Conditional("DEBUG")]
        public void PrintCurrentSheet()
        {
            for (int rowIndex = 0; rowIndex < currentSheet.LastRowNum; rowIndex++)
            {
                IRow row = currentSheet.GetRow(rowIndex);
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    Console.WriteLine(row.GetCell(cellIndex).StringCellValue);
                }
            }
        }
        #endregion

        #region DataTable helper
        /// <summary>
        /// 移动行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fromRowIndex"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="n"></param>
        private void ShiftRows(ISheet sheet, int fromRowIndex, int endRowIndex, int n)
        {
            sheet.ShiftRows(fromRowIndex, endRowIndex, n, false, true);
        }

        /// <summary>
        /// 拷贝行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceRowIndex"></param>
        /// <param name="formRowIndex"></param>
        /// <param name="n"></param>
        private void CopyRows(ISheet sheet, int sourceRowIndex, int formRowIndex, int n)
        {
            for (int i = formRowIndex; i < formRowIndex + n; i++)
            {
                SheetUtil.CopyRow(sheet, sourceRowIndex, i);
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fromRowIndex"></param>
        /// <param name="n"></param>
        //private void InsertRows(ISheet sheet, int fromRowIndex, int n,ICell style)
        //{
        //    for (int rowIndex = fromRowIndex; rowIndex < fromRowIndex + n; rowIndex++)
        //    {
        //        IRow rowSource = sheet.GetRow(rowIndex + n);
        //        IRow rowInsert = sheet.CreateRow(rowIndex);
        //        rowInsert.Height = rowSource.Height;
        //        for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
        //        {
        //            ICell cellSource = rowSource.GetCell(colIndex);
        //            ICell cellInsert = rowInsert.CreateCell(colIndex);
        //            if (cellSource != null)
        //            {
        //                cellInsert.CellStyle = cellSource.CellStyle;
        //            }
        //        }
        //    }
        //}
        #endregion


        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public byte[] TableToExcel(DataTable dt)
        {
            try
            {                
                int sheetCount = 1;
                //if(dt.Rows.Count>0)
                //{
                sheetCount = (dt.Rows.Count / 65530) + 1;
                //}

                for (int k = 0; k < sheetCount; k++)
                {
                    ISheet sheet =  sheetCount > 1 ? workbook.CreateSheet("Sheet" + (k + 1).ToString()) : this.currentSheet;

                    //表头  
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }
                    int sheetRowCount = (k + 1) * 65530;
                    int sheetRowIndex = 0;
                    sheetRowCount = dt.Rows.Count <= sheetRowCount ? dt.Rows.Count : sheetRowCount;

                    for (int i = 65530 * k; i < sheetRowCount; i++)
                    {
                        IRow rowTemp = sheet.CreateRow(sheetRowIndex + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j] != System.DBNull.Value && dt.Rows[i][j] != null)
                            {
                                ICell cell = rowTemp.CreateCell(j);
                                cell.SetCellValue(dt.Rows[i][j].ToString());
                            }

                        }
                        sheetRowIndex++;
                    }

                    for (int columnNum = 0; columnNum < dt.Columns.Count; columnNum++)
                    {
                        int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                        IRow currentRow;
                        //当前行未被使用过
                        if (sheet.GetRow(0) == null)
                        {
                            currentRow = sheet.CreateRow(0);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(0);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                        sheet.SetColumnWidth(columnNum, (columnWidth + 1) * 256);
                    }
                }
 
                //转为字节数组  
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }



    }
}
