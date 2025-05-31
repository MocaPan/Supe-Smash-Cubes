using UnityEngine;

public class TreeUITrigger : MonoBehaviour
{
    public TreeVisualizer visualizer;
    public PlayerTreeHandler treeHandler;

    public void ShowTree()
    {
        visualizer.VisualizeTree(treeHandler.tree);
    }
}
