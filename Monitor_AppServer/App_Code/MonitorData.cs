using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Monitor_AppServer.App_Code
{
    public class MonitorData
    {
        public string ServerName { get; set; }

        public string MemoryUsage { get; set; }

        public ConcurrentBag<MonitorData> GetData()
        {
            var list = new ConcurrentBag<MonitorData>();
          
            var connectionInfo = new PasswordConnectionInfo("192.168.163.128", "cloudera", "cloudera");
            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();
                var cmd = client.CreateCommand("ls -ltr");   //  very long list 
                var synch = cmd.Execute();
                Parallel.ForEach(synch.Split('\n'), (x) =>
                {
                    list.Add(new MonitorData { ServerName = "LocalHost", MemoryUsage = x });
                });                
                client.Disconnect();
            }  
            return list;
        }
    }
}
