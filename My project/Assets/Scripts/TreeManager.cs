using UnityEngine;

public enum TreeType { BST, AVL, BTree }

public class TreeManager : MonoBehaviour
{
    public TreeType selectedTreeType = TreeType.BST;
    public static TreeType CurrentTreeType;

    [Header("Referencias a jugadores")]
    public PlayerTreeHandler player1;
    public PlayerTreeHandler player2;

    private void Awake()
    {
        CurrentTreeType = selectedTreeType;
    }

    public void InsertNumberForPlayer(int number, int playerId)
    {
        Debug.Log($"Insertando número {number} para jugador {playerId}");
        if (playerId == 1 && player1 != null)
            player1.AddNumber(number);
        else if (playerId == 2 && player2 != null)
            player2.AddNumber(number);
    }
}
