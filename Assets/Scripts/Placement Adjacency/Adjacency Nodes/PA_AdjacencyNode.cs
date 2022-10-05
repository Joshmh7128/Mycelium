using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PA_AdjacencyNode : MonoBehaviour
{
    protected PA_PlayerController playerController;
    protected PA_NodeManager nodeManager; // our instanced node manager

    [Header("~~~ GFX Refs ~~~")]
    [SerializeField] protected GameObject gfx;
    protected MeshRenderer gfxMR;
    [SerializeField] protected Material[] gfxMaterials;
    [SerializeField] public GameObject adjacencyPlane;
    protected MeshRenderer planeMR;
    [SerializeField] protected Material[] planeMaterials;

    /*
    [Header("~~~ Stat Display UI ~~~")]
    public GameObject canvas;
    [SerializeField] Text lifetimeText;
    [SerializeField] Slider lifetimeSlider;
    */

    [Header("~~~ Adjacency Factors ~~~")]
    public PA_Taxonomy.Kingdom kingdom;
    public PA_Taxonomy.Species species;
    public float adjacencyRadius;
    public float maxAdjRadius;
    public bool adjRadActive;
    public List<PA_AdjacencyNode> adjacentNodes;
    [SerializeField] protected float timeTick = .12f;

    [Header("~~~ Lifetime Factors ~~~")]
    public Vector2 lifetimeRange;
    public float currentLifetime, expectedLifetime;

    [Header("~~~ Growth Factors ~~~")]
    public float growth;
    [Range(-1, 1)] public float growthRate;
    [SerializeField] public float maxGrowth;    
    [SerializeField] public float growthStep;
    public enum NodeStage { Growing, Sustaining, Decaying }
    public NodeStage stage;

    protected virtual void Start() {
        // get instanced refs
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
        if (PA_PlayerController.instance) playerController = PA_PlayerController.instance;

        //get components from serialized objects
        gfxMR = gfx.GetComponentInChildren<MeshRenderer>();
        planeMR = adjacencyPlane.GetComponent<MeshRenderer>();
        ToggleRadiusDisplay(false);
        // dividing by 5 because the plane mesh is 5 units long instead of 1 unit long
        
        EnterGrowing();

        StartCoroutine(AdjacencyCheck());
        StartCoroutine(ProgressThroughLifetime());
    }

    #region --- ADJACENCY FUNCTIONS ---
    // check to see if we are overlapping with any nodes    
    protected virtual IEnumerator AdjacencyCheck()
    {
        yield return new WaitForSecondsRealtime(1/10);
        // loop through all nodes
        foreach (PA_AdjacencyNode node in nodeManager.adjacencyNodes) {
            // compare sum of two node's radius to their distance apart
            float radii = adjacencyRadius + node.adjacencyRadius;
            if (radii >= Vector3.Distance(transform.position, node.transform.position)) {
                //exclude this node and nodes already in adjacent node list
                if (!adjacentNodes.Contains(node) && node != this) {
                    adjacentNodes.Add(node);
                    ProvideBenefit(node);
                }
            }
            /// check to remove nodes
            else
            {   // run the opposite check but see if that node exists in our list
                if (adjacentNodes.Contains(node))
                {
                    adjacentNodes.Remove(node);
                    RemoveBenefit(node);
                }
            }
        }
        StartCoroutine(AdjacencyCheck());
    }


    //overwrite for inherited class varience
    protected virtual void ProvideBenefit(PA_AdjacencyNode node) {}

    public virtual void RemoveBenefit(PA_AdjacencyNode node) {}

    #endregion

    #region --- LIFETIME FUNCTIONS ---
    protected virtual IEnumerator ProgressThroughLifetime() {
        yield return new WaitForSecondsRealtime(timeTick);
        //progress through lifetime
        if (currentLifetime < expectedLifetime) { 
            switch (stage) { 
                case NodeStage.Growing:
                    //Apply positive growth
                    ApplyGrowth(); currentLifetime += timeTick; break;
                case NodeStage.Sustaining:
                    currentLifetime += timeTick; break;
                case NodeStage.Decaying:
                    //Apply negative growth
                    ApplyGrowth(); expectedLifetime = growth / 1; break;
            }
            

        //lifetime is complete
        } else {
            switch (stage) { 
                case NodeStage.Growing:
                    EnterSustain(); break;
                case NodeStage.Sustaining:
                    EnterDecay(); break;
                case NodeStage.Decaying:
                    Decompose(); break;
            }
        }
        StartCoroutine(ProgressThroughLifetime());
    }

    protected virtual void ApplyGrowth() {
        growth += growthRate * growthStep * timeTick;
        if (growth > maxGrowth) growth = maxGrowth;
        if (growth < 1) 
            Decompose();

        adjacencyRadius = growth * maxAdjRadius;
        
        UpdateGFX();
    }

    protected virtual void EnterGrowing() {
        stage = NodeStage.Growing;
        growthRate = (float)Random.Range(05, 90) / 100f;
        expectedLifetime = (float)Random.Range((int)lifetimeRange.x, (int)lifetimeRange.y);
        growthStep = maxGrowth / expectedLifetime * growthRate;

        UpdateGFX();
    }

    protected virtual void EnterSustain() {
        stage = NodeStage.Sustaining;
        currentLifetime = 0;

        UpdateGFX();
    }

    protected virtual void EnterDecay() {
        stage = NodeStage.Decaying;
        currentLifetime = 1;

        growthRate = -growthRate;

        UpdateGFX();
    }

    //Destroy this node
    protected virtual void Decompose() {
        if (nodeManager.plantNodes.Contains(this)) nodeManager.plantNodes.Remove(this);
        if (nodeManager.fungusNodes.Contains(this)) nodeManager.fungusNodes.Remove(this);
        nodeManager.adjacencyNodes.Remove(this);

        // check all other adjacency nodes and remove this from their list of adjacent nodes
        for (int i = adjacentNodes.Count - 1; i >= 0; i--) { 
            RemoveBenefit(adjacentNodes[i]);
            if (adjacentNodes[i].adjacentNodes.Contains(this)) {
                adjacentNodes[i].adjacentNodes.Remove(this);
                adjacentNodes[i].RemoveBenefit(this);
            }
        }

        Destroy(gameObject);
    }

    #endregion

    //
    public void ToggleRadiusDisplay(bool state) {
        planeMR.enabled = state;
        adjRadActive = state;
    }

    protected virtual void UpdateGFX() {       
        gfx.transform.localScale = new Vector3(growth, growth, growth);
        // dividing by 5 because the plane mesh is 5 units long instead of 1 unit long
        adjacencyPlane.transform.localScale = new Vector3(adjacencyRadius / 5, 1, adjacencyRadius / 5);
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawWireSphere(transform.position, adjacencyRadius);
        Gizmos.color = Color.green;
        // draw between nodes
        foreach (PA_AdjacencyNode node in adjacentNodes) {
            if (node)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

}