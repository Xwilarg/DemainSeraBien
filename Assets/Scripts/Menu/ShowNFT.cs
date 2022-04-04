using UnityEngine;

namespace LudumDare50.Menu
{
    public class ShowNFT : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buyButton, _nftDisplay;

        [SerializeField]
        private int _id;

        private void Start()
        {
            if (NFTManager.Instance.ContainsId(_id))
            {
                Show();
            }
        }

        public void Buy()
        {
            if (NFTManager.Instance.MoneyAvailable >= 2)
            {
                NFTManager.Instance.MoneyAvailable -= 2;
                MainMenu.Instance.UpdateText();
                NFTManager.Instance.AddId(_id);
                Show();
            }
        }

        private void Show()
        {
            _buyButton.SetActive(false);
            _nftDisplay.SetActive(true);
        }
    }
}
