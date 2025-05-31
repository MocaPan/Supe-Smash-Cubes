using System.Collections.Generic;
using UnityEngine;

public class AVLTree : IProgrammingTree<int>
{
    private AVLNode root;

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    private AVLNode InsertRecursive(AVLNode node, int value)
    {
        if (node == null) return new AVLNode(value);

        if (value < node.value)
            node.left = InsertRecursive(node.left, value);
        else if (value > node.value)
            node.right = InsertRecursive(node.right, value);
        else
            return node;

        node.UpdateHeight();
        return Balance(node);
    }

    private AVLNode Balance(AVLNode node)
    {
        int balance = node.BalanceFactor;

        if (balance > 1)
        {
            if (node.left.BalanceFactor < 0)
                node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        if (balance < -1)
        {
            if (node.right.BalanceFactor > 0)
                node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }

    private AVLNode RotateLeft(AVLNode node)
    {
        // Standard AVL Left Rotation
        AVLNode newRoot = node.right;
        node.right = newRoot.left;
        newRoot.left = node;
        node.UpdateHeight();
        newRoot.UpdateHeight();
        return newRoot;
    }

    private AVLNode RotateRight(AVLNode node)
    {
        // Standard AVL Right Rotation
        AVLNode newRoot = node.left;
        node.left = newRoot.right;
        newRoot.right = node;
        node.UpdateHeight();
        newRoot.UpdateHeight();
        return newRoot;
    }

    public IProgrammingTreeNode<int> GetRoot() => root;

    public class AVLNode : IProgrammingTreeNode<int>
    {
        public int value;
        public AVLNode left, right;
        public int height = 1;

        public AVLNode(int val)
        {
            value = val;
        }

        public void UpdateHeight()
        {
            int leftHeight = left?.height ?? 0;
            int rightHeight = right?.height ?? 0;
            height = 1 + Mathf.Max(leftHeight, rightHeight);
        }

        public int BalanceFactor => (left?.height ?? 0) - (right?.height ?? 0);

        public int GetValue() => value;

        public List<IProgrammingTreeNode<int>> GetChildren()
        {
            var children = new List<IProgrammingTreeNode<int>>();
            if (left != null) children.Add(left);
            if (right != null) children.Add(right);
            return children;
        }
    }

    public int CountNodes()
    {
        return CountNodesRecursive(root);
    }

    private int CountNodesRecursive(AVLNode node)
    {
        if (node == null) return 0;
        return 1 + CountNodesRecursive(node.left) + CountNodesRecursive(node.right);
    }

    public int GetHeight()
    {
        return GetHeightRecursive(root);
    }

    private int GetHeightRecursive(AVLNode node)
    {
        if (node == null) return 0;
        return 1 + Mathf.Max(GetHeightRecursive(node.left), GetHeightRecursive(node.right));
    }

    public int GetRootValue()
    {
        return root != null ? root.value : -1;
    }

    public int GetBalanceFactorRoot()
    {
        return root == null ? 0 : root.BalanceFactor;
    }
}
