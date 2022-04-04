using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColorRandomization : MonoBehaviour
{
    [SerializeField]
    private List<Color> SkinColors = new List<Color>();

    [SerializeField]
    private GameObject Shirt;
    [SerializeField]
    private GameObject Skin;

    private void Start()
    {
        System.Random rand = new System.Random();
        Skin.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", SkinColors[rand.Next(SkinColors.Count)]);
        Shirt.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", Random.ColorHSV());
        /*
        Skin.GetComponent<Material>().SetColor("_Color", SkinColors[rand.Next(SkinColors.Count)]);
        Shirt.GetComponent<Material>().SetColor("_Color", Random.ColorHSV());
        */
    }
}
