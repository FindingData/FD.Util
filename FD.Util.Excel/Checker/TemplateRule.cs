using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class TemplateRule
    {       
        //模板名称
        public string TemplateName;
        
        //模版规则
        public TemplateRule(string templateName)
        {
            this.TemplateName = templateName;
        }

        public TemplateSheetRule TemplateSheetRule = new TemplateSheetRule();

        //以下为方法
        public bool SheetContains(string sheetName)
        {
            bool result = false;
            if (TemplateSheetRule != null)
            {
                if (TemplateSheetRule.SheetName.Equals(sheetName))
                {
                    result = true;
                }
            }
            return result;
        }

        public TemplateSheetRule SheetRule(string sheetName)
        {
            TemplateSheetRule aSheetRule = null;
            if (TemplateSheetRule != null)
            {
                //if (TemplateSheetRule.SheetName.Equals(sheetName))
                //{
                    aSheetRule = TemplateSheetRule;
                //}
            }
            return aSheetRule;
        }

        public bool IsEveryColumnCorred()
        {
            return TemplateSheetRule.IsEveryColumnCorred();
        }
    }
}
