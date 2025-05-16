using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// 右键指令
public enum ERightCommandType : int
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
    private static FCommandManager instance;

    /// <summary>
    /// 游戏ui有关的代码在激活ui场景时，应该将该值设为true，
    /// 在关闭ui场景时，应该将值设为false
    /// </summary>
    public bool isOnUI = false;

    private void Awake()
    {
        instance = this;
    }

    public static FCommandManager Instance()
    {
        return instance;
    }

    #region right_button_command

    private ERightCommandType rightCommand;
    public void SetRightButtonCommand(
        ERightCommandType rightCommand)
    {
        this.rightCommand |= rightCommand;
    }

    public void SignalCommand(ERightCommandType command)
    {
        rightCommand |= command;
    }


    private ECollectableEntity collectTarget = null;
    public void SignalCollectCommand(ECollectableEntity target)
    {
        SignalCommand(ERightCommandType.eCollect);
        collectTarget = target;
    }
    /// <SignalAttackCommand>
    /// 发射攻击指令和发射收集指令的逻辑类似，
    /// 具体参数的变量和数量由开发者自己决定
    /// </SignalAttackCommand>
    public void SignalAttackCommand(/* paramters... */)
    {
        SignalCommand(ERightCommandType.eAttack);
        /// some logic
    }

    #endregion

    private void LateUpdate()
    {
        //Debug.Log("Late Update!");
        if(EnumOperator.HasEnum(rightCommand,ERightCommandType.eCollect))
        {
            var selectedEntities = MSelectSystem.instance.selectedEntity;
            foreach (var ett in selectedEntities)
            {
                if (ett.ettType != EEntityType.Controlable) return;
                if (((AControlableActor)ett).HasProperty(EControlableProperties.Collect) == false)
                {
                    return;
                }

                var icc = ett.GetComponent<ICanCollect>();
                if (icc != null)
                {

                    icc.ChangeToCollectState(collectTarget);

                }
            }
        }
        else if(EnumOperator.HasEnum(rightCommand,ERightCommandType.eMove))
        {
            Vector2 pos = new Vector2();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // {x,y,2} = origin + direction * t;
            // oZ + dZ*t = 2;
            // t = (2-oZ)/dZ;
            float plane_pos_z = 0f;
            var t = (plane_pos_z - ray.origin.z) / ray.direction.z;
            pos.x = ray.origin.x + ray.direction.x * t;
            pos.y = ray.origin.y + ray.direction.y * t;


            var indexedPos = GridManager.GetIndexedPos(pos);
            var posId = GridManager.GetPosID(indexedPos);
            if (indexedPos.x < 0 || indexedPos.y < 0)
            {
                return;
            }


            if (posId != 0)
            {
                return;
            }

            MMoveSystem.instance.AddFlowFieldPathFinding(
                //GridManager.GetIndexedPos( CameraController.instance.GetMousePosByRay(0) ),
                indexedPos,
                MSelectSystem.instance.selectedEntity
                );

            foreach (var ett in MSelectSystem.instance.selectedEntity)
            {
                if (ett.TryGetComponent<ICanMove>(out var icm))
                {
                    icm.ChangeToMoveState();
                }
            }
        }
        else
        {

        }

        ResetCommond();
    }

    private void ResetCommond()
    {
        rightCommand = 0;
        collectTarget = null;
    }
}
