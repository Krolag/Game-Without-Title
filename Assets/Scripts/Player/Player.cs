using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Throwable _objectToThrow;
    [SerializeField] private Transform _throwingPosition;
    [SerializeField] private float _throwStrenght;

    public void PickUp(Throwable throwObject)
    {
        if (throwObject == null)
            return;
        if (_objectToThrow != null)
        {
            _objectToThrow.transform.SetParent(null);

            Rigidbody rigidbody = _objectToThrow.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.detectCollisions = true;

            _objectToThrow.Throw(Camera.main.transform.forward, 0f);
            _objectToThrow = null;
        }
        Rigidbody rb = throwObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
        _objectToThrow = throwObject;
        throwObject.transform.position = _throwingPosition.position;
        throwObject.transform.SetParent(_throwingPosition, false);
        throwObject.transform.localPosition = Vector3.zero;

    }

    public void Throw()
    {
        if (_objectToThrow == null)
            return;

        _objectToThrow.transform.SetParent(null);

        Rigidbody rb = _objectToThrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;

        _objectToThrow.Throw(Camera.main.transform.forward, _throwStrenght);
        _objectToThrow = null;
    }
}
