using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Services.Pdf
{
    public interface IListingReportGenerator
    {
        void Save(string filePath, Document document);
    }
}
