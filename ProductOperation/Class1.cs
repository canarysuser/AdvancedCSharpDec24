using MEFInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ProductOperation
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypes.Product)]
    public class Products : IOperation
    {
        public string GetData()
        {
            return "Product Operations goes here.";
        }
    }
}
