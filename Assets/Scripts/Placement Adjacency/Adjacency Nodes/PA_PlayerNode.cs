using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_PlayerNode : PA_AdjacencyNode
{
    protected PA_FungusManager fungusManager;

    [Header("~~~ Placable Refs ~~~")]
 
    [SerializeField] protected float storage;
    [SerializeField] protected float consumption;

    public float cost;
    [SerializeField] public Collider[] colliders;

    [SerializeField] Material[] invalidPlaceMaterials;
    public bool placing;
    public bool validPlacement;

    protected override void Start() {

        placing = false;

        // get instanced refs
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
        if (PA_PlayerController.instance) playerController = PA_PlayerController.instance;
        if (PA_FungusManager.instance) fungusManager = PA_FungusManager.instance;

        //get components from serialized objects
        gfxMR = gfx.GetComponentInChildren<MeshRenderer>();
        planeMR = adjacencyPlane.GetComponent<MeshRenderer>();
        ToggleRadiusDisplay(false);

        EnterGrowing();
    }

    public void Place() {
        placing = true;
        foreach (Collider col in colliders) col.enabled = false;
        
        StartCoroutine(WhilePlacing());
    }

    public virtual void ConfirmPlace() {
        placing = false;
        foreach (Collider col in colliders) col.enabled = true;

        fungusManager.nutrientStorage += storage;
        fungusManager.nutrientConsumption += consumption;

        StartCoroutine(AdjacencyCheck());
        StartCoroutine(ProgressThroughLifetime());
    }

    IEnumerator WhilePlacing() {
        validPlacement = false;
        yield return new WaitForEndOfFrame();
        ToggleRadiusDisplay(true);
        while (placing) {
            yield return new WaitForEndOfFrame();
            ValidPlacementCheck();
            ToggleValidVisuals();
        }
    }

    //overwrite for placement requirements
    protected virtual void ValidPlacementCheck() {
        validPlacement = true;
    }

    void ToggleValidVisuals() { 
        if (validPlacement) {
            gfxMR.material = gfxMaterials[0];
            planeMR.material = planeMaterials[0];
        } else {
            gfxMR.material = invalidPlaceMaterials[0];
            planeMR.material = invalidPlaceMaterials[1];
        }
    }

    protected override void Decompose() {
        fungusManager.nutrientConsumption -= consumption;
        fungusManager.nutrientStorage -= storage;

        base.Decompose();
    }
}
