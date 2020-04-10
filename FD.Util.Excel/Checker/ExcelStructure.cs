using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{  
    //excel整体结构
    public class ExcelStructure
    {

        public bool SheetContains(string sheetName)
        {
            bool result = false;
            if (_SheetStructures != null && _SheetStructures.Any())
            {
                if (_SheetStructures.Any(s => s.ExcelSheetName == sheetName))
                {
                    result = true;
                }
            }
            return result;
        }

        public int SheetNum
        {
            get
            {
                int num = 0;
                if (_SheetStructures != null)
                {
                    num = _SheetStructures.Count();
                }
                return num;
            }
        }

        public ExcelSheetStructure SheetStructure(int index)
        {
            ExcelSheetStructure aSheetStructure = null; ;
            if (_SheetStructures != null && _SheetStructures.Count() > index)
            {
                aSheetStructure = _SheetStructures[index];
            }
            return aSheetStructure;
        }

        public ExcelSheetStructure SheetStructure(string sheetName)
        {
            ExcelSheetStructure aSheetStructure = null;
            if (_SheetStructures != null && _SheetStructures.Any())
            {
                if (_SheetStructures.Any(s => s.ExcelSheetName == sheetName))
                {
                    aSheetStructure = _SheetStructures.Single(s => s.ExcelSheetName == sheetName);
                }
            }
            return aSheetStructure;
        }

        public List<ExcelSheetStructure> _SheetStructures = new List<ExcelSheetStructure>();
    }
    //excel工作表结构
  

   
}
