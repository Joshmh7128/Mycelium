using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuasiBehaviour : MonoBehaviour
{
    float QuasiTimeStep = 0.01f;
    [HideInInspector] public float QuasiDeltaTime = 0.01f; // the time since the last frame in static form

    // Start is called before the first frame update
    void Start()
    {
        // start quasitime
        StartCoroutine(QuasiTime());
        QuasiStart();
    }

    // the coroutine that runs our quasitime
    public IEnumerator QuasiTime()
    {
        QuasiUpdate();
        yield return new WaitForSecondsRealtime(QuasiTimeStep * Time.timeScale);
        QuasiLateUpdate();
        // run the time again
        StartCoroutine(QuasiTime());
    }

    // the coroutine that manages our fixed quasitime
    public IEnumerator FixedQuasiTime()
    {
        QuasiFixedUpdate();
        yield return new WaitForSecondsRealtime(QuasiTimeStep);
    }

    // our quasi behaviours
    public virtual void QuasiStart() { } // runs as the start for all quasi objects
    public virtual void QuasiUpdate() { } // runs as the update for all quasi objects
    public virtual void QuasiLateUpdate() { } // runs as the update for all quasi objects
    public virtual void QuasiFixedUpdate() { } // runs as the update for all quasi objects

}
