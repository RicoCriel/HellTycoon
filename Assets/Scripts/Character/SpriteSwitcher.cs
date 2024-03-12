using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    // head
    public SpriteRenderer Horns;
    public SpriteRenderer Head;
    public SpriteRenderer Face;
    public SpriteRenderer Armor;
    public SpriteRenderer Wings;
    



   

    

    // head
    public Sprite[] hornsSprites;
    public Sprite[] HeadSprites;
    public Sprite[] FaceSprites;
    public Sprite[] ArmorSprites;
    public Sprite[] WingsSprites;





    private int BodyLevel;
    private int FaceLevel;
    private int HornLevel;
    private int ArmorLevel;
    private int WingsLevel;

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
