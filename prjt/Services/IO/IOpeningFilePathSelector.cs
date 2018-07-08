using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjt.Services.IO
{
    public interface IOpeningFilePathSelector
    {
        string GetFilePath(string defaultFilePath, Action<object> modifier);
    }
}
