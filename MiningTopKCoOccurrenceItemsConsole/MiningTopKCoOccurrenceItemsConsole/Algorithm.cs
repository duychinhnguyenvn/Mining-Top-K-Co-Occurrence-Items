using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Algorithm
    {
        public double nt(List<List<string>> db,List<string> p,int k)
        {
            Dictionary<string, int> co = new Dictionary<string, int>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var t in db)
            {
                if (Utils.IsSubSetOf (p,t))
                {                    
                    List<string> coList = Utils.GetCoItems(p, t);                    
                    foreach(var item in coList)
                    {
                        if (!co.ContainsKey(item)) {
                            co.Add(item, 1);
                        }
                        else
                        {
                            co[item] = co[item] + 1;
                        }
                    }
                }
            }
            var rTK = (from entry in co orderby entry.Value descending select entry).Take(k);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            // Display results.
            foreach (KeyValuePair<string, int> pair in rTK)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("Time taken: {0}ms", elapsedMs);
            return elapsedMs;
        }
        public double nti(List<List<string>> db, List<string> p, int k) {
            Dictionary<string,List<int>> TID_list = new Dictionary<string, List<int>>();
            int index=-1;
            foreach (var t in db)
            {
                index++;
                foreach (var i in t)
                {
                    if (!TID_list.ContainsKey(i))
                    {
                        TID_list[i] = new List<int>();

                    }                  
                    TID_list[i].Add(index);
                }
            }
            // Display TID_list[i].
            //foreach (KeyValuePair<string, List<int>> pair in TID_list)
            //{
                //Console.WriteLine("{0}: {1}", pair.Key, string.Join(",",pair.Value));
            //}
            List<int> TID_list_p = new List<int>(TID_list[p[0]]);
            for (int i=1; i<p.Count();i++)
            {
                TID_list_p = TID_list_p.Intersect(TID_list[p[i]]).ToList();
            }
            //Console.WriteLine("TID List of P: {0}",string.Join(",",TID_list_p));
            // Get DB_p
            List<List<string>> db_p = new List<List<string>>();
            foreach(int tid in TID_list_p)
            {
                db_p.Add(db[tid]);
            }
            // Display DB_p.
            //Console.WriteLine("Database db_p:");
            //foreach (var item in db_p)
            //{
                //Console.WriteLine(string.Join(",",item));
            //}
            // Re run NT
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            Dictionary<string, int> co = new Dictionary<string, int>();
            foreach (var t in db_p)
            {
                List<string> coList = Utils.GetCoItems(p, t);
                foreach (var item in coList)
                {
                    if (!co.ContainsKey(item))
                    {
                        co.Add(item, 1);
                    }
                    else
                    {
                        co[item] = co[item] + 1;
                    }
                }
            }
            Console.WriteLine("Rtk result:");
            var rTK = (from entry in co orderby entry.Value descending select entry).Take(k);
            
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            
            // Display results.
            foreach (KeyValuePair<string, int> pair in rTK)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("Time taken: {0}ms", elapsedMs);
            return elapsedMs;
        }

    }
}
