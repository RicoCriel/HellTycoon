using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{





    private int _bodyLevel;
    private int _faceLevel;
    private int _hornLevel;
    private int _armorLevel;
    private int _wingsLevel;

    // head
    public SpriteRenderer Horns;
    public SpriteRenderer Head;
    public SpriteRenderer Face;
    public SpriteRenderer Armor;
    public SpriteRenderer LeftWings;
    public SpriteRenderer RightWings;







    // head
    public Sprite[] HornsSprites;
    public Sprite[] HeadSprites;
    public Sprite[] FaceSprites;
    public Sprite[] ArmorSprites;
    public Sprite[] WingsSprites;


    private void Start()
    {
        //_bodyLevel = 0;
        //_faceLevel = 0;
        //_hornLevel = 0;
        //_armorLevel = 0;
        //_wingsLevel = 0;
    }


    private void Update()
    {

    }

    
}
