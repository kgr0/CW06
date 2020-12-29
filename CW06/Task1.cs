using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace CW06
{
    public class Task1
    {
        public void Start()
        {
           // Ping pingSender = new Ping();
           // PingOptions options = new PingOptions();

           // // Use the default Ttl value which is 128,
           // // but change the fragmentation behavior.
           // options.DontFragment = true;

           // // Create a buffer of 32 bytes of data to be transmitted.
           // string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
           // byte[] buffer = Encoding.ASCII.GetBytes(data);
           // int timeout = 120;
           // PingReply reply = pingSender.Send("ftp.au.debian.org", timeout, buffer, options);
           // if (reply.Status == IPStatus.Success)
           // {
           //     Console.WriteLine("Address: {0}", reply.Address.ToString());
           //     Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
           //     Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
           //     Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
           //     Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
           // }
           // else
           // {
           //     Console.WriteLine("no");
           // }
            
           // Parallel.For
           //(0  , 1000
           // , new ParallelOptions { MaxDegreeOfParallelism = 4 }
           // , (i) =>
           // {
           // }
           //);







            //string path = @"ping.txt";
            //using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            //{
            //    string line;
            //    while ((line = await sr.ReadLineAsync()) != null)
            //    {
            //        Console.WriteLine(line);
            //    }
            //}

            string path = @"ping.txt";
            List<string> list = new List<string>();
            int i = 0;

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    if (i != 0)
                    {
                        int index = line.LastIndexOf(';');
                        list.Add(line.Remove(0, index + 1));
                        //Console.WriteLine(line.Remove(0, index + 1));
                    }
                    i++;
                }
            }

            Stopwatch asParallel = new Stopwatch();
            asParallel.Start();
            AsParallelFunc(list);
            asParallel.Stop();
            Console.WriteLine($"AsParallel: {asParallel.ElapsedMilliseconds}");

            Console.WriteLine();

            Stopwatch task = new Stopwatch();
            task.Start();
            TaskFunc(list);
            task.Stop();
            Console.WriteLine($"Task: {task.ElapsedMilliseconds}");

            Console.WriteLine();
            
            Stopwatch sync = new Stopwatch();
            sync.Start();
            SyncFunc(list);
            sync.Stop();
            Console.WriteLine($"Synchronized: {sync.ElapsedMilliseconds}");

        }

        public void TaskFunc(List<string> list)
        {
            int i = 0;

            ParallelLoopResult result = Parallel.ForEach
           (list, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (server) =>
            {
                if (Func(server))
                    i++;
            });

            Console.WriteLine($"Number of availible servers : {i}");
        }

        public void SyncFunc(List<string> list)
        {
            int i = 0;
            int halfSize = list.Count / 4;

            Thread t1 = new Thread(() =>
            {
                for (int j = 0; j < halfSize / 2; j++)
                {
                    if (Func(list[j]))
                        i++;
                }
            });
            Thread t2 = new Thread(() =>
            {
                for (int j = halfSize / 2; j < halfSize; j++)
                {
                    if (Func(list[j]))
                        i++;
                }
            });
            Thread t3 = new Thread(() =>
            {
                for (int j = halfSize; j < halfSize + (halfSize / 2); j++)
                {
                    if (Func(list[j]))
                        i++;
                }
            });
            Thread t4 = new Thread(() =>
            {
                for (int j = halfSize + (halfSize / 2); j < list.Count; j++)
                {
                    if (Func(list[j]))
                        i++;
                }
            });

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            Console.WriteLine($"Number of availible servers : {i}");
        }

        public void AsParallelFunc(List<string> list)
        {
            int i = 0;

            (from server in list.AsParallel().WithDegreeOfParallelism(4)
             select Func(server)).ForAll((value) => { if (value) i++; });

            Console.WriteLine($"Number of availible servers : {i}");
        }

        public bool Func(string server)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;
            
            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(server, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                //Console.WriteLine("Address: {0}", reply.Address.ToString());
                //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                //Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);

                return true;
            }
            else
            {
              //  Console.WriteLine("no");

                return false;
            }
        }
    }
}
