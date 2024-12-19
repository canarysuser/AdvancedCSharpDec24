using MEFInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class MEFClient
    {

        [Import(typeof(IDataRetriever))]
        public IDataRetriever DataRetriever;

        private CompositionContainer _container;
        public MEFClient()
        {
            var catObj = new AggregateCatalog(); 
            catObj.Catalogs.Add(new AssemblyCatalog(typeof(IDataRetriever).Assembly)); //MEFInterfaces.dll 
            catObj.Catalogs.Add(new DirectoryCatalog(@"../../Extensions")); 
            _container = new CompositionContainer(catObj);
            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
