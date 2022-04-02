using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
                toDrag.GetComponent<Draggable>().EndDrag();
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                {
                    if (hit.collider.transform.GetComponent<Draggable>() != null && hit.collider.transform.GetComponent<Draggable>().CanPick)
                    {
                        toDrag = hit.collider.transform;
                        toDrag.GetComponent<Draggable>().CanPick = false;
                        GhostMouse.StartDragging(hit);
                    }
                }
            }
        }
    }
}