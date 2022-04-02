using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare50
{
    public class GhostMouse : MonoBehaviour
    {
        [SerializeField]
        private Vector3 PlaneSpaceHeight;

        private SpringJoint Joint;
        private Plane PositionPlane;

        private void Start()
        {
            PositionPlane = new Plane(Vector3.up, PlaneSpaceHeight);
            Joint = GetComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (PositionPlane.Raycast(ray, out float Enter))
            {
                Vector3 hitPoint = ray.GetPoint(Enter);
                transform.position = hitPoint;
            }
        }

        public void StartDragging(RaycastHit hit)
        {
            Joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody>();
            Joint.connectedAnchor = hit.collider.transform.position;
        }

        public void EndDrag()
        {
            Joint.connectedBody = null;
        }
    }
}