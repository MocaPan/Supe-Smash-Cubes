using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerTreeHandler : MonoBehaviour
{
    public IProgrammingTree<int> myTree; // BST, AVLTree, BTree, etc.
    public IProgrammingTree<int> Tree => myTree;

    public void AddNumber(int value)
    {
        if (myTree != null)
            myTree.Insert(value);
    }

    public IProgrammingTree<int> GetTree()
    {
        return myTree;
    }
}


