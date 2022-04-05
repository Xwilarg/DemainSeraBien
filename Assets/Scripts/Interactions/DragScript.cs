using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace LudumDare50
{
    public class DragScript : MonoBehaviour
    {
        public static DragScript Instance { get; private set; }

        [FormerlySerializedAs("GhostMouse")]
        [SerializeField]
        private GhostMouse m_GhostMouse;

        public GhostMouse GhostMouse => m_GhostMouse;


        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                m_GhostMouse.EndDrag();
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                {
                    if (hit.rigidbody != null && hit.rigidbody.TryGetComponent(out Draggable _))
                    {
                        Vector3 connectedAnchor = hit.rigidbody.transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                        m_GhostMouse.StartDragging(hit.rigidbody, connectedAnchor);
                    }
                }
            }
        }

        public void EndDrag() => GhostMouse.EndDrag();
    }
}
