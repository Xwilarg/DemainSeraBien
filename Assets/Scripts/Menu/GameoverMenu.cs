using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare50.Menu
{
    public class GameoverMenu : MonoBehaviour
    {
        public void GoBackToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
