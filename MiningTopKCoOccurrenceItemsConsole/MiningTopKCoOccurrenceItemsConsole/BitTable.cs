using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class BitTable
    {
        private List<BitArray> data;
        private string[] headerColumns;
        private Dictionary<string, int> indexDic;
        public BitTable(List<List<string>> db)
        {
            indexDic = new Dictionary<string, int>();
            //Scan all items
            List<string> tempList = new List<string>();
            foreach(var tran in db)
            {
                foreach(var i in tran)
                {
                    if (!tempList.Contains(i)) tempList.Add(i);
                }
            }
            tempList.Sort();
            this.headerColumns = tempList.ToArray();
            for (int i=0;i<this.headerColumns.Length;i++)
            {
                indexDic[this.headerColumns[i]] = i;
            }
            // Build bit table
            this.data = new List<BitArray>();
            foreach(var tran in db)
            {
                BitArray bitTran = new BitArray(this.headerColumns.Length);
                foreach(var item in tran)
                {
                    bitTran[indexDic[item]] = true;
                }
                this.data.Add(bitTran);
            }
        }
        public List<BitArray> getData() {
            return this.data;
        }
        public Dictionary<string,int> getIndexDic()
        {
            return this.indexDic;
        }
        public string[] getHeaderColumns()
        {
            return this.headerColumns;
        }
        public void showBitTable() {
            Console.WriteLine(string.Join(" ",this.headerColumns));
            foreach (var tran in this.data)
            {
                for (int i=0;i<tran.Length;i++)
                {
                    if (i > 0) Console.Write(" ");
                    if (tran[i]) Console.Write("1");
                    if (!tran[i]) Console.Write("0");
                    if (i == tran.Length - 1) Console.WriteLine();
                }
            }
        }
        public BitArray getBitArrayPattern(List<string> p)
        {
            BitArray bitP = new BitArray(this.headerColumns.Length);
            foreach(var i in p)
            {
                bitP[indexDic[i]] = true;
            }
            return bitP;
        }
    }
}
