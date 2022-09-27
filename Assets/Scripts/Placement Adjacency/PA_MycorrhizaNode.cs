using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_MycorrhizaNode : PA_Placeable
{
    protected override void Start()
    {
        base.Start();


        UpdateGFX();
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
        Debug.Log("FungusAdj: " + fungusAdj + ", PlantAdj: " + plantAdj);
        validPlacement = (fungusAdj && plantAdj);
    }
}
