using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerTreeHandler : MonoBehaviour
{
    private IProgrammingTree tree;

    private void Awake()
    {
        switch (TreeManager.CurrentTreeType)
        {
            case TreeType.BST:
                tree = new BST();
                break;
            case TreeType.AVL:
                tree = new AVLTree();
                break;
            case TreeType.BTree:
                tree = new BTree(); 
                break;
        }
    }

    public void AddNumber(int number)
    {
        tree.Insert(number);

        if (TreeVisualizer.Instance != null)
        {
            TreeVisualizer.Instance.Visualize(tree, this.transform);
        }
        else
        {
            Debug.LogWarning("TreeVisualizer.Instance no está asignado en la escena.");
        }
    }
}
