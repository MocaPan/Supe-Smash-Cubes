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
    
    public Transform treeOrigin;
    public void AddNumber(int number)
    {
        tree.Insert(number);
        Debug.Log($"Insertado n�mero {number} en el �rbol de {gameObject.name}");

        if (TreeVisualizer.Instance != null)
        {
            TreeVisualizer.Instance.Visualize(tree, treeOrigin);
        }
        else 
        {
            Debug.LogWarning("TreeVisualizer.Instance no est� asignado.");
        }

    public IProgrammingTree<int> GetTree()
    {
        return myTree;
    }
}