using UnityEngine;

public enum TreeType { BST, AVL, BTree }

public class TreeManager : MonoBehaviour
{
    public static TreeType CurrentTreeType = TreeType.BST; // Cambia a AVL o BTree si deseas
}

