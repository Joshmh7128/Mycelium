using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantClass : MonoBehaviour
{
    /// purpose of this script is to build all of the fundamental values and behaviours derrived
    /// from the other plants in the game
    /// these values express the average intake of each value of the plant over the course of an entire year

    // our sunlight category
    // sunlight at sunlightMax is that the plant sat in the sun for the entire year

    [Header(" ~~ Sunlight ~~ ")]
    [SerializeField] protected float sunlight; // our current sunlight amount, and our maximum sunlight intake
    [SerializeField] protected float sunlightMax; // our current sunlight amount, and our maximum sunlight intake
    [SerializeField] protected float phosphorus, nitrogen; // based on the amount of phosphorus and nitrogen we have, we can increase the sunlight max

    // water
    [Header(" ~~ Water ~~ ")]
    [SerializeField] protected float water; // the amount of water we've took in throughout the year, how much we can in total
    [SerializeField] protected float waterMax; // the amount of water we've took in throughout the year, how much we can in total
    [SerializeField] protected float immediateWaterClimate; // growth refers to the amount of roots it has, immediateClimate refers to the amount of water nearby
                                                            // growth also influences how much water this intakes - compare growth to the immediate climate to see if this can be fulfilled

    // carbon dioxide
    [Header(" ~~ Carbon Dioxide ~~ ")]
    [SerializeField] protected float carbonDioxide; // the amount of carbon we are intaking and the maximum amount we can intake
    [SerializeField] protected float carbonDioxideMax; // the amount of carbon we are intaking and the maximum amount we can intake
    [SerializeField] protected float nutrients, immediateCarbonDioxideClimate; // draws on growth as well

    // our production values
    [Header(" ~~ Production Values ~~ ")]
    [SerializeField] protected float oxygen;
    [SerializeField] protected float carbon, growth, offspring;

    // time management
    [Header(" ~~ Time Management ~~ ")]
    [Tooltip("The Epsilon is the current amount of time we are adding to to reach the Gamma")]
    [SerializeField] protected float processEpsilon; // the length of the year in seconds

    [Tooltip("The Gamma is the step length")]
    [SerializeField] protected float processGamma; // the length of the year in seconds

    [Tooltip("The Delta is the amount that we add to the epsilon each frame")]
    [SerializeField] protected float processDelta; // the length of the year in seconds

    // photosynthesis function, happens once every year
    [SerializeField]
    protected virtual void PhotoSynthesis()
    {
        // take water, sunlight, and carbon dioxide, then turn it into oxygen, growth, and biproducts

        Debug.Log("it has been " + processGamma + " seconds");
    }

    // step timestep function
    [SerializeField]
    protected virtual void ExecuteStep()
    {
        processDelta = Time.deltaTime; // make sure that every second processDelta = 1

        // add up to the delta
        if (processEpsilon < processGamma)
        {
            processEpsilon += processDelta;
        }

        if (processEpsilon > processGamma)
        {
            ProcessStep();
            processEpsilon = 0;
        }
    }

    // process step
    [SerializeField]
    protected virtual void ProcessStep()
    {
        PhotoSynthesis();
    }

}
