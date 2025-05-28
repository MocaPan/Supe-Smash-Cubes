using System.Collections.Generic;

public class BST : IProgrammingTree
{
    private class Node
    {
        public int value;
        public Node left, right;
        public Node(int value) => this.value = value;
    }

    private Node root;

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    private Node InsertRecursive(Node node, int value)
    {
        if (node == null)
            return new Node(value);
        if (value < node.value)
            node.left = InsertRecursive(node.left, value);
        else
            node.right = InsertRecursive(node.right, value);
        return node;
    }

    public List<int> GetValues()
    {
        List<int> values = new List<int>();
        InOrderTraversal(root, values);
        return values;
    }

    private void InOrderTraversal(Node node, List<int> values)
    {
        if (node == null) return;
        InOrderTraversal(node.left, values);
        values.Add(node.value);
        InOrderTraversal(node.right, values);
    }
}
