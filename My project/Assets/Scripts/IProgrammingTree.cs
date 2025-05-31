using System.Collections.Generic;
public interface IProgrammingTree<T>
{
    IProgrammingTreeNode<T> GetRoot();
    void Insert(T value);
}
