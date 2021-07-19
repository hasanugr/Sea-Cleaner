using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    private Vector3 _dragScreenPoint;
    private Vector3 _dragOffset;
    private bool _isDraggable = true;
    private Vector3 _startPos = new Vector3(3, 0.5f, 0);

    void OnMouseDown()
    {
        if (_isDraggable)
        {
            _dragScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            _dragOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _dragScreenPoint.z));
        }
    }

    void OnMouseDrag()
    {
        if (_isDraggable)
        {
            _dragScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _dragScreenPoint.z);
            Vector3 curWorldPoint = Camera.main.ScreenToWorldPoint(curScreenPoint) + _dragOffset;

            // Y position will not change, Y position changing by waves..
            float yPos = Mathf.Clamp(transform.position.y, -0.25f, 0.25f);
            transform.position = new Vector3(curWorldPoint.x, yPos, curWorldPoint.z);
        }
    }

    public void ConnectedToRopeLocker(Vector3 lockedPos)
    {
        _isDraggable = false;
        _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        transform.position = lockedPos;

    }

    public void ResetPlayer()
    {
        _isDraggable = true;
        _rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        transform.localPosition = _startPos;
    }
}
