using MEFInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypes.Account)]
    public class AccountOperation : IOperation
    {
        public string GetData()
        {
            return "Account Details is returned from here.";
        }
    }
}
