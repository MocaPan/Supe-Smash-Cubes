using UnityEngine;
using System.Collections.Generic;

public class TreeVisualizer : MonoBehaviour
{
    public static TreeVisualizer Instance { get; private set; }

    public GameObject nodePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Visualize(IProgrammingTree<int> tree, Transform origin)
    {
        ClearPrevious(origin);

        if (tree == null || tree.GetRoot() == null) return;

        VisualizeRecursive(tree.GetRoot(), Vector2.zero, 0, origin);
    }

    private void VisualizeRecursive(IProgrammingTreeNode<int> node, Vector2 position, int depth, Transform parent)
    {
        var nodeGO = Instantiate(nodePrefab, parent);
        var rectTransform = nodeGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

        var nodeUI = nodeGO.GetComponent<TreeNodeUI>();
        nodeUI.SetValue(node.GetValue());

        var children = node.GetChildren();
        float spacing = 200f / (depth + 1);
        for (int i = 0; i < children.Count; i++)
        {
            float xOffset = (i - children.Count / 2f) * spacing;
            Vector2 childPos = position + new Vector2(xOffset, -100);
            VisualizeRecursive(children[i], childPos, depth + 1, parent);
        }
    }

    private void ClearPrevious(Transform origin)
    {
        foreach (Transform child in origin)
        {
            Destroy(child.gameObject);
        }
    }
}
