using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Services.Entities
{
    public interface IListingFactory
    {
        Listing Create(int year, int month);
    }
}
