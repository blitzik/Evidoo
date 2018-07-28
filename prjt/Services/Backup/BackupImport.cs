using prjt.Domain;
using Perst;
using System;
using System.IO;
using System.Reflection;
using Common.Utils.ResultObject;

namespace prjt.Services.Backup
{
    public class BackupImport : IBackupImport
    {
        private StoragePool _storagePool;
        private PerstStorageFactory _storageFactory;


        public BackupImport(StoragePool storagePool, PerstStorageFactory storageFactory)
        {
            _storagePool = storagePool;
            _storageFactory = storageFactory;
        }


        public ResultObject<object> Import(string importFilePath, string appDBDirectory, string activeDBName, string activeDBExtension)
        {
            ResultObject<object> ro;
            try {
                Storage importedDb = StorageFactory.Instance.CreateStorage();
                importedDb.Open(importFilePath, 4 * 1024 * 1024);
                importedDb.Close();

                ro = new ResultObject<object>(true);
                ro.AddMessage("Import dat proběhl úspěšně!", ResultObjectMessageSeverity.SUCCESS);

            } catch (StorageError e) {
                ro = new ResultObject<object>(false);
                ro.AddMessage("Ze zvoleného souboru nelze importovat data.", ResultObjectMessageSeverity.WARNING);

            } catch (Exception e) {
                ro = new ResultObject<object>(false);
                ro.AddMessage("Při importu dat došlo k chybě.");
            }

            if (!ro.Success) {
                return ro;
            }

            _storagePool.Close(PerstStorageFactory.MAIN_DATABASE_NAME);
            DateTime now = DateTime.Now;

            string activeDbFilePath = Path.Combine(appDBDirectory, activeDBName + "." + activeDBExtension);

            string oldDbBackupFileName = string.Format("backup_{0}_{1}_{2}_{3}_{4}_{5}_v{6}.{7}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "-"), activeDBExtension);
            string lastWorkingDbBackupPath = Path.Combine(appDBDirectory, oldDbBackupFileName);

            File.Move(activeDbFilePath, lastWorkingDbBackupPath);
            File.Copy(importFilePath, activeDbFilePath);

            _storagePool.Add(PerstStorageFactory.MAIN_DATABASE_NAME, _storageFactory.OpenConnection(PerstStorageFactory.MAIN_DATABASE_NAME));

            return ro;
        }
    }
}
