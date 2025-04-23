using System;


public class FTechTreeNode
{
    public int level = 1;
    public int maxLevel = 1;
    // 如果没有等级，则不用传递任何参数
    public virtual void Active(int level = 1)
    {

    }

    public virtual void UndoActive()
    {

    }


}
