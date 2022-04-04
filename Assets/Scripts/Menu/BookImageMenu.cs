using UnityEngine;

namespace LudumDare50.Menu
{
    public class BookImageMenu : MonoBehaviour
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private MainMenu _menu;

        public void ShowContent()
        {
            _menu.Show(_sprite);
        }
    }
}
