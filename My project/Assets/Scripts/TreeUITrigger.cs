using UnityEngine;

public class TreeUITrigger : MonoBehaviour
{
    public TreeVisualizer visualizer;
    public PlayerTreeHandler treeHandler;

    public void ShowTree()
    {
        visualizer.Visualize(treeHandler.Tree, treeHandler.transform);

    }
}
