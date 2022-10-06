using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_SlimeMoldNode : PA_PlayerNode
{

    protected override void Start() {
        base.Start();

        UpdateGFX();
    }



    public override void ConfirmPlace() {
        EnterDecay();
        growthRate = -.25f;
        base.ConfirmPlace();
    }

    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (placing) return;
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage == NodeStage.Decaying) {
                growthRate += .125f;
                node.growthRate -= .5f;
 
                fungusManager.nutrientProduction += .25f;
            }
        }
    }

    public override void RemoveBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage == NodeStage.Decaying) { 
                growthRate -= .125f;

                node.growthRate += .5f;

                fungusManager.nutrientProduction -= .25f;
            }
        }
    }

    protected override void ValidPlacementCheck() {
        validPlacement = false;
        
        foreach(PA_AdjacencyNode node in nodeManager.plantNodes) {
            if (node.stage == NodeStage.Decaying) { 
                float radii = adjacencyRadius + node.adjacencyRadius;
                if (radii >= Vector3.Distance(transform.position, node.transform.position)) 
                    validPlacement = true;
            }                 
        }
        
    }

}