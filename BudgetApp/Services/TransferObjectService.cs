using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Services
{
    public class TransferObjectService
    {

        private Dictionary<Type,object> _transferObjectsList = new Dictionary<Type, object>();

        public void SetTransferObject<T>(T value)
        {
            if(_transferObjectsList.ContainsKey(typeof(T)))
            {
                _transferObjectsList[typeof(T)] = value;
            }
            else
            {
                _transferObjectsList.Add(typeof(T), value);
            }
        }

        public T? GetTransferObject<T>()
        {
            object? value; 
            _transferObjectsList.TryGetValue(typeof(T),out value);
            if(value != null)
            {
                T outValue = (T)_transferObjectsList[typeof(T)];
                return (T)value;
            }

            return default;
        }

        public T? GetTransferObjectAndDelete<T>()
        {
            T? outObject = GetTransferObject<T>();
            if (outObject != null)
                Delete<T>();
            return outObject;
        }

        public void Delete<T>()
        {
            _transferObjectsList.Remove(typeof(T));
        }

    }
}
