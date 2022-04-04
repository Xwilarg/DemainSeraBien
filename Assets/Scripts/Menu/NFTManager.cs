using UnityEngine;

namespace LudumDare50.Menu
{
    public class NFTManager : MonoBehaviour
    {
        private static NFTManager _instance;
        public static NFTManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("NFT Manager", typeof(NFTManager));
                    DontDestroyOnLoad(go);
                    _instance = go.GetComponent<NFTManager>();
                }
                return _instance;
            }
        }

        public int MoneyAvailable { get; private set; } = 0;
    }
}
