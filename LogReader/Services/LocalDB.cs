using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogReader.Services
{
    public static class LocalDB
    {
        public static string GetValue(string key)
        {
            lock (key)
            {
                string location = $"{System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString())}\\settings.json";
                string text = "";

                using (StreamReader r = new StreamReader(location))
                {
                    text = r.ReadToEnd();
                }

                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                if (dic.ContainsKey(text))
                {
                    return dic[key];
                }
                return null;
                
            }

        }

        public static void SetValue (string key, string value)
        {
            lock (key)
            {
                Dictionary<string, string> vars = new Dictionary<string, string>();
                string location = $"{System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString())}\\settings.json";
                string text = "";
                using (StreamReader r = new StreamReader(location))
                {
                    text = r.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(text))
                {
                    vars = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                }
                if (!vars.ContainsKey(key))
                {
                    vars.Add(key, value);
                    using (TextWriter tw = new StreamWriter(location))
                    {
                        string json = JsonConvert.SerializeObject(vars);
                        tw.Write(json);
                    }
                }
                else
                {
                    if (int.Parse(value) > int.Parse(vars[key]))
                    {
                        vars[key] = value;
                        using (TextWriter tw = new StreamWriter(location))
                        {
                            string json = JsonConvert.SerializeObject(vars);
                            tw.Write(json);
                        }
                    }
                }

            }

        }

    }
}
