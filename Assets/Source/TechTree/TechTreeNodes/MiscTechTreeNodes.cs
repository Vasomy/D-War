using System;
using System.Collections.Generic;

/// <summary>
/// 攻击强化
/// </summary>
/// 
public class FAttackUpTechTreeNodes : FTechTreeNode
{
    List<FPercent> attackBounces { get; set; }
    public FAttackUpTechTreeNodes()
    {
        attackBounces = new List<FPercent> { 0.1f,0.2f,0.3f};
    }

    public override void Active(int level = 1)
    {
        base.Active(level);
        
    }
}
