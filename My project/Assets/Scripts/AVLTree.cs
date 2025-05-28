using System.Collections.Generic;
using UnityEngine;

public class AVLTree : IProgrammingTree
{
    private class Node
    {
        public int value;
        public Node left, right;
        public int height;

        public Node(int value)
        {
            this.value = value;
            this.height = 1;
        }
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

        
        node.height = 1 + Mathf.Max(GetHeight(node.left), GetHeight(node.right));// Actualiza altura

        
        int balance = GetBalance(node);

        // Rotaciones
        
        if (balance > 1 && value < node.left.value)// Izquierda Izquierda
            return RotateRight(node);

        
        if (balance < -1 && value > node.right.value)// Derecha Derecha
            return RotateLeft(node);

        
        if (balance > 1 && value > node.left.value)// Izquierda Derecha
        {
            node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        
        if (balance < -1 && value < node.right.value)// Derecha Izquierda
        {
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }

    private int GetHeight(Node node) => node?.height ?? 0;

    private int GetBalance(Node node) => node == null ? 0 : GetHeight(node.left) - GetHeight(node.right);

    private Node RotateRight(Node y)
    {
        Node x = y.left;
        Node T2 = x.right;

        // Rotación
        x.right = y;
        y.left = T2;

        // Actualiza alturas
        y.height = 1 + Mathf.Max(GetHeight(y.left), GetHeight(y.right));
        x.height = 1 + Mathf.Max(GetHeight(x.left), GetHeight(x.right));

        return x;
    }

    private Node RotateLeft(Node x)
    {
        Node y = x.right;
        Node T2 = y.left;

        // Rotación
        y.left = x;
        x.right = T2;

        // Actualiza alturas
        x.height = 1 + Mathf.Max(GetHeight(x.left), GetHeight(x.right));
        y.height = 1 + Mathf.Max(GetHeight(y.left), GetHeight(y.right));

        return y;
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
