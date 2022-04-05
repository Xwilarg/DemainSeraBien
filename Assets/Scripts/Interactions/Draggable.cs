using System;
using UnityEngine;

namespace LudumDare50
{
    public class Draggable : MonoBehaviour
    {
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody != null && collision.rigidbody.CompareTag("Player") &&
                DragScript.Instance.GhostMouse.DraggingBody == _rigidbody)
            {
                DragScript.Instance.EndDrag();
            }
        }
    }
}
