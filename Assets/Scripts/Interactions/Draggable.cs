using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class Draggable : MonoBehaviour
    {
        [SerializeField]
        private float PickupDelay;

        public bool CanPick { get; set; } = true;

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