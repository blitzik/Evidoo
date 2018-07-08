using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjt.Services.IO
{
    public interface ISavingFilePathSelector
    {
        string GetFilePath(string defaultFilePath, Action<object> modifier);
    }
}
