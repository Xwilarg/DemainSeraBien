using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(SpringJoint))]
public class Draggable : MonoBehaviour
{
    [SerializeField]
    private float PickupDelay;

    private SpringJoint Spring;

    public bool CanPick { get; set; } = true;

    private void Start()
    {
        Spring = GetComponent<SpringJoint>();
    }

    public void EndDrag()
    {
        Spring.anchor = Vector3.zero;
        Spring.connectedBody = null;
        StartCoroutine(PickupDelayCoroutine());
    }

    private IEnumerator PickupDelayCoroutine()
    {
        yield return new WaitForSeconds(PickupDelay);
        CanPick = true;
    }
}