using BudgetApp.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.DTO
{
    public class PoleAccordanceInputDTO
    {
        public string PoleName { get; set; }
        public string PoleTitle { get; set; }
        public string ExcelColumnName { get; set; }
        public bool IsRequired { get; set; }


        public PoleAccordance CreatePoleAccordance()
        {
            return new PoleAccordance()
            {
                PoleName = PoleName,
                PoleTitle = PoleTitle,
                ExcelColumnName = ExcelColumnName,
            };
        }

    }
}
