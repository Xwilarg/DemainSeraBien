using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LudumDare50.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Image _imageContainer;

        [SerializeField]
        private GameObject[] _toDisable;

        public void Play()
        {
            SceneManager.LoadScene("HouseLayout");
        }

        public void Show(Sprite sprite)
        {
            foreach (var go in _toDisable)
            {
                go.SetActive(false);
            }
            _imageContainer.gameObject.SetActive(true);
            _imageContainer.sprite = sprite;
        }

        public void OnClick(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
                {
                    var book = hit.collider.GetComponent<BookImageMenu>();
                    if (book != null)
                    {
                        book.ShowContent();
                    }
                }
            }
        }
    }
}
