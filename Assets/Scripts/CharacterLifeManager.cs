using System.Collections;
using UnityEngine;

public class CharacterLifeManager : MonoBehaviour
{
    public float maxLifespan;
    public float losePerSecond;
    [HideInInspector]
    public float currentLifespan;

    private static CharacterLifeManager characterLifeManager;

    public static CharacterLifeManager instance
    {
        get
        {
            if (!characterLifeManager)
            {
                characterLifeManager = FindObjectOfType(typeof(CharacterLifeManager)) as CharacterLifeManager;

                if (!characterLifeManager)
                    Debug.LogError("There needs to be one active CharacterLifeManager script on a GameObject in the scene.");
                else
                    characterLifeManager.Init();
            }
            return characterLifeManager;
        }
    }

    void Init()
    {
        currentLifespan = maxLifespan;

        StartCoroutine(ConsumeLifespan());
    }

    public void AddAmount(float amount)
    {
        float newAmount = currentLifespan + amount;
        currentLifespan = Mathf.Clamp(newAmount, 0, maxLifespan);
    }

    IEnumerator ConsumeLifespan()
    {
        // TODO: add not game over condition
        while (currentLifespan > 0)
        {
            AddAmount(-losePerSecond);
            yield return new WaitForSeconds(1);
        }
    }
}
