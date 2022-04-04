using UnityEngine;
using TMPro;
using LudumDare50.Menu;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ageText;
    [SerializeField] private TextMeshProUGUI _innerAgeText;
    void Start()
    {
        string finalAge = NFTManager.Instance.FinalAge.ToString();
        _ageText.text = finalAge;
        _innerAgeText.text = finalAge;
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
