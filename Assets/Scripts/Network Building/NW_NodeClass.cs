using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_NodeClass : MonoBehaviour
{
    /// prototype node class
    /// this node holds passengers to be sent outwards by our emitters
    /// 

    public List<NW_AgentClass> idlePassengers = new List<NW_AgentClass>(); // the passengers waiting to depart
    public List<NW_EmitterClass> emitters = new List<NW_EmitterClass>(); // all of our emitters


    
}
