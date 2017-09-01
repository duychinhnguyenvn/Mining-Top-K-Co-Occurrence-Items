using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class PiTree
    {
        private Dictionary<string, List<PiTreeNode>> headerTable;
        private PiTreeNode root;
        public PiTree(List<List<string>> db)
        {
            this.headerTable = new Dictionary<string, List<PiTreeNode>>();
            this.root = new PiTreeNode("Root");
            // Scan all database to get support count
            Dictionary<string, int> CO = new Dictionary<string, int>();
            foreach(var tran in db)
            {
                foreach(var i in tran)
                {
                    if (!CO.ContainsKey(i))
                    {
                        CO[i] = 1;
                    }
                    else
                    {
                        CO[i]++;
                    }
                }
            }
            // Create header table
            var tempList = (from entry in CO orderby entry.Value descending select entry).ToList();
            foreach (var i in tempList) 
            {
                headerTable[i.Key] = new List<PiTreeNode>();
            }
            // Build tree
            foreach(var tran in db)
            {
                //sort tran
                tran.Sort(delegate (string x, string y) {
                    if (CO[x] < CO[y]) { return 1; }
                    else if (CO[x] == CO[y])
                    {
                        return x.CompareTo(y);
                    }
                    else return -1;
                });
                if (tran.Count > 0)
                {
                    string i = tran[0];
                    tran.Remove(i);
                    insertTree(i, tran, root, headerTable);
                }
            }

        }
        public void showTree() {
            showTree(this.root);
        }
        public PiTreeNode getRootNode()
        {
            return this.root;
        }
        public Dictionary<string, List<PiTreeNode>> getHeaderTable()
        {
            return this.headerTable;
        }
        private void showTree(PiTreeNode root)
        {
            if (root.getParentNode() != null)
            {
                if (root.getParentNode().getLabel().Equals("Root")) {
                    PiTreeNode father = root.getParentNode();
                    Console.WriteLine("node: {0}:{1} <---- {2}:{3}", root.getLabel(), root.getCount(), father.getLabel(), father.getCount());
                } 
            }
            else
            {
                Console.WriteLine("node: {0}:{1}", root.getLabel(), root.getCount());
            }
            
            foreach (var children in root.getChildrenNodes()) {
                showTree(children);
            }
        }
        private void insertTree(string i,List<string> tran, PiTreeNode ptr, Dictionary<string, List<PiTreeNode>> headerTable)
        {
            //Find child nodes regiter i
            PiTreeNode temp = null;
            foreach (var childrenNode in ptr.getChildrenNodes())
            {
                if (childrenNode.getLabel().Equals(i))
                {
                    temp=childrenNode;
                    break;
                }
            }
            if (temp!=null) {
                temp.increaseCount();                            
            }
            else
            {
                temp = new PiTreeNode(i);
                ptr.addChildrenNode(temp);
                temp.setParentLink(ptr);
                headerTable[i].Add(temp);
                
            }
            //if tran is not empty continue build tree
            if (tran.Count > 0)
            {
                i = tran[0];
                tran.Remove(i);
                insertTree(i, tran, temp,headerTable);
            }
        }
    }
}
