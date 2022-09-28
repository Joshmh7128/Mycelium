using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_AdjacencyNode : MonoBehaviour
{
    protected PA_PlayerController playerController;
    protected PA_NodeManager nodeManager; // our instanced node manager

    [Header("~~~ GFX Refs ~~~")]
    [SerializeField] protected GameObject gfx;
    protected MeshRenderer gfxMR;
    [SerializeField] protected Material[] gfxMaterials;

    [Header("~~~ Adjacency GFX Refs ~~~")]
    [SerializeField] public GameObject adjacencyPlane;
    protected MeshRenderer planeMR;
    [SerializeField] protected Material[] planeMaterials;

    [Header("~~~ Adjacency Factors ~~~")]
    public PA_Taxonomy.Kingdom kingdom;
    public PA_Taxonomy.Species species;
    public float adjacencyRadius;
    public float maxAdjRadius;
    public bool adjRadActive;
    public List<PA_AdjacencyNode> adjacentNodes;

    [Header("~~~ Growth Factors ~~~")]
    [SerializeField] protected Vector2 lifetimeRange;
    public float lifetimeModifier;
    [SerializeField] protected Vector2 decayTimeRange;

    [Range(0, 1)]
    public float growthRate;
    [SerializeField] protected float growthStep;
    public float growth;

    protected virtual void Start() {

        gfxMR = gfx.GetComponentInChildren<MeshRenderer>();
        planeMR = adjacencyPlane.GetComponent<MeshRenderer>();
        ToggleRadiusDisplay(false);
        // dividing by 5 because the plane mesh is 5 units long instead of 1 unit long
        adjacencyPlane.transform.localScale = new Vector3(adjacencyRadius / 5, 1, adjacencyRadius / 5);

        // get our nodemanager
        nodeManager = PA_NodeManager.instance;
        playerController = PA_PlayerController.instance;
    }

    public void ToggleRadiusDisplay(bool state) {
            planeMR.enabled = state;
            adjRadActive = state;
    }

    protected virtual void UpdateGFX() {       
        gfx.transform.localScale = new Vector3(growth, growth, growth);
        // dividing by 5 because the plane mesh is 5 units long instead of 1 unit long
        adjacencyPlane.transform.localScale = new Vector3(adjacencyRadius / 5, 1, adjacencyRadius / 5);
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(transform.position, adjacencyRadius);
        Gizmos.color = Color.green;
        // draw between nodes
        foreach (PA_AdjacencyNode node in adjacentNodes)
        {
            if (node)
            Gizmos.DrawLine(transform.position, node.transform.position);
        }

    }

    private void Update()
    {
        AdjacencyCheck();
    }

    // check to see if we are overlapping with any nodes
    protected virtual void AdjacencyCheck()
    {
        // go through the list of nodes
        foreach (PA_AdjacencyNode node in nodeManager.adjacencyNodes)
        {
            /// check to add nodes
            // add together our radius and the subject node's radius
            float radii = adjacencyRadius + node.adjacencyRadius; 
            // if that is more than the distance between them, we are overlapping
            if (radii >= Vector3.Distance(transform.position, node.transform.position))
            {
                if (!adjacentNodes.Contains(node) && node != this)
                {
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
    }
    
    protected virtual void ProvideBenefit(PA_AdjacencyNode node) { 
        
    }

    protected virtual void RemoveBenefit(PA_AdjacencyNode node) { 
    
    }
}