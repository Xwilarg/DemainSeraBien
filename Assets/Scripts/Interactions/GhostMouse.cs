using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare50
{
    public class GhostMouse : MonoBehaviour
    {
        [SerializeField]
        private Vector3 PlaneSpaceHeight;

        private SpringJoint Joint;
        private LineRenderer lineRenderer;
        private Plane PositionPlane;

        public Rigidbody DraggingBody => Joint.connectedBody;


        private void Start()
        {
            PositionPlane = new Plane(Vector3.up, PlaneSpaceHeight);
            Joint = GetComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (PositionPlane.Raycast(ray, out float Enter))
            {
                Vector3 hitPoint = ray.GetPoint(Enter);
                transform.position = hitPoint;
            }

            if (Joint.connectedBody != null) {
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.SetPosition(0, Joint.connectedBody.transform.localToWorldMatrix.MultiplyPoint(Joint.connectedAnchor));
            }
        }

        public void StartDragging(Rigidbody connectedBody, Vector3 connectedAnchor)
        {
            Joint.connectedBody = connectedBody;
            Joint.connectedAnchor = connectedAnchor;
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(new Vector3[] {transform.position, connectedBody.transform.localToWorldMatrix.MultiplyPoint(connectedAnchor)});
        }

        public void EndDrag()
        {
            Joint.connectedBody = null;
            lineRenderer.enabled = false;
        }
    }
}
