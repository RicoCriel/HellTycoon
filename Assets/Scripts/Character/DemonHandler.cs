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
    
    [SerializeField] int _maxBodyLevel = 2;
    [SerializeField] int _maxFaceLevel = 2;
    [SerializeField] int _maxHornLevel = 2;
    [SerializeField] int _maxArmorLevel = 2;
    [SerializeField] int _maxWingsLevel = 2;





    
    [SerializeField] SpriteRenderer _horns;
    [SerializeField] SpriteRenderer _head;
    [SerializeField] SpriteRenderer _face;
    [SerializeField] SpriteRenderer _armor;
    [SerializeField] SpriteRenderer _wings;

    [SerializeField] Sprite[] _hornsSprites;
    [SerializeField] Sprite[] _headSprites;
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Sprite[] _armorSprites;
    [SerializeField] Sprite[] _wingsSprites;



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
