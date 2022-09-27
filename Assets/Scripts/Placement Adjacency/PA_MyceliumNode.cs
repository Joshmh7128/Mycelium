using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_MyceliumNode : PA_Placeable, PA_AdjacencyNode
{

    public float resourceDrain;

    protected override void Start() {
        base.Start();


        UpdateGFX();
    }




}
