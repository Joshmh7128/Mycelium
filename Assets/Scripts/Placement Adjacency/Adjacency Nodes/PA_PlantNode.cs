using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_PlantNode : PA_AdjacencyNode
{
    Vector3 billboardOffset = new Vector3(2.9f, 0, 0);


    protected override void Start()
    {
        base.Start();


        UpdateGFX();
        gfxMR.gameObject.transform.localPosition -= billboardOffset;

        StartCoroutine(ProgressThroughLifetime());
    }

    protected override void EnterSustain() {
        stage = NodeStage.Sustaining;
        currentLifetime = 0;
        StartCoroutine(ProgressThroughLifetime());
    }

    protected override void EnterDecay() {
        //stage = NodeStage.Decaying;
        //currentLifetime = 0;
        //expectedLifetime = Random.Range(lifetimeRange.x, lifetimeRange.y);

        //growthStep = maxGrowth / expectedLifetime;


        //UpdateGFX();
        
        
        for (int i = adjacentNodes.Count - 1; i >= 0; i--) {
            if (adjacentNodes[i].adjacentNodes.Contains(this)) 
                adjacentNodes[i].adjacentNodes.Remove(this);
            adjacentNodes.Remove(adjacentNodes[i]);
        }           
        base.EnterDecay();

        growthRate = -.125f;
        gfxMR.gameObject.transform.localPosition += billboardOffset;
        UpdateGFX();
    }

    //Destroy this node
    protected override void Decompose() {
        //nodeManager.plantNodes.Remove(this);
        //nodeManager.adjacencyNodes.Remove(this);

        //// check all other adjacency nodes and remove this from their list of adjacent nodes
        //foreach (var node in adjacentNodes) {
        //    RemoveBenefit(node);
        //    if (node.adjacentNodes.Contains(this)) { 
        //        node.adjacentNodes.Remove(this);
        //        node.RemoveBenefit(this);
        //    }
        //}

        //Destroy(gameObject);
        base.Decompose();
    } 


    protected override void UpdateGFX() {
        base.UpdateGFX();

        if (stage == NodeStage.Decaying) {
            gfxMR.material = gfxMaterials[1];
            planeMR.material = planeMaterials[1];     
        } else {
            gfxMR.material = gfxMaterials[0];
            planeMR.material = planeMaterials[0];
        }
    }
    /*
    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.lifetimeModifier *= 1.1f;
        }
        if (node.kingdom == PA_Taxonomy.Kingdom.Fungi) {
            node = node.GetComponent<PA_PlayerNode>();
        }
    }
    public override void RemoveBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.lifetimeModifier /= 1.1f;
        }
    }
    */
}

