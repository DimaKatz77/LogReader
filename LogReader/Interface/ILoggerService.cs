﻿using LogReader.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogReader.Interface
{
    public interface ILoggerService
    {
        Task<List<LogInfo>> ReadLog();
        Task WriteLog(LogInfo item);

    }
}
