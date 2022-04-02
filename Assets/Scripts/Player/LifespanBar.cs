using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50.Player
{
    public class LifespanBar : MonoBehaviour
    {
        public Image fillBar;
        public Image fillBarSlow;

        private float _value;
        private float _valueSlow;
        private float t = 0;

        public void SetValue(float value, bool smoothen)
        {
            _value = value;
            t = 0;
            if (smoothen)
            {
                _valueSlow = value;
                fillBar.fillAmount = _value;
                fillBarSlow.fillAmount = _valueSlow;
            }
        }


        void Update()
        {
            if (_valueSlow != _value)
            {
                _valueSlow = Mathf.Lerp(_valueSlow, _value, t);
                t += 1.0f * Time.deltaTime;
            }

            fillBar.fillAmount = _value;
            fillBarSlow.fillAmount = _valueSlow;
        }
    }
}
