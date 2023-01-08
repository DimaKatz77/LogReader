using LogReader.DTO;
using LogReader.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogReader.Services
{
    public class MainService : IMainService
    {
        private readonly IConfigurationRoot _configurationBuilder;
        private readonly IDBService _dBService;

       Dictionary<string, string> _filesPath;
        int position = 0;


        public MainService(
            IConfigurationRoot configurationBuilder, 
            IDBService dBService) 
        {
            _configurationBuilder = configurationBuilder;
            _filesPath = _configurationBuilder.GetChildren().Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
            _dBService = dBService;
        }

        public async Task Start() {

            //We can Use - (  task.whenall )or ( Linq with anonimous delegats) instead Parallel.ForEach

            Parallel.ForEach(_filesPath, async path =>
            {
                await ExecuteAsync(path);
            });

        }

        private  async Task ExecuteAsync(KeyValuePair<string, string> path)
        {
            DateTime today = DateTime.Today;

            string fileName = $"{path.Value}/{today.ToString("MM_dd_yyyy")}.log";

            LoggerService read = new LoggerService(fileName);

            //I can use FileSystemWatcher Solution instead (while).  I add file with this solution to main Zip
            while (DateTime.Today == today)
            {
                List<LogInfo> items = await read.ReadLog();

                foreach (var item in items)
                {
                    try
                    {
                        _dBService.CreateLogDBRecord(item);
                    }
                    catch (Exception)
                    {
                        await read.WriteLog(item);
                    }
                }

            }
        }
    }
}
