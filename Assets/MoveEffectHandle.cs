using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class MoveEffectHandle : SingletonBase<MoveEffectHandle>
{
    [Header("Triangle Function : A*Sin(W*x)")]
    // control pitch
    public float paramterA;
    // control w
    public float paramterW;
    public class MoveEffectTarget
    {
        public Entity actor;
        public ICanMove icm;
        public FTimer timer;
        public float lastSinVal = 0.0f;
    }

    public List<MoveEffectTarget> targets = new List<MoveEffectTarget>();
    public Queue<MoveEffectTarget>deleteQueue = new Queue<MoveEffectTarget>();
    public void ProccessEntity(Entity ett,ICanMove icm)
    {
        MoveEffectTarget target = new MoveEffectTarget();
        target.actor = ett;
        target.timer = new FTimer();
        target.timer.SetGap(-1.0f);
        target.icm = icm;
        targets.Add(target);
    }

    bool Check(object obj)
    {
        return obj != null;
    }

    protected override void OnUpdate()
    {
        foreach (MoveEffectTarget target in targets)
        {

           
            if (Check(target.actor) && Check(target.actor.rb2d))
            {

                target.timer.Timer();
                var time = target.timer.GetTime();

                float sinVal = paramterA * Mathf.Abs(Mathf.Sin(paramterW * time));
                float offset_y = sinVal - target.lastSinVal;
                target.lastSinVal = sinVal;

                var pos = target.actor.srObject.transform.localPosition;
                pos.y += offset_y;

                if (CompareFunction.is_same_float(sinVal,0.0f) && target.icm.iPathFinding == null)
                {
                    pos.y = 0.0f;
                    deleteQueue.Enqueue(target);
                }
                target.actor.srObject.transform.localPosition = pos;

            }
        }
        foreach(MoveEffectTarget target in deleteQueue)
        {
            targets.Remove(target);
        }
    }

}
