using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

    public Line joao;
    public Line maria;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            joao.DrawLine(BubbleSystem.Situation.GreaterReward);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            maria.DrawLine(BubbleSystem.Situation.GreaterReward);

    }
}
