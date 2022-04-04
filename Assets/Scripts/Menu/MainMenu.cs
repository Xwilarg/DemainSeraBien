using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LudumDare50.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Image _imageContainer;

        [SerializeField]
        private GameObject[] _toDisable;

        [SerializeField]
        private TMP_Text _moneyText;

        private void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            _moneyText.text = $"{NFTManager.Instance.MoneyAvailable}$";
        }

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
