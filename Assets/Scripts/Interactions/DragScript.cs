using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Extensions;

namespace LudumDare50
{
    public class DragScript : MonoBehaviour
    {
        [SerializeField]
        private GhostMouse GhostMouse;

        private void Update()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                GhostMouse.EndDrag();
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                {
                    if (hit.rigidbody != null && hit.rigidbody.TryGetComponent(out Draggable draggable))
                    {
                        Vector3 connectedAnchor = hit.rigidbody.transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                        GhostMouse.StartDragging(hit.rigidbody, connectedAnchor);
                    }
                }
            }
        }
    }
}
