using UnityEngine;
using System;
using System.Collections.Generic;

public class ChallengeManager : MonoBehaviour
{
    public IProgrammingTree<int> playerTree;
    public Score playerScore;
    public int pointsPerChallenge = 100;

    // Lista de retos (puedes expandir)
    private List<Action> challengeSetters;
    private System.Random random = new System.Random();
    public Func<IProgrammingTree<int>, bool> currentChallenge;
    public string challengeDescription;

    private void Awake()
    {
        challengeSetters = new List<Action>
        {
            SetBSTChallenge_5Nodes,
            SetBSTChallenge_Height3,
            () => SetBSTChallenge_RootValue(10),    // Puedes poner X random aquí
            SetAVLChallenge_6Nodes,
            SetAVLChallenge_Balance0,
            () => SetAVLChallenge_RootValue(20),
            SetBTreeChallenge_7Keys,
            SetBTreeChallenge_RootTwoChildren,
            SetBTreeChallenge_Leaf3Keys
        };
    }

    private void Start()
    {
        NextRandomChallenge();
    }

    // Llama esto cada vez que el jugador inserta un número
    public bool OnPlayerInsertedNumber(IProgrammingTree<int> tree, Score score)
    {
        if (currentChallenge != null && currentChallenge(tree))
        {
            if (score != null) score.AddScore(pointsPerChallenge);
            Debug.Log("¡Reto cumplido! " + challengeDescription);
            NextRandomChallenge();
            return true;
        }
        return false;
    }

    // Selecciona un reto aleatorio
    public void NextRandomChallenge()
    {
        int idx = random.Next(challengeSetters.Count);
        challengeSetters[idx]();
        Debug.Log("Nuevo reto: " + challengeDescription);
    }

    // ======================== RETOS ========================
    public void SetBSTChallenge_5Nodes()
    {
        challengeDescription = "Construye un BST con exactamente 5 nodos";
        currentChallenge = (tree) => (tree as BSTTree)?.CountNodes() == 5;
    }

    public void SetBSTChallenge_Height3()
    {
        challengeDescription = "Construye un BST de altura exactamente 3";
        currentChallenge = (tree) => (tree as BSTTree)?.GetHeight() == 3;
    }

    public void SetBSTChallenge_RootValue(int value)
    {
        challengeDescription = $"La raíz del BST debe ser igual a {value}";
        currentChallenge = (tree) => (tree as BSTTree)?.GetRootValue() == value;
    }

    public void SetAVLChallenge_6Nodes()
    {
        challengeDescription = "Construye un AVL con exactamente 6 nodos";
        currentChallenge = (tree) => (tree as AVLTree)?.CountNodes() == 6;
    }

    public void SetAVLChallenge_Balance0()
    {
        challengeDescription = "El factor de balance de la raíz del AVL debe ser 0";
        currentChallenge = (tree) => (tree as AVLTree)?.GetBalanceFactorRoot() == 0;
    }

    public void SetAVLChallenge_RootValue(int value)
    {
        challengeDescription = $"La raíz del AVL debe ser igual a {value}";
        currentChallenge = (tree) => (tree as AVLTree)?.GetRootValue() == value;
    }

    public void SetBTreeChallenge_7Keys()
    {
        challengeDescription = "El B-Tree debe tener al menos 7 claves";
        currentChallenge = (tree) => (tree as BTree)?.CountKeys() >= 7;
    }

    public void SetBTreeChallenge_RootTwoChildren()
    {
        challengeDescription = "La raíz del B-Tree debe tener al menos 2 hijos";
        currentChallenge = (tree) => (tree as BTree)?.RootChildrenCount() >= 2;
    }

    public void SetBTreeChallenge_Leaf3Keys()
    {
        challengeDescription = "Al menos una hoja del B-Tree debe tener exactamente 3 claves";
        currentChallenge = (tree) => (tree as BTree)?.HasLeafWithThreeKeys() == true;
    }
}
