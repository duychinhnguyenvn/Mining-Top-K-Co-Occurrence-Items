﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Algorithm
    {
        private List<List<string>> getDBP(List<List<string>> db, List<string> p)
        {
            List<List<string>> dbp = new List<List<string>>();
            Dictionary<string, List<int>> TID_list = new Dictionary<string, List<int>>();
            int index = -1;
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
            for (int i = 1; i < p.Count(); i++)
            {
                TID_list_p = TID_list_p.Intersect(TID_list[p[i]]).ToList();
            }
            //Console.WriteLine("TID List of P: {0}",string.Join(",",TID_list_p));
            // Get DB_p
            
            foreach (int tid in TID_list_p)
            {
                dbp.Add(db[tid]);
            }
            // Display DB_p.
            //Console.WriteLine("Database db_p:");
            //foreach (var item in db_p)
            //{
            //Console.WriteLine(string.Join(",",item));
            //}
            return dbp;
        }
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
            List < List < string >> db_p = getDBP(db, p);
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
        public double ntta(List<List<string>> db, List<string> p, int k)
        {
            double runningTime = 0;
            List<string> IC = new List<string>();
            List<string> LK = new List<string>();
            int MINV_lsk = 0;
            int MAXV_lsc = 0;
            int CU = db.Count;
            Dictionary<string, int> CO = new Dictionary<string, int>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var tran in db) {
                if (Utils.IsSubSetOf(p,tran)) {
                    List<string> CoList = Utils.GetCoItems(p,tran);
                    foreach(var i in CoList)
                    {
                        if (!CO.ContainsKey(i))
                        {
                            CO[i] = 1;
                        }
                        else
                        {
                            CO[i] = CO[i] + 1;
                        }
                        if (CO[i] > MINV_lsk)
                        {
                            if (!LK.Contains(i)) LK.Add(i);
                            LK.Sort(delegate(string x, string y) {
                                if (CO[x] < CO[y]) { return 1; }
                                else if (CO[x] == CO[y]) {
                                    return x.CompareTo(y);
                                }
                                else return -1;                                
                            });
                                                        
                            if (LK.Count >= k)
                            {
                                MINV_lsk = CO[LK[k - 1]];
                            }
                            List<string> removeItems = new List<string>();
                            foreach (var item in LK)
                            {
                                if (CO[item] < MINV_lsk) removeItems.Add(item);
                            }
                            // remove
                            foreach (var item in removeItems)
                            {
                                LK.Remove(item);
                            }
                        }
                        else
                        {
                            if(!IC.Contains(i)) IC.Add(i);
                            foreach (var ic_item in IC) {
                                if (MAXV_lsc<CO[ic_item])
                                {
                                    MAXV_lsc = CO[ic_item];
                                }
                            }
                        }
                    }
                }
                CU = CU - 1;              
                if (MINV_lsk>MAXV_lsc+CU)
                {
                    break;
                }
            }
            watch.Stop();
            runningTime = watch.Elapsed.TotalMilliseconds;
            //Show LK
            foreach (var item in LK)
            {
                Console.WriteLine("{0}:{1}",item,CO[item]);
            }
            Console.WriteLine("Time taken: {0}ms", runningTime);
            return runningTime;
        }
        public double ntita(List<List<string>> db, List<string> p, int k)
        {
            double runningTime = 0;
            List<List<string>> dbp = getDBP(db, p);
            Algorithm algorithm = new Algorithm();
            runningTime = algorithm.ntta(dbp, p, k);
            return runningTime;
        }
        public double pt(List<List<string>> db, List<string> p, int k)
        {
            double runningTime = 0;
            PiTree ptr = new PiTree(db);
            ptr.showTree();
            return runningTime;
        }
        public double ptta(List<List<string>> db, List<string> p, int k)
        {
            double runningTime = 0;
            return runningTime;
        }
        
    }
}
