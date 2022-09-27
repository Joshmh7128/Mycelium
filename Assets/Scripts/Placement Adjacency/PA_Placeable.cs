using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Placeable : PA_AdjacencyNode
{

    [Header("~~~ Placable Refs ~~~")]
    PA_PlayerController playerController;
    [SerializeField] public Collider[] colliders;

    [SerializeField] Material[] invalidPlaceMaterials;
    public bool placing;

    protected override void Start()
    {
        if (PA_PlayerController.instance) playerController = PA_PlayerController.instance;
        placing = false;
        base.Start();
    }

    public void Place() {
        placing = true;
        foreach (Collider col in colliders) col.enabled = false;
        
        StartCoroutine(WhilePlacing());
    }

    public void ConfirmPlace() {
        placing = false;
        foreach (Collider col in colliders) col.enabled = true;

    }

    IEnumerator WhilePlacing() {
        yield return new WaitForEndOfFrame();
        ToggleRadiusDisplay();
        while (placing)
        {
            yield return new WaitForEndOfFrame();
            ToggleValidVisuals();
        }
        ToggleRadiusDisplay();
    }

    void ToggleValidVisuals() { 
        if (playerController.placementValid) {
            gfxMR.material = gfxMaterials[0];
            adjacencyPlane.GetComponent<MeshRenderer>().material = planeMaterials[0];
        } else {
            gfxMR.material = invalidPlaceMaterials[0];
            adjacencyPlane.GetComponent<MeshRenderer>().material = invalidPlaceMaterials[1];
        }
    }

}
