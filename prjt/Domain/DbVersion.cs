using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    /// <summary>
    /// Contains version of database
    /// </summary>
    public class DbVersion
    {
        public const string UNIQUE_KEY = "db_version";


        private string _id;
        public string ID
        {
            get { return _id; }
        }


        private readonly string _version;
        public string Version
        {
            get { return _version; }
        }


        public DbVersion(string id, string version)
        {
            _id = id;
            _version = version;
        }
    }
}
