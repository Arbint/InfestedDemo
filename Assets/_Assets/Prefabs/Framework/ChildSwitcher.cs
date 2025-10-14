using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSwitcher : MonoBehaviour
{
    List<GameObject> mChildObjects = new List<GameObject>();
    void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            mChildObjects.Add(childTransform.gameObject);
        }

        SetActiveChildByIndex(0);
    }

    // int FindIndex(GameObject gameObjectToFind, Predicate<GameObject> pred)
    // {
    //     for (int i = 0; i < mChildObjects.Count; ++i)
    //     {
    //         if (pred(mChildObjects[i]))
    //         {
    //             return i;
    //         }
    //     }

    //     return -1;
    // }

    public void SetActiveChild(GameObject newActiveChild)
    {
        Predicate<GameObject> pred = (x) => { return x == newActiveChild; };
        int childIndex = mChildObjects.FindIndex(pred);

        SetActiveChildByIndex(childIndex);
    }

    public void SetActiveChildByIndex(int index)
    {
        if (index < 0 || index >= mChildObjects.Count)
            return;

        foreach (GameObject child in mChildObjects)
        {
            child.SetActive(false);
        }

        mChildObjects[index].SetActive(true);
    }
}
