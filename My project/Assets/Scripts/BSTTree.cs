using System.Collections.Generic;
using UnityEngine;

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
    // BSTTree.cs
    public int CountNodes()
    {
        return CountNodesRecursive(root);
    }
    private int CountNodesRecursive(Node node)
    {
        if (node == null) return 0;
        return 1 + CountNodesRecursive(node.left) + CountNodesRecursive(node.right);
    }

    public int GetHeight()
    {
        return GetHeightRecursive(root);
    }
    private int GetHeightRecursive(Node node) {
        if (node == null) return 0;
        return 1 + Mathf.Max(GetHeightRecursive(node.left), GetHeightRecursive(node.right));

    }

    public int GetRootValue()
    {
        return root != null ? root.value : -1; // o el valor que represente "vacío"
    }

}
    