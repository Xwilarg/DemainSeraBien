using LudumDare50.Player;
using UnityEngine;

namespace LudumDare50.Prop
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField]
        private Transform _output;

        [SerializeField]
        private GameObject[] _foods;

        [SerializeField]
        private int _count;

        private void Start()
        {
            PlayerController.Instance.FridgeLauncher = this;
        }

        public void Throw()
        {
            for (int i = 0; i < _count; i++)
            {
                var go = Instantiate(_foods[Random.Range(0, _foods.Length)], _output.transform.position, Random.rotation);
                if (go.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.AddForce((transform.position - _output.position).normalized * 10f
                                                                                          * Random.insideUnitCircle, ForceMode.Impulse);
                }
            }
        }
    }
}
