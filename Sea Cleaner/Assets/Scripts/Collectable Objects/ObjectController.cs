using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private WaterFloat _waterFloatScript;
    [SerializeField] private float _pullDurationTime;

    private Vector3 _zeroScale = new Vector3(0, 0, 0);
    private Vector3 _normalScale;

    private void OnEnable()
    {
        _normalScale = transform.localScale;

        MakeStatic();
    }

    public void MakeStatic()
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public void MakeHalfDynamic()
    {
        _rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    }

    public void MakeFullDynamic()
    {
        _rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.None;
    }

    public void CollectingProcess(Transform pullTransform, Transform dropTransform)
    {
        GameManager.instance.AddCollectedObjectCount();

        _rb.useGravity = true;
        _rb.mass = 0.1f;
        _capsuleCollider.height = 2;
        _capsuleCollider.center = new Vector3(0, 0, 0);
        _waterFloatScript.enabled = false;

        LeanTween.scale(gameObject, _zeroScale, _pullDurationTime);
        LeanTween.move(gameObject, pullTransform, _pullDurationTime).setOnComplete(() => 
        {
            MakeFullDynamic();
            transform.position = dropTransform.position;
            transform.localScale = _normalScale;
        });
    }

    public void ResetObject()
    {
        _rb.mass = 5f;
        _capsuleCollider.height = 10;
        _capsuleCollider.center = new Vector3(0, -2, 0);
        _waterFloatScript.enabled = true;
        MakeStatic();
    }
}
