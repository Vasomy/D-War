using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// 右键指令
public enum ECommandType : int
{ 
    // 指令的优先级从上到下
    eNone = 0,
    eMove = 1<<0,
    eAttack = 1<<1,
    eCollect = 1<<2,
}
// 该类的执行顺序应该在最后
public class FCommandManager : MonoBehaviour
{
    #region right_button_command

    private ECommandType rightCommand;
    public void SetRightButtonCommand(
        ECommandType rightCommand)
    {
        this.rightCommand |= rightCommand;
    }

    private void ProccessRightButton()
    {

    }
    #endregion

    private void LateUpdate()
    {
        Debug.Log("Late Update!");
        if(EnumOperator.HasEnum(rightCommand,ECommandType.eCollect))
        {

        }
    }
}
