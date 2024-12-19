using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFInterfaces
{
    public enum OperationTypes { None=0, Account=1, Product=2  };
    public interface IDataRetriever
    {
        string GetData(OperationTypes opType);
    }
    public interface IOperation
    {
        string GetData(); 
    }
    public interface IOperationMetadata
    {
        OperationTypes OperationType { get; }
    }
}
