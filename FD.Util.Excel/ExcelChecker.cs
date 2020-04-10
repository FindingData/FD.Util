using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using FD.Util.Excel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace FD.Util.Excel
{
    public class ExcelChecker
    {       
        private IWorkbook _workbook; //excel对象

        private ExcelStructure _excelStructure; //excel文档结构
     
        private Stream _excelStream; //excel流

        /// <summary>
        /// 进度改变事件
        /// </summary>
        public event Action<int> CheckProgressChanged;

        public DataTable Table { get; set; }//excel解析对象


        private TemplateRule _templateRule;  // 确定匹配的模板规则   

        private IList<TemplateRule> _templateRules;// 需要自动匹配的模板规则   
     


        private decimal _customerId;

        private int _checkType=0;

        // 检查错误详情

        public List<ExcelCheckError> ErrorList = new List<ExcelCheckError>();  //错误汇总

        private Dictionary<string, List<string>> _errorListExcelStructure = new Dictionary<string, List<string>>();  //excel结构错误信息

        private Dictionary<string, List<string>> _errorListColumn = new Dictionary<string, List<string>>(); //excel列错误信息

        private Dictionary<string, List<string>> _errorListReadExcel = new Dictionary<string, List<string>>(); //excel读取错误信息

        private Dictionary<string, List<string>> _errorListTemplateMatch = new Dictionary<string, List<string>>(); //excel与模版匹配错误信息

        private Dictionary<string, List<string>> _errorListDataColumnCheck = new Dictionary<string, List<string>>();   // 列数据检错错误

        private Dictionary<string, List<string>> _errorListPrimaryCheck = new Dictionary<string, List<string>>();   // 列数据唯一性检错错误
        
        private List<string> tmpList = new List<string>();
        #region 类型格式化

        /// <summary>
        /// integer,number
        /// </summary>
        readonly string[] _fromNumeric = {"integer", "number"};

        /// <summary>
        /// integer
        /// </summary>
        readonly string[] _fromInt = {"integer"};

        /// <summary>
        /// number
        /// </summary>
        readonly string[] _fromDouble = {"number"};

        /// <summary>
        /// date,datetime
        /// </summary>
        readonly string[] _fromData = {"date"}; //{ "date", "datetime" };

        /// <summary>
        /// nvarchar2,varchar2
        /// </summary>
        readonly string[] _fromString = {"nvarchar2", "varchar2"};

        /// <summary>
        /// single_dic
        /// </summary>
        readonly string[] _fromSingleDic = {"single_dic"};

        /// <summary>
        /// multi_dic
        /// </summary>
        readonly string[] _fromMultiDic = {"multi_dic"}; //还未处理

        #endregion

        public ExcelChecker() { }

        public ExcelChecker(decimal customerId, Stream stream)
        {
            _customerId = customerId;
            _excelStream = stream;
        }



        public void StartCheck()
        {
            if (!ReadExcelFile(_excelStream))
            {
                ProgressChangeEvent(5);
                AddCheckError("读取文件失败", _errorListReadExcel);
                return;
            }
            ProgressChangeEvent(1);
            if (!ReadExcelStructure())
            {
                ProgressChangeEvent(4);
                AddCheckError("Excel表头解析失败", _errorListExcelStructure);
                return;
            }
            ProgressChangeEvent(1);
            if (_templateRule == null) //查找模版并匹配
            {
                if (!MatchExcelTemplates())
                {
                    ProgressChangeEvent(3);
                    AddCheckError("模版匹配失败", _errorListTemplateMatch);
                    return;
                }
            }
            else
            {
                if (!MatchExcelTemplate(_templateRule))
                {
                    ProgressChangeEvent(3);
                    AddCheckError("模版匹配失败", _errorListTemplateMatch);
                    return;
                }
            }
            ProgressChangeEvent(1);
            if (!ReadDataTable())
            {
                ProgressChangeEvent(2);
                AddCheckError("列数据检查失败", _errorListColumn);
            }
            ProgressChangeEvent(1);
            if (!CheckDataTable())
            {
                ProgressChangeEvent(1);
                AddCheckError("数据检查失败", _errorListDataColumnCheck);   
            }
            ProgressChangeEvent(1);
            if(_checkType==1){
                if (!PrimaryCheck())
                {
                    ProgressChangeEvent(1);
                    AddCheckError("数据检查失败", _errorListPrimaryCheck);
                }
            }
            ProgressChangeEvent(1);
        }         

        /// <summary>
        /// 设置模版检测规则
        /// </summary>
        /// <param name="templateRule"></param>
        public void SetTemplateRule(TemplateRule templateRule)
        {
            this._templateRule = templateRule;
        }

        /// <summary>
        /// 设置模版检测规则
        /// </summary>
        public void SetTemplateRules(IList<TemplateRule> templateRules)
        {
            this._templateRules = templateRules;
        }


        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool ReadExcelFile(Stream stream)
        {            
            try
            {
                if (stream == null)
                {
                    if (!_errorListReadExcel.Keys.Contains("文件不存在"))
                        _errorListReadExcel.Add("文件不存在", new List<string>());
                    tmpList = _errorListReadExcel["文件不存在"];
                    tmpList.Add("指定文件不存在！");
                    return false;
                }
                _workbook = WorkbookFactory.Create(stream);
            }
            catch (Exception ex)
            {
                if (!_errorListReadExcel.Keys.Contains("文件读取失败"))
                    _errorListReadExcel.Add("文件读取失败", new List<string>());
                tmpList = _errorListReadExcel["文件读取失败"];
                tmpList.Add("---读取Excel文件时遇到未知错误，如果确定文件有效并且未被其它软件打开，请联系系统管理员!\r\n" + ex.Message);
            }
            if (_errorListReadExcel.Keys.Count <= 0) return _workbook != null;

            return false;
        }

        /// <summary>
        /// 读取Excel表结构
        /// </summary>
        /// <returns></returns>
        private bool ReadExcelStructure()
        {        
            try
            {
                if(_excelStructure == null) _excelStructure = new ExcelStructure();
                
                var sheetList = _excelStructure._SheetStructures;

                var excelTablesName = GetExcelTablesName(); //获取excel表头
                if (excelTablesName == null || !excelTablesName.Any()) //1.0、无工作表
                {
                    if (!_errorListExcelStructure.Keys.Contains("无工作表"))
                        _errorListExcelStructure.Add("无工作表", new List<string>());
                    tmpList = _errorListExcelStructure["无工作表"];
                    tmpList.Add("没有找到工作表，请检查Excel是否有效！");
                    return false;
                }

                var strSheet = excelTablesName[0]; //只取第一个工作表
                if (string.IsNullOrEmpty(strSheet.Trim())) //1.1、工作表名称无效
                {
                    if (!_errorListExcelStructure.Keys.Contains("工作表名称无效"))
                        _errorListExcelStructure.Add("工作表名称无效", new List<string>());
                    tmpList = _errorListExcelStructure["工作表名称无效"];
                    tmpList.Add("工作表名称无效");
                    return false;
                }
                var aSheet = new ExcelSheetStructure()
                {
                    SheetIndex = 0,
                    ExcelSheetName = strSheet.Trim()
                };
                var columnList = aSheet._ColumnStructures;
                //从npoi中读取excel结构
                var sheet = _workbook.GetSheet(strSheet);
                #region -- 构造aSheet

                var headerRow = sheet.GetRow(0);//第一行为head
                //2017-12-11修改
                //if (headerRow == null) //1.2、存在空工作表 
                //{
                //    if (!_errorListExcelStructure.Keys.Contains("工作表内容为空"))
                //        _errorListExcelStructure.Add("工作表内容为空", new List<string>());
                //    tmpList = _errorListExcelStructure["工作表内容为空"];
                //    tmpList.Add("工作表内容为空");
                //    return false;
                //}

                //添加aSheet
                int lastCellNum = headerRow.LastCellNum;
                var nullColError = ""; //空列头
                var duplicateColError = ""; //重复列头
                for (int columnIndex = 0; columnIndex < lastCellNum; columnIndex++)
                {
                    var cell = headerRow.GetCell(columnIndex);
                    var aColumn = new ExcelColumnStructure()
                    {
                        ColumnIndex =  columnIndex
                    };
                    try
                    {
                        if (cell == null || cell.ToString().Trim() == "") //1.3、存在为空的列名称
                        {
                            nullColError += string.Format("---工作表,第{0}列，列名称为空！\r\n", columnIndex + 1);
                        }
                        else
                        {
                            var value = cell.CellType == CellType.String
                                ? cell.StringCellValue.Trim()
                                : cell.ToString().Trim();
                            //存在重复列
                            if (columnList.Any(c => c.ExcelColumn == Convert.ToString(value)))
                            {
                                duplicateColError += string.Format("---工作表,第{0}列，列名称重复！\r\n", columnIndex + 1);
                            }
                            else
                            {
                                aColumn.ExcelColumn = value;
                                columnList.Add(aColumn);
                            }
                        }
                    }
                    catch (Exception )
                    {
                        if (!_errorListExcelStructure.Keys.Contains("列名称读取失败"))
                            _errorListExcelStructure.Add("列名称读取失败",new List<string>());
                        tmpList = _errorListExcelStructure["列名称读取失败"];
                        tmpList.Add("列名称读取失败");
               
                    }
                }

                if (_errorListExcelStructure.Keys.Contains("列名称读取失败"))
                    return false;

                if (!string.IsNullOrEmpty(duplicateColError))
                {
                    if (!_errorListExcelStructure.Keys.Contains("列名称重复"))
                        _errorListExcelStructure.Add("列名称重复", new List<string>());
                    tmpList = _errorListExcelStructure["列名称重复"];
                    tmpList.Add(duplicateColError);
                    return false;
                }

                if (!string.IsNullOrEmpty(nullColError))
                {
                    if (!_errorListExcelStructure.Keys.Contains("列名称为空"))
                        _errorListExcelStructure.Add("列名称为空", new List<string>());
                    tmpList = _errorListExcelStructure["列名称为空"];
                    tmpList.Add(nullColError);
                    return false;
                }
                
                sheetList.Add(aSheet); //添加到sheet                
                return true;
                #endregion

            }
            catch (Exception ex)
            {
                if (!_errorListExcelStructure.Keys.Contains("读取表结构出现错误"))
                    _errorListExcelStructure.Add("读取表结构出现错误", new List<string>());
                tmpList = _errorListExcelStructure["读取表结构出现错误"];
                tmpList.Add(ex.Message);
              
            }
            return false;

        }

        /// <summary>
        /// 将要导入的数据与所有模版匹配（工具检测）
        /// </summary>
        /// <returns></returns>
        private bool MatchExcelTemplates()
        {
            if (_templateRules == null)
            {
                return false;
            }
            try
            {
                var aSheetStructure = _excelStructure.SheetStructure(0); //只取第一个工作表
                //var aTemplateRule = _templateRules.Where(t => t.SheetContains(aSheetStructure.ExcelSheetName)).FirstOrDefault();
                var aTemplateRule = _templateRules.Where(t => t.TemplateSheetRule.SheetName == aSheetStructure.ExcelSheetName).FirstOrDefault();
                if (aTemplateRule == null)
                {
                    _errorListTemplateMatch.Add("匹配工作表失败", new List<string>());
                    tmpList = _errorListTemplateMatch["匹配工作表失败"];
                    tmpList.Add(string.Format("---Excel中的工作表[{0}]不能匹配任何模板,注:只取第一个工作表.",
                        aSheetStructure.ExcelSheetName));
                    return false;
                }
                if (MatchExcelTemplate(aTemplateRule))
                {
                    _templateRule = aTemplateRule;
                    _checkType = 1;
                }                
            }
            catch (Exception ex)
            {
                if (!_errorListTemplateMatch.Keys.Contains("模版匹配出现错误"))
                {
                    _errorListTemplateMatch.Add("模版匹配出现错误", new List<string>());
                    tmpList = _errorListTemplateMatch["模版匹配出现错误"];
                    tmpList.Add(ex.Message);
     
                }
            }
            return _templateRule != null;
        }
       
        /// <summary>
        /// 将要导入的数据与模板进行匹配
        /// </summary>
        /// <returns></returns>
        private bool MatchExcelTemplate(TemplateRule templateRule)
        {
            try
            {
                var aSheetStructure = _excelStructure.SheetStructure(0); //只取第一个工作表
                //if (!templateRule.SheetContains(aSheetStructure.ExcelSheetName))
                //{
                //    if (!_errorListTemplateMatch.Keys.Contains("匹配工作表失败"))
                //    {
                //        _errorListTemplateMatch.Add("匹配工作表失败", new List<string>());
                //        tmpList = _errorListTemplateMatch["匹配工作表失败"];
                //        tmpList.Add(string.Format("---Excel中不存在工作表[{0}],注:只取第一个工作表.", templateRule.TemplateSheetRule.SheetName));
                //    }
                //    return false;
                //}
                //一旦匹配成功,清除匹配失败错误
                
                var templateSheetColList = templateRule.SheetRule(aSheetStructure.ExcelSheetName).ColumnNameList();
                //未匹配上的列
                var unMatchColumns = templateSheetColList
                    .Where(colName => !aSheetStructure.ColumnContains(colName))
                    .ToList();

                if (unMatchColumns.Any())
                {
                    var message = String.Format("----工作表[{0}]不包含列:[{1}]与模板《{2}》，不匹配！"
                                , aSheetStructure.ExcelSheetName
                                , ConvertListToString(unMatchColumns, "]、[")
                                , templateRule.TemplateName);
                    if (!_errorListTemplateMatch.Keys.Contains("匹配列失败"))
                    {
                        _errorListTemplateMatch.Add("匹配列失败", new List<string>());
                        tmpList = _errorListTemplateMatch["匹配列失败"];
                        tmpList.Add(message);
                    }
                    return false;                    
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!_errorListTemplateMatch.Keys.Contains("模版匹配出现错误"))
                {
                    _errorListTemplateMatch.Add("模版匹配出现错误", new List<string>());
                    tmpList= _errorListTemplateMatch["模版匹配出现错误"];
                    tmpList.Add(ex.Message);
           
                }
            }
            return false;
        }




        /// <summary>
        /// 将数据读取到DataTable数据结构
        /// </summary>
        /// <returns></returns>
        private bool ReadDataTable()
        {
            if (_templateRule == null)
            {
                return false;
            }
            
            var aSheetStructure = _excelStructure.SheetStructure(0); //取第一个sheet读取
            //var aSheetRule = _templateRule.SheetRule(aSheetStructure.ExcelSheetName); //取对应规则
            var sheet = _workbook.GetSheetAt(aSheetStructure.SheetIndex);
            var table = new DataTable(aSheetStructure.ExcelSheetName);
          
            try
            {            
                foreach (var col in aSheetStructure._ColumnStructures)
                {
                    table.Columns.Add(col.ExcelColumn);
                }
                var iRowNo = 0;
                var dataError = ""; //数据错误
                table.Columns.Add("temp_row_id");
                var rowEnumerator = sheet.GetRowEnumerator();
                rowEnumerator.MoveNext(); //跳过第一行列名

                while (rowEnumerator.MoveNext())
                {
                    bool emptyRow = true;
                    iRowNo++;
                    var cur = (IRow) rowEnumerator.Current;
                    var row = table.NewRow();
                    foreach (var aColumnStructure in aSheetStructure._ColumnStructures)
                    {                     
                        string value = null;
                        var cell = cur.GetCell(aColumnStructure.ColumnIndex);
                        var cellTypeText = string.Empty;
                        try
                        {
                            if (cell != null && cell.ToString().Trim() != String.Empty) //不为空时，按照规则读取数据
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.String:
                                        value = cell.StringCellValue.Trim();
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.String:
                                                value = cell.StringCellValue.Trim();
                                                break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    cellTypeText = "日期";
                                                    value = cell.DateCellValue.ToString();
                                                }
                                                else
                                                {
                                                    cellTypeText = "数字";
                                                    value = cell.NumericCellValue.ToString();
                                                }
                                                break;
                                            default:
                                                value = cell.ToString().Trim();
                                                break;
                                        }

                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            value = cell.DateCellValue.ToString();
                                            break;
                                        }
                                        // 若是自定义时间格式，仍无法判断
                                        value = cell.ToString().Trim();
                                        break;
                                    default:
                                        value = cell.ToString().Trim();
                                        break;
                                }
                            }

                        }
                        catch (Exception )
                        {
                            dataError = string.Format("----[读值失败]：尝试读取第{0}行 【{1}】的值失败，请手动将该单元格格式为“{2}”！\r\n", iRowNo,
                                aColumnStructure.ExcelColumn,
                                cellTypeText);
                            if (!_errorListColumn.Keys.Contains("数据错误"))
                                _errorListColumn.Add("数据错误", new List<string>());
                            tmpList = _errorListColumn["数据错误"];
                            tmpList.Add(dataError);
                        }
                        if (!string.IsNullOrEmpty(value))
                        {
                            emptyRow = false;
                        }
                        if (value != null) {
                            value = ToDBC(value);
                        }
                        row[aColumnStructure.ColumnIndex] = value; //单元格赋值                      
                    }
                    //跳过空行
                    if (emptyRow)
                    {
                        iRowNo--;
                        continue;                        
                    }
                    //插入行
                    row["temp_row_id"] = iRowNo;
                    table.Rows.Add(row);
                }
                if (!string.IsNullOrEmpty(dataError))
                {                   
                    return false;
                }
                Table = table;
            }
            catch (Exception ex)
            {
                if (!_errorListExcelStructure.Keys.Contains("解析数据出现错误"))
                    _errorListExcelStructure.Add("解析数据出现错误",new List<string>());
                tmpList = _errorListExcelStructure["解析数据出现错误"];
                tmpList.Add(ex.Message);
    
                Table = null;
            }

            return Table != null;
        }


        /// <summary>
        /// 检查数据是否满足要求
        /// </summary>
        /// <returns></returns>
        private bool CheckDataTable()
        {
            try
            {
                #region //列检查

                var aTableRowList = Table.Select().ToList();
                var aSheetRule = _templateRule.SheetRule(Table.TableName);
                var aSheetStructure = _excelStructure.SheetStructure(Table.TableName);
                //var colErrorMsg = "";
                foreach (var aColumnStructure in aSheetStructure._ColumnStructures)
                {
                    if (aColumnStructure == null)
                        continue;

                    var aColumnRule = aSheetRule.ColumnRule(aColumnStructure.ExcelColumn);
                    if (aColumnRule == null)
                        continue;

                    IList<string> columnValues;
                    int iRowNum;
                    var colKey = String.Format("[{0}]表.[{1}]列", aSheetStructure.ExcelSheetName,
                       aColumnStructure.ExcelColumn);
                    #region //不可为空检查

                   
                    if (aColumnRule.CheckNull == 1)
                    {
                        iRowNum = Table.Select(string.Format("[{0}] is null", aColumnStructure.ExcelColumn)).Count();
                        if (iRowNum > 0)
                        {
                            if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                            {
                                _errorListDataColumnCheck.Add(colKey,new List<string>());
                            }
                            tmpList = _errorListDataColumnCheck[colKey];
                            tmpList.Add(string.Format("----[{0}]不通过：该列为必填字段，有{1}值为空！", "不可为空检查", iRowNum));
                        }
                    }

                    #endregion

                    #region //"字符串"aColumnRule.MeanDataType == "字符串"


                    if (_fromString.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        var charAllowLength = (int) (aColumnRule.CharFieldLengh/2);//NOTE:不区分中英文编码.直接使用字符串长度比较
                        iRowNum =
                            aTableRowList.Count(
                                s =>
                                    s[aColumnRule.ColumnName].ToString().Length >
                                    charAllowLength); 
                        if (iRowNum > 0)
                        {
                            if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                            {
                                _errorListDataColumnCheck.Add(colKey, new List<string>());
                            }
                            
                            tmpList = _errorListDataColumnCheck[colKey];
                            tmpList.Add(string.Format("----[{0}]不通过：该列数据长度应当小于等于{1}，共有{2}个值长度超过{3}！",
                                "字符串长度检测",
                                charAllowLength, iRowNum, charAllowLength));
                        }
                    }

                    #endregion

                    #region //"单字典型"

                    if (_fromSingleDic.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        columnValues =
                            Table.Select(String.Format("[{0}] is not null ", aColumnStructure.ExcelColumn)).ToList()
                                .Select(p => p[aColumnStructure.ColumnIndex].ToString())
                                .Distinct().ToList();
                        foreach (var aParName in columnValues)
                        {
                            if (aColumnRule.DicList != null && aColumnRule.DicList.ContainsKey(aParName))
                            {
                                //将dic_par_id替换成dic_par_name
                                var aDic = aColumnRule.DicList[aParName];
                                //if (aDic != null)
                                //{
                                //    foreach (var aRow in
                                //        Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                                //            aParName)))
                                //    {
                                //        aRow[aColumnRule.ColumnName] = aDic.DIC_PAR_ID.ToString();
                                //    }
                                //}
                                //else
                                if(aDic==null){
                                    iRowNum =
                                        Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                                            aParName)).Count();

                                    if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                    {
                                        string checkFromDic = null;
                                        if (aColumnRule.DicList != null && aColumnRule.DicList.Any())
                                        {
                                            foreach (var aDicTemp in aColumnRule.DicList)
                                            {
                                                if (string.IsNullOrEmpty(checkFromDic))
                                                {
                                                    if (string.IsNullOrEmpty(checkFromDic))
                                                        checkFromDic = "*----[注]可选填内容为：" + "“" + aDicTemp.Key +
                                                                       "”";
                                                    else
                                                        checkFromDic = checkFromDic + "," + "“" + aDicTemp.Key +
                                                                       "”";
                                                }
                                                checkFromDic = checkFromDic + "；";
                                            }
                                        }
                                        _errorListDataColumnCheck.Add(colKey, new List<string>());
                                        _errorListDataColumnCheck[colKey].Add(checkFromDic);
                                    }
                                    tmpList = _errorListDataColumnCheck[colKey];
                                    tmpList.Add(string.Format("----[{0}]不通过：“{1}”对应字典项出现问题，共有{2}为该值，请联系管理员！"
                                        , "字典项检测"
                                        , aParName
                                        , iRowNum));
                                }
                            }
                            else //字典不存在
                            {
                                iRowNum = Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                                    aParName)).Count();                               
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”不是有效的可选内容，共有{2}处为该值！", "字典项检测", aParName, iRowNum));                               
                            }
                        }
                    }

                    #endregion

                    #region //"多字典型"

                    if (_fromMultiDic.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        columnValues =
                            Table.Select(String.Format("[{0}] is not null ", aColumnStructure.ExcelColumn)).ToList()
                                .Select(p => p[aColumnStructure.ColumnIndex].ToString())
                                .Distinct().ToList();
                        foreach (var columnValue in columnValues)
                        {
                            string aParNameValue = null;
                            var aParNameList = columnValue.Replace("，", ",").Replace("、", ",").Split(',').Distinct()
                                .ToList();
                            aParNameList.RemoveAll(f => string.IsNullOrEmpty(f.Trim()));
                            //遍历产生字典值
                            foreach (var aParName in aParNameList)
                            {
                                if (aColumnRule.DicList != null &&
                                    aColumnRule.DicList.ContainsKey(aParName))
                                {
                                    var aDic = aColumnRule.DicList[aParName];
                                    if (aDic != null)
                                    {
                                        if (aParNameValue == null) aParNameValue = aDic;
                                        else aParNameValue = aParNameValue + "," + aDic;
                                    }
                                    else
                                    {
                                       iRowNum =
                                       Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                                           aParName)).Count();

                                        if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                        {
                                            string checkFromDic = null;
                                            if (aColumnRule.DicList != null && aColumnRule.DicList.Any())
                                            {
                                                foreach (var aDicTemp in aColumnRule.DicList)
                                                {
                                                    if (string.IsNullOrEmpty(checkFromDic))
                                                    {
                                                        if (string.IsNullOrEmpty(checkFromDic))
                                                            checkFromDic = "*----[注]可选填内容为：" + "“" + aDicTemp.Key +
                                                                           "”";
                                                        else
                                                            checkFromDic = checkFromDic + "," + "“" + aDicTemp.Key +
                                                                           "”";
                                                    }
                                                    checkFromDic = checkFromDic + "；";
                                                }
                                            }
                                            _errorListDataColumnCheck.Add(colKey, new List<string>());
                                            _errorListDataColumnCheck[colKey].Add(checkFromDic);
                                        }
                                        tmpList = _errorListDataColumnCheck[colKey];
                                        tmpList.Add(string.Format("----[{0}]不通过：“{1}”对应字典项出现问题，共有{2}为该值，请联系管理员！"
                                            , "字典项检测"
                                            , aParName
                                            , iRowNum));
                                    }
                                }
                                else //字典项不存在
                                {
                                    iRowNum =
                                        Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                                            aParName)).Count();
                                    if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                    {
                                        _errorListDataColumnCheck.Add(colKey, new List<string>());
                                    }
                                    tmpList = _errorListDataColumnCheck[colKey];
                                    tmpList.Add(string.Format("----[{0}]不通过：“{1}”对应字典项出现问题，共有{2}为该值，请联系管理员！"
                                        , "字典项检测"
                                        , aParName
                                        , iRowNum));
                                }
                            }
                            //修改表格中的字典值
                            //if (aParNameValue != null)
                            //{
                            //    foreach (
                            //        var aRow in
                            //            Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                            //                columnValue)))
                            //    {
                            //        aRow[aColumnRule.ColumnName] = aParNameValue;
                            //    }
                            //}
                        }

                    }

                    #endregion

                    #region //"小数型";"十进制数值";默认两位小数

                    if (_fromDouble.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        columnValues =
                            Table.Select(string.Format("[{0}] is not null ", aColumnStructure.ExcelColumn)).ToList()
                                .Select(p => p[aColumnStructure.ColumnIndex].ToString())
                                .Distinct().ToList();
                        foreach (var columnValue in columnValues)
                        {
                            decimal? aDecimalVal = null;
                            //if (columnValue.EndsWith("%"))
                            //{
                            //    var valStr = columnValue.Substring(0, columnValue.Length - 1);
                            //    decimal val;
                            //    if (decimal.TryParse(valStr, out val))
                            //        aDecimalVal = val/100;
                            //}
                            //else
                            //{
                                decimal val;
                                if (decimal.TryParse(columnValue, out val))
                                    aDecimalVal = val;
                            //}
                            //检查精度
                            if (aDecimalVal.HasValue && aColumnRule.Field_Scale.HasValue)
                                aDecimalVal = Math.Round(aDecimalVal.Value, (int) aColumnRule.Field_Scale.Value);

                            iRowNum =
                                Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn, columnValue))
                                    .Count();

                            if (!aDecimalVal.HasValue)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”不能转化为{2}位小数，共有{3}处为该值！"
                                    , "小数型检测"
                                    , columnValue
                                    , aColumnRule.Field_Scale
                                    , iRowNum
                                    ));                               
                            }
                            else if (aColumnRule.DecimalMax.HasValue && aDecimalVal > aColumnRule.DecimalMax)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须小于{2}，共有{3}处为该值！"
                                    , "小数范围检测"
                                    , columnValue
                                    , aColumnRule.DecimalMax
                                    , iRowNum
                                    ));                                 
                                
                            }
                            else if (aColumnRule.DecimalMin.HasValue && aDecimalVal < aColumnRule.DecimalMin)
                            {
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须大于{2}，共有{3}处为该值！"
                                    , "小数范围检测"
                                    , columnValue
                                    , aColumnRule.DecimalMin
                                    , iRowNum
                                    )); 
                            }
                            //else
                            //{
                            //    if (columnValue.EndsWith("%"))
                            //    {
                            //        foreach (
                            //            var aRow in
                            //                Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                            //                    columnValue)))
                            //        {
                            //            aRow[aColumnRule.ColumnName] = aDecimalVal.Value;
                            //        }
                            //    }
                            //}
                        }
                    }

                    #endregion

                    #region  //"整数型"

                    if (_fromInt.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        columnValues =
                            Table.Select(String.Format("[{0}] is not null ", aColumnStructure.ExcelColumn)).ToList()
                                .Select(p => p[aColumnStructure.ColumnIndex].ToString())
                                .Distinct().ToList();

                        foreach (var columnValue in columnValues)
                        {
                            int? aIntVal = null;
                            int val;
                            if (int.TryParse(columnValue, out val))
                            {
                                aIntVal = val;
                            }

                            iRowNum =
                                Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn, columnValue))
                                    .Count();

                            if (!aIntVal.HasValue)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”不能转化为整数，共有{2}为该值！",
                                    "整数型检测", columnValue,
                                    iRowNum)); 
                            }
                            else if (aColumnRule.DecimalMax.HasValue && aIntVal > aColumnRule.DecimalMax)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须小于{2}，共有{3}为该值！"
                                    , "整数范围检测"
                                    , columnValue
                                    , aColumnRule.DecimalMax
                                    , iRowNum)); 
                            }
                            else if (aColumnRule.DecimalMax.HasValue && aIntVal > aColumnRule.DecimalMax)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须大于{2}，共有{3}为该值！"
                                    , "整数范围检测"
                                    , columnValue
                                    , aColumnRule.DecimalMin
                                    , iRowNum)); 
                            }
                        }
                    }

                    #endregion

                    #region //"日期型" 2005-11-5

                    if (_fromData.Contains(aColumnRule.MeanDataType.ToLower()))
                    {
                        columnValues =
                            Table.Select(String.Format("[{0}] is not null ", aColumnStructure.ExcelColumn)).ToList()
                                .Select(p => p[aColumnStructure.ColumnIndex].ToString())
                                .Distinct().ToList();

                        foreach (var columnValue in columnValues)
                        {
                            DateTime? aDateTime = null;
                            DateTime val;
                            if (DateTime.TryParse(columnValue, out val))
                            {
                                aDateTime = val;
                            }

                            
                            if (!aDateTime.HasValue)
                            {
                                if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                                {
                                    _errorListDataColumnCheck.Add(colKey, new List<string>());
                                }
                                iRowNum =
                                Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn, columnValue))
                                    .Count();
                                tmpList = _errorListDataColumnCheck[colKey];
                                tmpList.Add(string.Format(
                                    "----[{0}]不通过，“{1}”不能转化为日期型，共有{2}处为该值(若无法查看错误位置，请在excel中将此列格式转换成常规模式再查看)！"
                                    , "日期型检测",
                                    columnValue, iRowNum)); 
                            }
                            //else if (aColumnRule.DateTimeMax != null && aDateTime > aColumnRule.DateTimeMax)
                            //{
                            //    if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                            //    {
                            //        _errorListDataColumnCheck.Add(colKey, new List<string>());
                            //    }
                            //    tmpList = _errorListDataColumnCheck[colKey];
                            //    tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须小于{2}，共有{3}为该值！"
                            //        , "日期型检测"
                            //        , columnValue
                            //        , aColumnRule.DecimalMax
                            //        , iRowNum
                            //        )); 
                            //}
                            //else if (aColumnRule.DateTimeMin != null && aDateTime < aColumnRule.DateTimeMin)
                            //{
                            //    if (!_errorListDataColumnCheck.Keys.Contains(colKey))
                            //    {
                            //        _errorListDataColumnCheck.Add(colKey, new List<string>());
                            //    }
                            //    tmpList = _errorListDataColumnCheck[colKey];
                            //    tmpList.Add(string.Format("----[{0}]不通过，“{1}”必须小于{2}，共有{3}为该值！"
                            //        , "日期型检测"
                            //        , columnValue
                            //        , aColumnRule.DecimalMax
                            //        , iRowNum
                            //        )); 
                            //}
                            //else
                            //{
                            //    foreach (
                            //        var aRow in
                            //            Table.Select(String.Format("[{0}] = '{1}' ", aColumnStructure.ExcelColumn,
                            //                columnValue)))
                            //    {
                            //        //aRow[aColumnRule.ColumnName] = aDateTime.Value.ToShortDateString();
                            //    }
                            //}

                        }

                    }

                    #endregion
                }

                if (_errorListDataColumnCheck!=null && _errorListDataColumnCheck.Any())
                {                    
                    return false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (!_errorListDataColumnCheck.Keys.Contains("检查数据出现错误"))
                {
                    _errorListDataColumnCheck.Add("检查数据出现错误", new List<string>());
                }
                tmpList = _errorListDataColumnCheck["检查数据出现错误"];
                tmpList.Add(string.Format("----[错误信息]：{0} \r\n", ex.Message));
       
                Table = null;
            }
            return Table != null;
        }


        /// <summary>
        /// 数据唯一性检测
        /// </summary>
        /// <returns></returns>
        private bool PrimaryCheck()
        {
            List<List<string>> valueList = new List<List<string>>();
            List<string> nameList = new List<string>();
            List<string> errorList = new List<string>();
            var thisMaxTempRowID = 0; //
            int aTableOnceRowNum = 1 * 10000;
            var aTableRowList = Table.Select().ToList();
            var aSheetRule = _templateRule.SheetRule(Table.TableName);
            var aSheetStructure = _excelStructure.SheetStructure(Table.TableName);
            while (aTableRowList.Any())
            {
                thisMaxTempRowID = thisMaxTempRowID + aTableOnceRowNum;
                foreach (var aColumnRule in _templateRule.SheetRule(Table.TableName).TemplateSheetDbTable.ColumnRuleList)
                {
                    if (aColumnRule.IsPk == 1)
                    {
                        var columnArray =
                            aTableRowList.Where(f => Convert.ToInt32(f["temp_row_id"]) <= thisMaxTempRowID)
                                .Select(f => f[aColumnRule.ColumnName].ToString())
                                .ToList();
                        valueList.Add(columnArray);
                        nameList.Add(aColumnRule.ColumnName);
                    }
                }
                aTableRowList.RemoveAll(f => Convert.ToInt32(f["temp_row_id"]) <= thisMaxTempRowID); //移除其已经插入的数据 
            }
            //检查excel数据是否违法唯一性约束
            if (valueList.Count > 0)
            {
                for (int i = 0; i < valueList.Count; i++)
                {
                    var duplicates = valueList[i].GroupBy(l => l)
                        .Where(g => g.Count() > 1)
                        .Select(o => o.Key)
                        .ToList();
                    if (duplicates.Count > 0)
                    {
                        string values = "[" + string.Join("、", duplicates.ToArray()) + "]";
                        var colKey = String.Format("[{0}]表.[{1}]列", aSheetStructure.ExcelSheetName,
                       nameList[i]);
                        if (!_errorListPrimaryCheck.Keys.Contains(colKey))
                        {
                            _errorListPrimaryCheck.Add(colKey, new List<string>());
                        }
                        tmpList = _errorListPrimaryCheck[colKey];
                        tmpList.Add(string.Format("----[{0}]不通过：该列为唯一性字段，有{1}值{2}重复！", "唯一性检查", duplicates.Count,values));
                       
                    }
                }
                if (_errorListPrimaryCheck != null && _errorListPrimaryCheck.Any())
                {
                    return false;
                } 
            }
            return true;
        }

        /// <summary>
        /// 添加错误检查
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorList"></param>
        private void AddCheckError(string errorType, Dictionary<string, List<string>> errorList)
        {
            var aError = new ExcelCheckError()
            {
                ErrorType = errorType,
                Errorlist = errorList
            };
            ErrorList.Add(aError);
        }

        private List<string> GetExcelTablesName()
        {
            var list = new List<string>();
            foreach (ISheet sheet in _workbook)
            {
                list.Add(sheet.SheetName);
            }
            return list;
        }

        private string ConvertListToString(List<string> propertyList, string delimiter = "$$$")
        {
            string result = null;
            if (propertyList != null && propertyList.Any())
            {
                foreach (var a in propertyList)
                {
                    if (!string.IsNullOrEmpty(a))
                        result = result + a + delimiter;
                }
            }
            if (result != null)
            {
                result = result.Substring(0, result.Length - delimiter.Length);
            }
            return result;
        }


        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public byte[] TableToExcel(DataTable dt)
        {

            IWorkbook workbook = new HSSFWorkbook();

            int sheetCount = 1;
            //if(dt.Rows.Count>0)
            //{
            sheetCount = (dt.Rows.Count / 65530) + 1;
            //}

            for (int k = 0; k < sheetCount; k++)
            {
                ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet" + (k + 1).ToString()) : workbook.CreateSheet(dt.TableName);

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


        /// <summary>
        /// 进度改变事件
        /// </summary>
        /// <param name="stepCount"></param>
        private void ProgressChangeEvent(int stepCount)
        {
            if (CheckProgressChanged == null)
            {
                return;
            }
            CheckProgressChanged(stepCount);
        }


        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        // /任意字符串
        // /半角字符串
        // /
        // /全角空格为12288，半角空格为32
        // /其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        // /
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
    }
}
