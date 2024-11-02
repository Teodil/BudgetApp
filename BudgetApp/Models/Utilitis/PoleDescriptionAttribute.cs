using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.Utilitis
{
    public class PoleDescriptionAttribute : Attribute
    {
        public string Name { get; set; }

        public bool IsEditable { get; set; }

        public PoleDescriptionAttribute(string name,bool isEditable = true)
        {
            Name = name;
            IsEditable = isEditable;
        }
    }
}
