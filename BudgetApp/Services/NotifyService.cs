using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Services
{
    public class NotifyService
    {
        public event Action DataParsedEvent;

        public void DataParsed()
        {
            if (DataParsedEvent != null)
            {
                DataParsedEvent.Invoke();
            }
        }
    }
}
