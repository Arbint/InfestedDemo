using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    PerceptionStimuli mCurrentTargetStimuli = null;

    List<Sense> mSenses = new List<Sense>();

    LinkedList<PerceptionStimuli> mCurrentlyPerceievedStimulis = new LinkedList<PerceptionStimuli>();

    public delegate void OnTargetUpdated(GameObject target, bool wasSuccessfullySensed);
    public event OnTargetUpdated onTargetUpdated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mSenses.AddRange(GetComponents<Sense>());
        foreach (Sense sense in mSenses)
        {
            sense.onSenseUpdated += SenseUpdated;
        }
    }

    private void SenseUpdated(PerceptionStimuli stimuli, bool sensed)
    {
        UpdateStimuliRecord(stimuli, sensed);
        DetermineStimuli();
    }

    private void UpdateStimuliRecord(PerceptionStimuli stimuli, bool sensed)
    {
        //finding a node that has the stimuli O(n), same as List.
        LinkedListNode<PerceptionStimuli> foundStimuliNode = mCurrentlyPerceievedStimulis.Find(stimuli);
        if (sensed)
        {
            if (foundStimuliNode != null)
            {
                // insert right after the stimuli found O(1), it would be O(N) if using List
                mCurrentlyPerceievedStimulis.AddAfter(foundStimuliNode, stimuli);
            }
            else
            {
                mCurrentlyPerceievedStimulis.AddLast(stimuli);
            }
        }
        else
        {
            if (foundStimuliNode != null)
            {
                mCurrentlyPerceievedStimulis.Remove(foundStimuliNode);
            }
        }
    }

    private void DetermineStimuli()
    {
        if (mCurrentlyPerceievedStimulis.Count > 0)
        {
            PerceptionStimuli highestStimuli = mCurrentlyPerceievedStimulis.First.Value;
            if (highestStimuli != mCurrentTargetStimuli)
            {
                mCurrentTargetStimuli = highestStimuli;
                onTargetUpdated?.Invoke(mCurrentTargetStimuli.gameObject, true);
            }
        }
        else
        {
            if (mCurrentTargetStimuli != null)
            {
                onTargetUpdated?.Invoke(mCurrentTargetStimuli.gameObject, false);
                mCurrentTargetStimuli = null;
            }
        }
    }

    internal GameObject GetTarget()
    {
        if(mCurrentTargetStimuli)
        {
            return mCurrentTargetStimuli.gameObject;
        }
        return null;
    }
}
