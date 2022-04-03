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

        public bool CanPick { get; set; } = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.Contains<Player.PlayerController>())
            {
                FindObjectOfType<GhostMouse>().EndDrag();
            }
        }

        public void EndDrag()
        {
            StartCoroutine(PickupDelayCoroutine());
        }

        private IEnumerator PickupDelayCoroutine()
        {
            yield return new WaitForSeconds(PickupDelay);
            CanPick = true;
        }
    }
}