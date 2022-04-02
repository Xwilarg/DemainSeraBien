using UnityEngine;
using UnityEngine.UI;

public class LifespanBar : MonoBehaviour
{
    public Image fillBar;
    public Image fillBarSlow;

    private float maximum;
    private float current;
    private float currentSlow;

    void Start()
    {
        float lifespan = CharacterLifeManager.instance.currentLifespan;
        maximum = lifespan;
        current = lifespan;
        currentSlow = lifespan;
    }
    
    private float t = 0;

    void Update()
    {
        float lifespan = CharacterLifeManager.instance.currentLifespan;

        if (current != lifespan)
        {
            current = lifespan;
            t = 0;
        }

        if (currentSlow != current)
        {
            currentSlow = Mathf.Lerp(currentSlow, current, t);
            t += 1.0f * Time.deltaTime;
        }

        fillBar.fillAmount = current/maximum;
        fillBarSlow.fillAmount = currentSlow/maximum;
    }
}
