using BudgetApp.Models.Utilitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Services
{
    public static class PolesDescriber
    {
        public static Dictionary<PropertyInfo,string> GetPolesName(Type type)
        {
            Dictionary<PropertyInfo, string> names = new Dictionary<PropertyInfo, string>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                //var attrs = prop.GetCustomAttributes(true);
                var paramNameAttribute = prop.GetCustomAttribute(typeof(PoleDescriptionAttribute)) as PoleDescriptionAttribute;
                if (paramNameAttribute != null && paramNameAttribute.IsEditable == true)
                {
                    names.Add(prop, paramNameAttribute.Name);
                }
                /*
                string paramNameValue = paramNameAttribute is null ? prop.Name : paramNameAttribute.Name;
                string propName = prop.Name;
                */
            }
            return names;
        }
    }
}
