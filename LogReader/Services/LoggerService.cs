using LogReader.DTO;
using LogReader.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace LogReader.Services
{
    public class LoggerService : ILoggerService
    {
        volatile bool m_IsReading;
        private string _location;
        private int _lastPosition = 0;

        public LoggerService(string location)
        {
            _location = location;
            //Check is the run is from 
            string lineNumber = LocalDB.GetValue(_location);

            if (lineNumber== null)
            {
                _lastPosition = 0;
            }
            else
            {
                _lastPosition =  int.Parse(lineNumber);
            }

        }

        public async Task<List<LogInfo>> ReadLog()
        {
            if (m_IsReading) return null;

            List<LogInfo> info  = null;

            m_IsReading = true;
            try
            {
                FileInfo m_FileInfo = new FileInfo(_location);

                using (StreamReader reader = m_FileInfo.OpenText())
                {
                    reader.BaseStream.Position = _lastPosition;

                    string line = string.Empty;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty( line))  continue;

                        if (info == null) 
                            info = new List<LogInfo>();

                        info.Add(ConvertStringToLogFormat(line));
                    }
                    _lastPosition = (int)reader.BaseStream.Position;

                }
            }
            catch (Exception)
            {
                //Save To Local DB for Next run
                LocalDB.SetValue(_location, _lastPosition.ToString());
                throw; 
            }
           
            m_IsReading = false;
            return info;
        }

        public async Task WriteLog(LogInfo item)
        {
            using (TextWriter tw = new StreamWriter(_location, true))
            {
                string line = ConvertLogToStringFormat(item);

                if (line != null)
                {
                    tw.WriteLine(line);
                }
            }
        }

        private LogInfo ConvertStringToLogFormat(string log)
        {
            try
            {
                LogInfo info = new LogInfo();

                string[] data = log.Split(' ');

                info.Date = DateTime.ParseExact(data[0], "HH:mm:ss.fff", new CultureInfo("en-US"));

                info.Message = data[1];

                return info;

            }
            catch (Exception)
            {
                return null;
            }
          
        }

        private string ConvertLogToStringFormat(LogInfo item)
        {
            try
            {
                return $"{item.Date.ToString("HH:mm:ss.fff")} {item.Message}";

            }
            catch (Exception)
            {
                return null;
            }
          
        }


    }
}
