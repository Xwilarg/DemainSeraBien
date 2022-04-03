using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Extensions;

namespace LudumDare50
{
    public class DragScript : MonoBehaviour
    {
        private Transform toDrag;

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
                    if (hit.collider.transform.GetComponent<Draggable>() != null)
                    {
                        toDrag = hit.collider.transform;
                        GhostMouse.StartDragging(hit);
                    }
                    else if (hit.collider.gameObject.FindFittingParent(x => x.GetComponent<Draggable>() != null)) {
                        GameObject FittingParent = hit.collider.gameObject.FindFittingParent(x => x.GetComponent<Draggable>());
                        toDrag = FittingParent.transform;
                        GhostMouse.StartDragging(hit);
                    }
                }
            }
        }
    }
}