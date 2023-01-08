using LogReader.DTO;
using LogReader.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogReader.Services
{
    public class DBService: IDBService
    {
        public DBService() { }
        public void CreateLogDBRecord(LogInfo item)
        {
            throw new Exception("Not implemented");
        }
    }
}
