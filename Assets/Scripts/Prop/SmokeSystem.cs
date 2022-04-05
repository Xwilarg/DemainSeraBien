using LudumDare50.Player;
using UnityEngine;

namespace LudumDare50.Prop
{
    public class SmokeSystem : MonoBehaviour
    {
        private void Start()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Stop();
            PlayerController.Instance.SmokeSystem = particleSystem;
        }
    }
}
