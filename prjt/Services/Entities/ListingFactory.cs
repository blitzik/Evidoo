using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.Domain;
using Perst;

namespace prjt.Services.Entities
{
    public class ListingFactory : IListingFactory
    {
        private StoragePool _storagePool;


        public ListingFactory(StoragePool storagePool)
        {
            _storagePool = storagePool;
        }


        public Listing Create(int year, int month)
        {
            return new Listing(_storagePool.GetByName(PerstStorageFactory.MAIN_DATABASE_NAME), year, month);
        }
    }
}
