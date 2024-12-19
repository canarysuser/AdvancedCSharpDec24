using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFInterfaces
{
    [Export(typeof(IDataRetriever))]
    public class DataRetriever : IDataRetriever
    {
        [ImportMany(typeof(IOperation))]
        public IEnumerable<Lazy<IOperation, IOperationMetadata>> operations;

        public string GetData(OperationTypes opType)
        {
            foreach(var item in operations)
            {
                if(item.Metadata.OperationType==opType)
                    return item.Value.GetData();
            }
            if (opType == OperationTypes.None)
                return $"Operation Type {opType} on DataRetriever class is executed.";
            return "could not find any matching parts";
        }
    }
}
