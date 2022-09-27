using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_AdjacencyNode : MonoBehaviour
{
    [Header("~~~ GFX Refs ~~~")]
    [SerializeField] protected GameObject gfx;
    protected MeshRenderer gfxMR;
    [SerializeField] protected Material[] gfxMaterials;

    [Header("~~~ Adjacency GFX Refs ~~~")]
    [SerializeField] public GameObject adjacencyPlane;
    protected MeshRenderer planeMR;
    [SerializeField] protected Material[] planeMaterials;

    [Header("~~~ Adjacency Factors ~~~")]
    public float adjacencyRadius;
    public float maxAdjRadius;
    public bool adjRadActive;
    public List<PA_AdjacencyNode> adjacentNodes;

    [Header("~~~ Growth Factors ~~~")]
    [Range(0, 1)]
    public float growthRate;
    [SerializeField] protected float growthStep;
    public float growth;

    protected virtual void Start() {

        gfxMR = gfx.GetComponentInChildren<MeshRenderer>();
        planeMR = adjacencyPlane.GetComponent<MeshRenderer>();

        adjacencyPlane.transform.localScale = new Vector3(adjacencyRadius / 5, 1, adjacencyRadius / 5);
    }

    public void ToggleRadiusDisplay() {
        if (adjRadActive) {
            planeMR.enabled = false;
            adjRadActive = false;
            }
        else {
            planeMR.enabled = true;
            adjRadActive = true;
        }
    }

    protected virtual void UpdateGFX() {       
            gfx.transform.localScale = new Vector3(growth, growth, growth);
            adjacencyPlane.transform.localScale = new Vector3(adjacencyRadius / 5, 1, adjacencyRadius / 5);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, adjacencyRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}