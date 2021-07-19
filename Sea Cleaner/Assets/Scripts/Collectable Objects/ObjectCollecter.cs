using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollecter : MonoBehaviour
{
    [SerializeField] private Transform _objectPullPointTransform;
    [SerializeField] private Transform _objectDropPointTransform;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("CollectableObject"))
        {
            col.GetComponent<ObjectController>().CollectingProcess(_objectPullPointTransform, _objectDropPointTransform);
        }
    }
}
