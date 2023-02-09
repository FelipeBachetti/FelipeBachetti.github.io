using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenRotation : MonoBehaviour
{
    private Quaternion lastParentRotation;
    void OnEnable()
    {
        transform.root.localRotation = new Quaternion(0f,0f,0f,0f);
        transform.localRotation = new Quaternion(0f,0f,0f,0f);
        lastParentRotation = transform.root.localRotation;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.root.localRotation) *
            lastParentRotation * transform.localRotation;

        lastParentRotation = transform.root.localRotation;

    }
}
