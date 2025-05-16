using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public int curRound = 0;
    public float time;
    public float baseRoundGap = 10.0f;

    // ui
    public Image coldDownBar;
    public TextMeshProUGUI coldDownTime;
    private void LateUpdate()
    {
        if(time >=0.0f)
        {
            time-=Time.deltaTime;
        }
        else
        {
            // info spawn enemy
            var sp = Layer.currentLayer.spawnPoints;

            // temp design
            var _1stSp = sp[0];
            int count = SpawnInfo.instance.spawnBounds.Count;
            if(curRound < count)
            {
                var bound = SpawnInfo.instance.spawnBounds[curRound];
                _1stSp.MakeSpawnPlan(bound.spawnBatches);
            }

            time = baseRoundGap;
            curRound++;
        }

        //coldDownBar.fillAmount = time/baseRoundGap;

    }
}
