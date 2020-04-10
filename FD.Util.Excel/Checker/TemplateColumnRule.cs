using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class TemplateColumnRule
    {
        /// <summary>
        /// (Excel)sheet页中的列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// 关联到的DB_Field的ID
        /// </summary>
        public decimal? CorrelationID { get; set; }


        /// <summary>
        /// 是否主键
        /// </summary>
        public decimal? IsPk { get; set; }

        /// <summary>
        /// 关联到的DB_Field的
        /// </summary>
        public string FsqField { get; set; }

        /// <summary>
        /// 字段的中文名称
        /// </summary>
        public string FsqField_chs { get; set; }

        /// <summary>
        /// 是否筛选字段
        /// </summary>
        public decimal? IsFilter { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public decimal? IsDisable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public decimal? Sort { get; set; }
        //字段更新类型:0:不更新,1更新字段名,2新增,3类型更改,4text长度修改,5数字精度修改
        public decimal? updateType { get; set; }
        //下面的列值从数据库中读值
        #region --检查Excel的列值检查时需要使用

        /// <summary>
        /// 数据库中 CHECK_TYPE字段
        /// 列数据类型：
        ///     按照ExcelDataType分类：String；Number；Datetime；RichString
        ///         1、字符串；单字典型；Bool值；
        ///         2、整数型；十进制数值；
        ///         3、时间型；日期型；
        ///         4、带格式字符串 ==未处理
        ///     1、单字典型             ：DirTypeID
        ///     5、Bool型（有无、是否） ：最终转化为1、0
        ///     2、数值型-整数-百分数-比例：最大值；最小值
        ///     3、日期型/时间型        ：最大值；最小值yyyy-mm-dd;hh:mm:ss
        ///     4、字符型               ：最大长度
        /// </summary> 
        public string MeanDataType { get; set; }

        /// <summary>
        /// 数据库中对应字段类型
        /// </summary>
        public string DBDataType { get; set; }              //FIELD_TYPE

        /// <summary>
        /// Excel读取值的类型
        /// </summary>
        public string ExcelDataType { get; set; }

        /// <summary>
        /// 字符串字段长度限制  ：最大长度
        /// </summary>
        public decimal? CharFieldLengh { get; set; }          //FIELD_LENGTH

        /// <summary>
        /// 字符串字段长度限制  ：最大长度
        /// </summary>
        public decimal? FieldScale { get; set; }            //精度

        /// <summary>
        /// 字典型字段 ： 字典类型ID
        /// </summary>
        public decimal? DirTypeID { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public decimal? Field_Scale { get; set; }

        /// <summary>
        /// 字典型字段 ： 字典类型Name
        /// </summary>
        public string DirTypeName { get; set; }

        /// <summary>
        /// 字典项可选择的项
        /// </summary>
        public Dictionary<string,string> DicList;

        /// <summary>
        /// 列值不可为空
        /// </summary>
        public decimal? CheckNull { get; set; }

        /// <summary>
        /// 数值型:最小值
        /// </summary>
        public decimal? DecimalMin { get; set; }

        /// <summary>
        /// 数值型:最大值
        /// </summary>
        public decimal? DecimalMax { get; set; }

        /// <summary>
        /// 日期型/时间型:最小值
        /// </summary>
        public DateTime? DateTimeMin { get; set; }

        /// <summary>
        /// 日期型/时间型:最大值
        /// </summary>
        public DateTime? DateTimeMax { get; set; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public decimal? DecimalLength { get; set; }

        #endregion --检查Excel的列值检查时需要使用

    }
}
