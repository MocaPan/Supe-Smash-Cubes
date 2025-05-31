using System.Collections.Generic;

public interface IProgrammingTreeNode<T>
{
    T GetValue();
    List<IProgrammingTreeNode<T>> GetChildren();
}

