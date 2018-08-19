using prjt.Domain;
using prjt.Services;
using System.Collections.Generic;
using System.Linq;

namespace prjt.Facades
{
    public class EmployerFacade : BaseFacade
    {
        public EmployerFacade(StoragePool db) : base (db)
        {
        }


        public Employer CreateEmployer(string name)
        {
            Employer e = new Employer(Storage(), name);

            Storage().Store(e);
            Root().Employers.Add(e);
            Storage().Commit();

            return e;
        }


        public void Update(Employer employer)
        {
            Storage().Modify(employer);
            Storage().Commit();
        }


        public void Delete(Employer employer)
        {
            Root().Employers.Remove(employer);
            foreach (Listing l in employer.Listings) {
                l.Employer = null;
                Storage().Modify(l);
            }
            Storage().Deallocate(employer);
            Storage().Commit();
        }


        public List<Employer> FindAllEmployers()
        {            
            return new List<Employer>(from Employer e in Root().Employers select e);
        }
    }
}
