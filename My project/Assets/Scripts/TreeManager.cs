using UnityEngine;

public enum TreeType { BST, AVL, BTree }

public class TreeManager : MonoBehaviour
{
    public TreeType selectedTreeType = TreeType.BST;
    public static TreeType CurrentTreeType;

    private void Awake()
    {
        CurrentTreeType = selectedTreeType;
    }
}


