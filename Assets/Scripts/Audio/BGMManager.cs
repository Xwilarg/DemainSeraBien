using UnityEngine;

namespace LudumDare50.Audio
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager Instance { private set; get; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                GetComponent<AudioSource>().Play();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
