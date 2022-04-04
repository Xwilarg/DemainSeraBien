using UnityEngine;

namespace LudumDare50.Rendering
{
    public class RoomTrigger : MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraTargetPos;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CameraManager.Instance.MoveTo(_cameraTargetPos.position);
            }
        }
    }
}
