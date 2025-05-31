using System.Collections.Generic;
using UnityEngine;

public class BSTTree : IProgrammingTree<int>
{
    private BSTNode root;

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    private BSTNode InsertRecursive(BSTNode node, int value)
    {
        if (node == null) return new BSTNode(value);
        if (value < node.value)
            node.left = InsertRecursive(node.left, value);
        else
            node.right = InsertRecursive(node.right, value);
        return node;
    }

    public IProgrammingTreeNode<int> GetRoot() => root;

    public class BSTNode : IProgrammingTreeNode<int>
    {
        public int value;
        public BSTNode left, right;

        public BSTNode(int val)
        {
            value = val;
        }

        public int GetValue() => value;

        public List<IProgrammingTreeNode<int>> GetChildren()
        {
            var children = new List<IProgrammingTreeNode<int>>();
            if (left != null) children.Add(left);
            if (right != null) children.Add(right);
            return children;
        }
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
    