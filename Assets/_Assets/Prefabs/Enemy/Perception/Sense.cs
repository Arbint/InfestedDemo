using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour
{
    static HashSet<PerceptionStimuli> registeredStimulis = new HashSet<PerceptionStimuli>();
    public static void RegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Add(stimuli);
    }

    public static void UnReisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Remove(stimuli);
    }

    [SerializeField] bool mDrawDebug = true;
    [SerializeField] float mForgettingDuration = 4;
    private HashSet<PerceptionStimuli> mPerceivedStimulis = new HashSet<PerceptionStimuli>();
    private Dictionary<PerceptionStimuli, Coroutine> mCurrentlyForgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>();

    protected abstract bool IsStimuliSensed(PerceptionStimuli stimuli);

    public delegate void OnSenseUpdated(PerceptionStimuli stimuli, bool sensed);
    public event OnSenseUpdated onSenseUpdated;

    // Update is called once per frame
    void Update()
    {
        foreach (PerceptionStimuli stimuli in registeredStimulis)
        {
            if (IsStimuliSensed(stimuli))
            {
                TryAddSensedStimuli(stimuli);
            }
            else
            {
                if (mPerceivedStimulis.Contains(stimuli))
                {
                    mPerceivedStimulis.Remove(stimuli);
                    if (!mCurrentlyForgettingRoutines.ContainsKey(stimuli))
                    {
                        mCurrentlyForgettingRoutines.Add(stimuli, StartCoroutine(StartForgetStimuli(stimuli)));
                    }
                }
            }
        }
    }

    private IEnumerator StartForgetStimuli(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(mForgettingDuration);
        mCurrentlyForgettingRoutines.Remove(stimuli);
        Debug.Log($"I just lost track of {stimuli.gameObject}");
        onSenseUpdated?.Invoke(stimuli, false);
    }

    protected void TryAddSensedStimuli(PerceptionStimuli stimuli)
    {
        if (!mPerceivedStimulis.Contains(stimuli))
        {
            if (mCurrentlyForgettingRoutines.TryGetValue(stimuli, out Coroutine forgettingCoroutine))
            {
                StopCoroutine(forgettingCoroutine);
                mCurrentlyForgettingRoutines.Remove(stimuli);
            }
            else
            {
                Debug.Log($"I just sensed: {stimuli.gameObject}");
                onSenseUpdated?.Invoke(stimuli, true);
            }

            mPerceivedStimulis.Add(stimuli);
        }
    }

    protected virtual void DrawDebug()
    {

    }

    private void OnDrawGizmos()
    {
        if (mDrawDebug)
        {
            DrawDebug();
        }
    }
}
