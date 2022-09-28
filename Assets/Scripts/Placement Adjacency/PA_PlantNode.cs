using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_PlantNode : PA_AdjacencyNode
{
    Vector3 billboardOffset = new Vector3(2.9f, 0, 0);

    public enum PlantStage { Sappling, Grown, Decaying }
    public PlantStage stage;


    [SerializeField] protected float currentLifetime;
    [SerializeField] protected float expectedLifetime;

    [Range(0, 1)]
    public float progressRate;
    

    protected override void Start()
    {
        base.Start();
        lifetimeModifier = 1;
        progressRate = 1;
        currentLifetime = 0;
        expectedLifetime = Random.Range(lifetimeRange.x + (int)lifetimeModifier, lifetimeRange.y + (int)lifetimeModifier);

        growthRate = (float)Random.Range(0, 100) / 100f;
        growth = 1;
        growthStep = 3 / expectedLifetime;
        stage = PlantStage.Sappling;

        UpdateGFX();
        gfxMR.gameObject.transform.localPosition -= billboardOffset;

        StartCoroutine(ProgressThroughLifetime());
    }

    IEnumerator ProgressThroughLifetime() {
        yield return new WaitForFixedUpdate();
        //progress through lifetime
        if (currentLifetime < expectedLifetime) { 
            switch (stage) { 
                case PlantStage.Sappling:
                    //Apply positive growth
                    ApplyGrowth();
                    break;
                case PlantStage.Grown:
                    break;
                case PlantStage.Decaying:
                    //Apply negative growth
                    ApplyGrowth();
                    break;
            }
            currentLifetime += Time.deltaTime * progressRate;
            StartCoroutine(ProgressThroughLifetime());
        //lifetime is complete
        } else { 
            switch (stage) { 
                case PlantStage.Sappling:
                    Sustain();
                    break;
                case PlantStage.Grown:
                    Die();
                    break;
                case PlantStage.Decaying:
                    Decompose();
                    break;
            }
        }
    }

    void Sustain() {
        stage = PlantStage.Grown;
        currentLifetime = 0;
        StartCoroutine(ProgressThroughLifetime());
    }

    void Die() {
        currentLifetime = 0;
        expectedLifetime = Random.Range(decayTimeRange.x, decayTimeRange.y);
        stage = PlantStage.Decaying;

        growthRate = -growthRate;
        growthStep = 3 / expectedLifetime;

        UpdateGFX();
        gfxMR.gameObject.transform.localPosition += billboardOffset;
        StartCoroutine(ProgressThroughLifetime());
    }

    void Decompose() {
        nodeManager.plantNodes.Remove(this);
        nodeManager.adjacencyNodes.Remove(this);

        // check all other adjacency nodes and remove this from their list of adjacent nodes
        foreach (var node in adjacentNodes)
        {
            if (node.adjacentNodes.Contains(this))
            node.adjacentNodes.Remove(this);
        }

        Destroy(this.gameObject);
    } 

    void ApplyGrowth() {
        growth += growthRate * growthStep * Time.deltaTime;
        adjacencyRadius = growth * maxAdjRadius;
        UpdateGFX();
    }

    protected override void UpdateGFX() {
        base.UpdateGFX();

        if (stage == PlantStage.Decaying) {
            gfxMR.material = gfxMaterials[1];
            planeMR.material = planeMaterials[1];     
        } else {
            gfxMR.material = gfxMaterials[0];
            planeMR.material = planeMaterials[0];
        }
    }

    protected override void ProvideBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.lifetimeModifier *= 1.1f;
        }
        if (node.kingdom == PA_Taxonomy.Kingdom.Fungi) {
            node = node.GetComponent<PA_Placeable>();
        }
    }
    protected override void RemoveBenefit(PA_AdjacencyNode node)
    {
        if (node.kingdom == PA_Taxonomy.Kingdom.Plant) {
            node.lifetimeModifier /= 1.1f;
        }
    }
}

