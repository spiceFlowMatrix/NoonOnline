using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ErpAccountsBusiness
    {
        //this used for erp
        private readonly EFErpAccountsRepository _EFErpAccountsRepository;
        public ErpAccountsBusiness(
            EFErpAccountsRepository EFErpAccountsRepository
        )
        {
            _EFErpAccountsRepository = EFErpAccountsRepository;
        }

        public ERPAccounts GetBytypeId(long type)
        {
            return _EFErpAccountsRepository.GetById(b => b.Type == type && b.DeletionTime == null);
        }

        public List<ERPAccounts> GetAll()
        {
            return _EFErpAccountsRepository.GetAll();
        }

        public ERPAccounts UpdateAccounts(ERPAccounts obj)
        {
            var erpac = GetBytypeId(obj.Type);
            try
            {
                if (erpac != null)
                {
                    erpac.Type = obj.Type;
                    erpac.AccountCode = obj.AccountCode;
                    erpac.LastModificationTime = obj.LastModificationTime;
                    erpac.LastModifierUserId = obj.LastModifierUserId;
                    _EFErpAccountsRepository.Update(erpac);
                    WriteLog("Erp Account Saved" + "===" + erpac.Type + "|" + erpac.AccountCode);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return erpac;
        }

        public static void WriteLog(string strLog)
        {
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs/" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(strLog);
                }
            }
        }
    }
}
