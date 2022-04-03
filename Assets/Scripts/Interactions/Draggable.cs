using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extensions;

namespace LudumDare50
{
    public class Draggable : MonoBehaviour
    {
        [SerializeField]
        private float PickupDelay;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.Contains<Player.PlayerController>())
            {
                FindObjectOfType<GhostMouse>().EndDrag();
            }
        }
    }
}