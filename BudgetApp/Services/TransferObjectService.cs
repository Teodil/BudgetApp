using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Services
{
    public class TransferObjectService
    {
        private static TransferObjectService instance;
        private static object syncRoot = new Object();

        private object transferObject;

        private TransferObjectService()
        { }

        public static TransferObjectService getInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    instance = new TransferObjectService();
                }
            }
            return instance;
        }

        public void SetTransferObject(object transferObject)
        {
            this.transferObject = transferObject;
        }

        public object GetTransferObject()
        {
            return this.transferObject;
        }

    }
}
