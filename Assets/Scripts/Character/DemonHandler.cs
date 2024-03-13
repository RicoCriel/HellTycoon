using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHandler : MonoBehaviour
{
    public int BodyLevel;
    public int FaceLevel;
    public int HornLevel;
    public int ArmorLevel;
    public int WingsLevel;
    
    [SerializeField] int MaxBodyLevel = 2;
    [SerializeField] int MaxFaceLevel = 2;
    [SerializeField] int MaxHornLevel = 2;
    [SerializeField] int MaxArmorLevel = 2;
    [SerializeField] int MaxWingsLevel = 2;





    
    [SerializeField] SpriteRenderer Horns;
    [SerializeField] SpriteRenderer Head;
    [SerializeField] SpriteRenderer Face;
    [SerializeField] SpriteRenderer Armor;
    [SerializeField] SpriteRenderer Wings;

    [SerializeField] Sprite[] hornsSprites;
    [SerializeField] Sprite[] HeadSprites;
    [SerializeField] Sprite[] FaceSprites;
    [SerializeField] Sprite[] ArmorSprites;
    [SerializeField] Sprite[] WingsSprites;



    private void Start()
    {
        BodyLevel = 0;
        FaceLevel = 0;
        HornLevel = 0;
        ArmorLevel = 0;
        WingsLevel = 0;
    }


    private void Update()
    {

    }


}
