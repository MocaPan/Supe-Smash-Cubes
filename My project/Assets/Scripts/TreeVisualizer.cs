using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeVisualizer : MonoBehaviour
{
    public static TreeVisualizer Instance;

    public GameObject treeNodeUIPrefab; // Prefab del nodo visual (círculo con número)
    public float horizontalSpacing = 100f;
    public float verticalSpacing = 100f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Visualize(IProgrammingTree<int> tree, Transform parent)
    {
        if (treeNodeUIPrefab == null)
        {
            Debug.LogError("Falta asignar el prefab del nodo visual en el TreeVisualizer.");
            return;
        }

        // Elimina nodos anteriores
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        IProgrammingTreeNode<int> root = tree.GetRoot();
        if (root != null)
        {
            DrawNode(root, parent, 0, 0, 1);
        }
    }

    private void DrawNode(IProgrammingTreeNode<int> node, Transform parent, float x, float y, float scale)
    {
        GameObject nodeGO = Instantiate(treeNodeUIPrefab, parent);
        RectTransform rectTransform = nodeGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);

        Text numberText = nodeGO.GetComponentInChildren<Text>();
        if (numberText != null)
            numberText.text = node.GetValue().ToString();

        List<IProgrammingTreeNode<int>> children = node.GetChildren();
        if (children.Count == 0) return;

        float width = horizontalSpacing * scale;

        for (int i = 0; i < children.Count; i++)
        {
            float childX = x + (i - (children.Count - 1) / 2f) * width;
            float childY = y - verticalSpacing;
            DrawNode(children[i], parent, childX, childY, scale * 0.7f);
        }
    }
}
