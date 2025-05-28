using System.Collections.Generic;

public class BTree : IProgrammingTree
{
    private class Node
    {
        public List<int> keys = new List<int>();
        public List<Node> children = new List<Node>();
        public bool isLeaf = true;

        public Node() { }
    }

    private Node root = new Node();
    private int t = 2; // Grado mínimo del B-Tree

    public void Insert(int value)
    {
        if (root.keys.Count == 2 * t - 1)
        {
            Node newRoot = new Node { isLeaf = false };
            newRoot.children.Add(root);
            SplitChild(newRoot, 0);
            root = newRoot;
        }

        InsertNonFull(root, value);
    }

    private void InsertNonFull(Node node, int value)
    {
        int i = node.keys.Count - 1;

        if (node.isLeaf)
        {
            node.keys.Add(0); // Espacio para el nuevo valor
            while (i >= 0 && value < node.keys[i])
            {
                node.keys[i + 1] = node.keys[i];
                i--;
            }
            node.keys[i + 1] = value;
        }
        else
        {
            while (i >= 0 && value < node.keys[i])
                i--;

            i++;
            if (node.children[i].keys.Count == 2 * t - 1)
            {
                SplitChild(node, i);
                if (value > node.keys[i])
                    i++;
            }

            InsertNonFull(node.children[i], value);
        }
    }

    private void SplitChild(Node parent, int index)
    {
        Node fullChild = parent.children[index];
        Node newChild = new Node { isLeaf = fullChild.isLeaf };

        parent.keys.Insert(index, fullChild.keys[t - 1]);
        parent.children.Insert(index + 1, newChild);

        // Copiar la mitad derecha de las claves y niños
        for (int j = 0; j < t - 1; j++)
            newChild.keys.Add(fullChild.keys[j + t]);

        if (!fullChild.isLeaf)
        {
            for (int j = 0; j < t; j++)
                newChild.children.Add(fullChild.children[j + t]);
        }

        // Reducir el tamaño del nodo dividido
        fullChild.keys.RemoveRange(t - 1, fullChild.keys.Count - (t - 1));
        if (!fullChild.isLeaf)
            fullChild.children.RemoveRange(t, fullChild.children.Count - t);
    }

    public List<int> GetValues()
    {
        List<int> values = new List<int>();
        Traverse(root, values);
        return values;
    }

    private void Traverse(Node node, List<int> values)
    {
        for (int i = 0; i < node.keys.Count; i++)
        {
            if (!node.isLeaf)
                Traverse(node.children[i], values);
            values.Add(node.keys[i]);
        }

        if (!node.isLeaf)
            Traverse(node.children[node.keys.Count], values);
    }
}
