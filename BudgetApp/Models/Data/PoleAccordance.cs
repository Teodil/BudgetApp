﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.Data
{
    public class PoleAccordance
    {
        public int Id { get; set; }
        public BankPolesAccordance BankPolesAccordance { get; set; }
        public string PoleName { get; set; }
        public string ExcelColumnName { get; set; }
        /*
        public Bank bank { get; set; }

        public string DatePoleName { get; set; }
        public string CardNumberPoleName { get; set;}
        public string DescriptionPoleName { get; set;}
        public string SummPoleName { get; set;}
        public string OperationCategoryPoleName { get; set;}
        public string OperationTypePoleName { get; set;}
        public string CashBackPoleName { get; set;}
        */
    }


   
}