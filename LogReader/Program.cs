using LogReader.Interface;
using LogReader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace LogReader
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            //Get the File 
            var localConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IMainService, MainService>()
                .AddSingleton<IDBService, DBService>()
                .AddSingleton(localConfig)
                .BuildServiceProvider();


            //do the actual work here
            var bar = serviceProvider.GetService<IMainService>();
            await bar.Start();


        }


      
    }
}
