using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_MyceliumNode : PA_PlayerNode
{

    public float resourceDrain;

    protected override void Start() {
        base.Start();


        UpdateGFX();
    }


    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (placing) return;
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage != NodeStage.Decaying) {
                node.growthRate += .025f;
                node.expectedLifetime += 5;
                fungusManager.nutrientProduction += .025f;
            } else {
                node.growthRate -= .05f;
                fungusManager.nutrientProduction += .01f;
            }
            node.growthRate *= 1.1f;      
        }
    }

    public override void RemoveBenefit(PA_AdjacencyNode node) {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.growthRate /= 1.1f;

            if (node.stage != NodeStage.Decaying) {
                node.growthRate -= .025f;
                fungusManager.nutrientProduction -= .025f;
                node.expectedLifetime -= 5;
            }
            else {
                node.growthRate += .05f;
                fungusManager.nutrientProduction -= .01f;
            }
        }
    }

     protected override void ValidPlacementCheck() {
        validPlacement = false;
        if (nodeManager.fungusNodes.Count < 1) validPlacement = true;
        else { 
            foreach(PA_AdjacencyNode node in nodeManager.fungusNodes) {
                float radii = adjacencyRadius + node.adjacencyRadius;
                if (radii >= Vector3.Distance(transform.position, node.transform.position))
                    validPlacement =  true;          
            }
        }
    }
}
