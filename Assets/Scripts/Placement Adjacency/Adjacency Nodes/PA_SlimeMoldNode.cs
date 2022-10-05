using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_SlimeMoldNode : PA_PlayerNode
{

    List<PA_AdjacencyNode> nodesInCoverage = new List<PA_AdjacencyNode>();


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
                node.growthRate -= .333f;
                node.growthRate *= 1.1f;
                fungusManager.nutrientProduction += .25f;
                nodesInCoverage.Add(node);
            }
        }
    }

    public override void RemoveBenefit(PA_AdjacencyNode node)
    {
        if (nodesInCoverage.Contains(node)) {
            growthRate -= .125f;
            node.growthRate /= 1.1f;
            node.growthRate += .333f;

            fungusManager.nutrientProduction -= .25f;
            nodesInCoverage.Remove(node);
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
