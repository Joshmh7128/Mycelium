using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_MyceliumNode : PA_PlayerNode
{

    public float resourceDrain;
    [SerializeField] float growthMod, decayMod; 

    protected override void Start() {
        base.Start();


        UpdateGFX();
    }


    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (placing) return;
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            if (node.stage != NodeStage.Decaying) {
                node.growthRateMod += growthMod;
                fungusManager.nutrientProduction += production;
            } else {
                node.growthRateMod -= decayMod;
                fungusManager.nutrientProduction += production;
            }
            node.growthRateMod *= 1.1f;      
        }
    }

    public override void RemoveBenefit(PA_AdjacencyNode node) {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.growthRateMod /= 1.1f;

            if (node.stage != NodeStage.Decaying) {
                node.growthRateMod -= growthMod;
                fungusManager.nutrientProduction -= production;
            }
            else {
                node.growthRateMod += decayMod;
                fungusManager.nutrientProduction -= production;
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
