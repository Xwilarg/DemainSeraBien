using System.Collections.Generic;
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

        public int MoneyAvailable { get; set; } = 0;

        public int FinalAge { set; get; }

        private List<int> _idGot = new();

        public void AddId(int id)
        {
            _idGot.Add(id);
        }

        public bool ContainsId(int id)
        {
            return _idGot.Contains(id);
        }
    }
}
