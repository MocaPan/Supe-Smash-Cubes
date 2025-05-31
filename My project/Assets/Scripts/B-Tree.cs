using System.Collections.Generic;

public class BTree : IProgrammingTree<int>
{
    private int T;
    private BTreeNode root;

    public BTree(int t)
    {
        T = t;
        root = new BTreeNode(true, t);
    }

    public void Insert(int value)
    {
        if (root.IsFull)
        {
            BTreeNode newRoot = new BTreeNode(false, T);
            newRoot.children.Add(root);
            newRoot.SplitChild(0, root);
            root = newRoot;
        }

        root.InsertNonFull(value);
    }

    public IProgrammingTreeNode<int> GetRoot() => root;

    public class BTreeNode : IProgrammingTreeNode<int>
    {
        public List<int> keys = new List<int>();
        public List<BTreeNode> children = new List<BTreeNode>();
        public bool leaf;
        private int t; // minimum degree

        public BTreeNode(bool isLeaf, int t)
        {
            leaf = isLeaf;
            this.t = t;
        }

        public bool IsFull => keys.Count == 2 * t - 1;

        public void InsertNonFull(int key)
        {
            int i = keys.Count - 1;

            if (leaf)
            {
                keys.Add(0); // Add a dummy value for shifting
                while (i >= 0 && key < keys[i])
                {
                    keys[i + 1] = keys[i];
                    i--;
                }
                keys[i + 1] = key;
            }
            else
            {
                while (i >= 0 && key < keys[i]) i--;
                i++;
                if (children[i].IsFull)
                {
                    SplitChild(i, children[i]);
                    if (key > keys[i]) i++;
                }
                children[i].InsertNonFull(key);
            }
        }

        public void SplitChild(int i, BTreeNode y)
        {
            BTreeNode z = new BTreeNode(y.leaf, t);

            // Copy last t-1 keys of y to z
            for (int j = 0; j < t - 1; j++)
                z.keys.Add(y.keys[j + t]);

            if (!y.leaf)
            {
                for (int j = 0; j < t; j++)
                    z.children.Add(y.children[j + t]);
                y.children.RemoveRange(t, y.children.Count - t);
            }

            y.keys.RemoveRange(t - 1, y.keys.Count - (t - 1));
            children.Insert(i + 1, z);
            keys.Insert(i, y.keys[t - 1]);
            y.keys.RemoveAt(t - 1); // Remove the median key
        }

        public int GetValue() => keys.Count > 0 ? keys[0] : -1;

        public List<IProgrammingTreeNode<int>> GetChildren()
        {
            return new List<IProgrammingTreeNode<int>>(children);
        }
    }

    public int CountKeys()
    {
        return CountKeysRecursive(root);
    }
    private int CountKeysRecursive(BTreeNode node)
    {
        if (node == null) return 0;
        int sum = node.keys.Count;
        if (!node.leaf)
        {
            foreach (var child in node.children)
                sum += CountKeysRecursive(child);
        }
        return sum;
    }

    // Number of children of the root
    public int RootChildrenCount()
    {
        return root != null ? root.children.Count : 0;
    }

    // Returns true if any leaf has exactly 3 keys
    public bool HasLeafWithThreeKeys()
    {
        return HasLeafWithNKeys(root, 3);
    }
    private bool HasLeafWithNKeys(BTreeNode node, int n)
    {
        if (node == null) return false;
        if (node.leaf) return node.keys.Count == n;
        foreach (var child in node.children)
            if (HasLeafWithNKeys(child, n))
                return true;
        return false;
    }
}
