using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Services.Backup
{
    public interface IBackupImport
    {
        ResultObject Import(string importFilePath, string appDBDirectory, string activeDBName, string activeDBExtension);
    }
}
