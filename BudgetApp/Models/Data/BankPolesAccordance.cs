using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.Data
{
    public class BankPolesAccordance
    {
        public int Id { get; set; }
        public Bank Bank { get; set; }
        public List<PoleAccordance> PoleAccordances { get; set; }
    }
}
