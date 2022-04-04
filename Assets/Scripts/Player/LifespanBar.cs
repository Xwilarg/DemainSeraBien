using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50.Player
{
    public class LifespanBar : MonoBehaviour
    {
        public Image fillBar;

        public void SetValue(float value)
        {
            fillBar.fillAmount = value;
        }
    }
}
