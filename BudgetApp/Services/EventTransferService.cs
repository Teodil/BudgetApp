using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Services
{
    public class EventTransferService
    {

        public event Action<Type> DataUpdatedEvent;

        public event Action? DataBaseDataUpdatedEvent;

        public void DataBaseDataUpdated()
        {
            if(DataBaseDataUpdatedEvent != null)
            {
                DataBaseDataUpdatedEvent.Invoke();
            }
        }

        public void ChandedData<T>() where T : class
        {
            if(DataUpdatedEvent != null)
            {
                DataUpdatedEvent.Invoke(typeof(T));
            }
        }
    }
}
