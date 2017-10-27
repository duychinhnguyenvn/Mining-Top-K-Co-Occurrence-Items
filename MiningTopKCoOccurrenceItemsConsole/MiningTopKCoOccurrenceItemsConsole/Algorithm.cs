using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public AlgorithmResult nt(List<List<string>> db,List<string> p,int k)
        {
            Dictionary<string, int> co = new Dictionary<string, int>();
            double runningTime = 0;
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
            var lk = (from entry in co orderby entry.Value descending select entry).Take(k);
            watch.Stop();
            runningTime = watch.Elapsed.TotalMilliseconds;            
            return new AlgorithmResult(runningTime,0,0,lk);
        }
        public AlgorithmResult nti(List<List<string>> db, List<string> p, int k) {
            //build Tid-set
            double timeToBuildTidSet = 0;
            double processingTime = 0;
            long preprocessingMem = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine("Original mem: {0}MB", memoryUsed/ 1048576);
            List < List < string >> db_p = getDBP(db, p);
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine("Original mem: {0}MB", memoryUsed / 1048576);
            preprocessingMem = GC.GetTotalMemory(true) - memoryUsed;
            Console.WriteLine("Preprocessing mem Usage: {0}MB", preprocessingMem / 1048576);
            watch.Stop();
            timeToBuildTidSet = watch.Elapsed.TotalMilliseconds;
            // Re run NT
            watch.Reset();
            watch = System.Diagnostics.Stopwatch.StartNew();
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
            var lk = (from entry in co orderby entry.Value descending select entry).Take(k);            
            watch.Stop();
            processingTime = watch.Elapsed.TotalMilliseconds;
            
            return new AlgorithmResult(processingTime,preprocessingMem,timeToBuildTidSet,lk);
        }
        public AlgorithmResult ntta(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
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
            processingTime = watch.Elapsed.TotalMilliseconds;
            var lk = new Dictionary<string,int>();
            //Build result
            foreach (var item in LK)
            {
                lk[item] = CO[item];
            }
            return new AlgorithmResult(processingTime,0,0,lk);
        }
        public AlgorithmResult ntita(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
            double timeToBuildTidSet = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<List<string>> dbp = getDBP(db, p);
            watch.Stop();
            timeToBuildTidSet = watch.Elapsed.TotalMilliseconds;
            Algorithm algorithm = new Algorithm();
            var result = algorithm.ntta(dbp, p, k);
            processingTime = result.getRunningTime();
            return new AlgorithmResult(processingTime,0,timeToBuildTidSet,result.getListTopK());
        }
        public AlgorithmResult pt(List<List<string>> db, List<string> p, int k)
        {
            double procesingTime = 0;
            double timeToBuildPiTree = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine("Original mem: {0}MB", memoryUsed / 1048576);
            PiTree ptr = new PiTree(db);
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine("Original mem: {0}MB", memoryUsed / 1048576);
            watch.Stop();
            timeToBuildPiTree = watch.Elapsed.TotalMilliseconds;
            watch = System.Diagnostics.Stopwatch.StartNew();
            Dictionary<string, List<PiTreeNode>> headerTable = ptr.getHeaderTable();
            Dictionary<string, int> CO_Items = ptr.getCoOccurrenceList();
            Dictionary<string, int> CO_Result = new Dictionary<string, int>();
            // Sort the pattern
            p.Sort(delegate (string x, string y) {
                if (CO_Items[x] < CO_Items[y]) { return 1; }
                else if (CO_Items[x] == CO_Items[y])
                {
                    return x.CompareTo(y);
                }
                else return -1;
            });
            //Console.WriteLine("Process pattern: {0}", string.Join(",", p));
            int s = p.Count-1;
            string i_s = p[s];
            List<PiTreeNode> CNS = headerTable[i_s];
            //Console.WriteLine("Total Nodes regiter {0}: {1}",i_s,CNS.Count);
            List<PiTreeNode> NS = new List<PiTreeNode>();
            foreach (var n in CNS) {
                int x = s - 1;                
                bool flag = false;
                PiTreeNode currentNode = n.getParentNode();                
                while (true)
                {
                    if (x < 0 || flag == true) break;
                    string i_x = p[x];
                    if (i_x.Equals(currentNode.getLabel())) {
                        x--;
                        currentNode = currentNode.getParentNode();
                    }
                    else
                    {
                        if (CO_Items[i_x] >= CO_Items[currentNode.getLabel()])
                        {
                            currentNode = currentNode.getParentNode();
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                }
                if (flag == false)
                {
                    NS.Add(n);
                }
            }
            foreach (var n in NS) {
                //scan up
                //Console.WriteLine("Scan up!");
                PiTreeNode up = n.getParentNode();
                while (up != null && !up.getLabel().Equals("Root"))
                {
                    if (!p.Contains(up.getLabel()))
                    {
                        if (!CO_Result.ContainsKey(up.getLabel())) {
                            CO_Result[up.getLabel()] = 0;
                        }
                        CO_Result[up.getLabel()] = CO_Result[up.getLabel()]+ n.getCount();
                    }
                    up = up.getParentNode();
                }
                //scan down
                //Console.WriteLine("Scan down!");
                foreach (var down in n.getChildrenNodes())
                {
                    Utils.travelDownSetCo(down, CO_Result);
                }
            }
            // show result
            var lk = (from entry in CO_Result orderby entry.Value descending select entry).Take(k);
            watch.Stop();
            procesingTime = watch.Elapsed.TotalMilliseconds;
            return new AlgorithmResult(procesingTime,0,timeToBuildPiTree,lk);
        }
        public AlgorithmResult ptta(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
            return new AlgorithmResult(processingTime,0,0,new Dictionary<string,int>());
        }
        public AlgorithmResult bt(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
            double timeToBuildBitTable = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            BitTable bitTable = new BitTable(db);
            watch.Stop();
            timeToBuildBitTable = watch.Elapsed.TotalMilliseconds;
            watch.Reset();
            string[] headerColumns = bitTable.getHeaderColumns();
            //bitTable.showBitTable();
            //Console.WriteLine("Pattern: {0}",string.Join(",",p));
            Dictionary<int, int> CO = new Dictionary<int, int>();
            System.Collections.BitArray bitPattern = bitTable.getBitArrayPattern(p);
            watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var tran in bitTable.getData())
            {
                //check P is subset of tran
                bool isSubset = true;
                BitArray tempTran = new BitArray(tran);
                BitArray andPattern = tempTran.And(bitPattern);
                for (int i=0; i<tran.Length;i++)
                {
                    if (andPattern[i] != bitPattern[i])
                    {
                        isSubset = false;
                        break;
                    }
                }
                if (isSubset)
                {
                    for(int i = 0; i < tran.Length; i++)
                    {
                        if (tran[i]&&!bitPattern[i])
                        {
                            if (!CO.ContainsKey(i)) CO[i] = 0;
                            CO[i]++;
                        }
                    }
                }
            }
            Dictionary<string, int> CO_result = new Dictionary<string, int>();
            foreach(var item in CO)
            {
                CO_result[headerColumns[item.Key]] = item.Value;
            }
            var lk = (from entry in CO_result orderby entry.Value descending select entry).Take(k);
            watch.Stop();
            processingTime = watch.Elapsed.TotalMilliseconds;
            return new AlgorithmResult(processingTime,0,timeToBuildBitTable,lk);
        }
        public AlgorithmResult bti(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
            double timeToBuildTidSet = 0;
            double timeToBuildBitTable = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<List<string>> dbp = getDBP(db, p);
            watch.Stop();
            timeToBuildTidSet = watch.Elapsed.TotalMilliseconds;
            Algorithm algorithm = new Algorithm();
            var algorithmResult = algorithm.bt(dbp,p,k);
            processingTime = algorithmResult.getRunningTime();
            timeToBuildBitTable = algorithmResult.getTimeToBuildDatbase();
            return new AlgorithmResult(processingTime,0,(timeToBuildTidSet+timeToBuildBitTable),algorithmResult.getListTopK());
        }
        public AlgorithmResult btiv(List<List<string>> db, List<string> p, int k)
        {
            double processingTime = 0;
            double timeToBuildBitTable = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<List<string>> dbp = getDBP(db, p);
            //build bittable h
            BitTableVertical bitTable = new BitTableVertical(dbp);            
            //bitTable.show();
            Dictionary<string, BitArray> data = bitTable.getData();
            //remove item in pattern
            foreach(var item in p)
            {
                data.Remove(item);
            }
            watch.Stop();
            timeToBuildBitTable = watch.Elapsed.TotalMilliseconds;
            watch.Reset();
            //calculate CO
            Dictionary<string, int> CO = new Dictionary<string, int>();
            watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var item in data)
            {
                CO[item.Key] = Utils.GetCardinality(item.Value);
            }
            
            var lk = (from entry in CO orderby entry.Value descending select entry).Take(k);
            watch.Stop();
            processingTime = watch.Elapsed.TotalMilliseconds;
            return new AlgorithmResult(processingTime,0,timeToBuildBitTable,lk);
        }
    }
}
