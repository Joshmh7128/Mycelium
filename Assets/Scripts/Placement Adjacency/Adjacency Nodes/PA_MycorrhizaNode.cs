using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_MycorrhizaNode : PA_PlayerNode
{
    protected override void Start()
    {
        base.Start();


        UpdateGFX();
    }
    
    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (placing) return;
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage != NodeStage.Decaying) {
                node.maxGrowth += 1;
                node.growthRate += .05f;
                node.expectedLifetime += 5;
                node.growthStep = maxGrowth / (expectedLifetime - currentLifetime) * growthRate;
                fungusManager.nutrientProduction += .1f;
            }
            else node.growthRate -= .01f;
        }
    }

    public override void RemoveBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage != NodeStage.Decaying) {
                node.maxGrowth -= 1;
                node.growthRate -= .05f;
                node.expectedLifetime -= 5;
                float remainingLife = (expectedLifetime - currentLifetime > 0) ? expectedLifetime - currentLifetime : .01f;
                node.growthStep = maxGrowth / remainingLife * growthRate;
                fungusManager.nutrientProduction -= .1f;
            } else node.growthRate += .01f;
        }
    }   
    protected override void ValidPlacementCheck() {
        bool fungusAdj = false;
        bool plantAdj = false;
        foreach(PA_AdjacencyNode node in nodeManager.adjacencyNodes) {
            float radii = adjacencyRadius + node.adjacencyRadius;
            if (radii >= Vector3.Distance(transform.position, node.transform.position))
            {
                if (node.kingdom == PA_Taxonomy.Kingdom.Fungi) fungusAdj = true;
                if (node.kingdom == PA_Taxonomy.Kingdom.Plant) plantAdj = true;
            }
        }
        validPlacement = (fungusAdj && plantAdj);
    }
}
