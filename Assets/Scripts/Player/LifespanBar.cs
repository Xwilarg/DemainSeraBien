using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50.Player
{
    public class LifespanBar : MonoBehaviour
    {
        public Image fillBar;
        public Image fillBarOther;

        public void SetValue(float value)
        {
            fillBar.fillAmount = value;
        }

        public void SetOtherValue(float value)
        {
            fillBarOther.fillAmount = value;
        }
    }
}
