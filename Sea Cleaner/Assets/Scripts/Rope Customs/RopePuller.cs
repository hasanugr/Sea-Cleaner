using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePuller : MonoBehaviour
{
    public enum ForceWays { Up, Down, Left, Right, Forward, Back };
	public ForceWays forceWay;
	public float force = 100;
	public float forcePullingMultiple = 3;
    public bool IsPullingMode;

    private Rigidbody _rb;
    private Vector3 _forceVector;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        switch (forceWay)
        {
            case ForceWays.Up:
                _forceVector = Vector3.up;
                break;
            case ForceWays.Down:
                _forceVector = Vector3.down;
                break;
            case ForceWays.Left:
                _forceVector = Vector3.left;
                break;
            case ForceWays.Right:
                _forceVector = Vector3.right;
                break;
            case ForceWays.Forward:
                _forceVector = Vector3.forward;
                break;
            case ForceWays.Back:
                _forceVector = Vector3.back;
                break;
            default:
                _forceVector = Vector3.down;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsPullingMode)
        {
            _rb.AddForce(_forceVector * (force * forcePullingMultiple));
        }else
        {
            _rb.AddForce(_forceVector * force);
        }
    }

}
