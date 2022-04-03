using UnityEngine;

namespace LudumDare50.Prop
{
    public class Roomba : MonoBehaviour
    {
        private Rigidbody _rb;

        private float speed = 100f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Vector3.Dot(transform.up, Vector3.down) <= 0
                && transform.position.y < 0f)
            {
                Debug.Log("Forward");
                _rb.velocity = new Vector3(transform.forward.x * speed, _rb.velocity.y, transform.forward.z * speed);
            }
        }
    }
}
