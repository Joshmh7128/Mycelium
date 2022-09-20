using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootClass : PlantClass
{
    /// purpose of this is to build a root system out from our plant
    /// right now the goal is to expand as far as you can in ?any? direction
    /// node to node root pathfinding, check if there is anything in the way at each root build
    /// 

    [SerializeField] protected GameObject branchNodePrefab; // the prefab we use to build root nodes
    [SerializeField] protected List<Branch> branchList = new List<Branch>(); // our list of nodes
    [SerializeField] protected GameObject activeNode, originNode; // the node we are building off of

    [SerializeField] protected float branchLength, branchY;

    public virtual void Start() 
    {
        branchList.Add(originNode.GetComponent<Branch>());
        GrowStep();
    }

    // with every step we must surely be learning... 
    public override void ExecuteStep()
    {
        GrowStep();
    }

    // our function for seeking out our next node
    void GrowStep()
    {
        Debug.Log("running");

        // get a direction away from our origin node based off of 
        Branch growingBranch = FindShortestBranch();
        GameObject growingBranchGO = growingBranch.GetComponent<GameObject>();
        // rotate around this point and find our new path
        for (int r = 0; r <= 360; r += 15) 
        {
            // every 15 degrees check in the direction of r by the branch length, and see if there is a collider there, bub 
            Quaternion rot =  Quaternion.Euler(0, r, 0); // make an euler angle
            // perform a sphere check in each direction
            Vector3 checkPos = (rot * Vector3.forward).normalized * branchLength + Vector3.up*branchY;
            // check the dir position for anything in the way
            Collider[] obstacles = Physics.OverlapSphere(checkPos, 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            // if there's nothing in the way
            if (obstacles.Length < 1)
            {
                // make a new node in the branch
                Branch branch = Instantiate(branchNodePrefab, checkPos, Quaternion.identity, null).GetComponent<Branch>();
                branchList.Add(branch);
            }

        }
    }

    Branch FindShortestBranch() 
    {
        foreach(Branch b in branchList) 
        { 
            if (b.roomToGrow) 
            {
                return b;
            }
        }
        return null;
    }
}
