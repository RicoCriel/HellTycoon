using Buildings;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHandler : MonoBehaviour
{
   
    
    [SerializeField] private int _maxBodyLevel = 2;
    [SerializeField] private int _maxFaceLevel = 2;
    [SerializeField] private int _maxHornLevel = 2;
    [SerializeField] private int _maxArmorLevel = 2;
    [SerializeField] private int _maxWingsLevel = 2;





    
    [SerializeField] private SpriteRenderer _horns;
    [SerializeField] private SpriteRenderer _head;
    [SerializeField] private SpriteRenderer _face;
    [SerializeField] private SpriteRenderer _armor;
    [SerializeField] private SpriteRenderer _wings;

    [SerializeField] private Sprite[] _hornsSprites;
    [SerializeField] private Sprite[] _headSprites;
    [SerializeField] private Sprite[] _faceSprites;
    [SerializeField] private Sprite[] _armorSprites;
    [SerializeField] private Sprite[] _wingsSprites;


    public int BodyLevel;
    public int FaceLevel;
    public int HornLevel;
    public int ArmorLevel;
    public int WingsLevel;



    private void Start()
    {
        BodyLevel = 0;
        FaceLevel = 0;
        HornLevel = 0;
        ArmorLevel = 0;
        WingsLevel = 0;
        MachinePart.onDemonStatsChanged += UpdateSprites;
      
}


    private void Update()
    {

    }

    private void UpdateSprites()
    {
        if (_maxBodyLevel > BodyLevel) { BodyLevel = _maxBodyLevel; }
        if (_maxFaceLevel > FaceLevel) { FaceLevel = _maxFaceLevel; }
        if (_maxHornLevel > HornLevel) { HornLevel = _maxHornLevel; }
        if (_maxArmorLevel > ArmorLevel) { ArmorLevel = _maxArmorLevel; }
        if (_maxWingsLevel > WingsLevel) { WingsLevel = _maxWingsLevel; }
        
        _horns.sprite = _hornsSprites[HornLevel];
        _head.sprite = _headSprites[BodyLevel];
        _face.sprite = _faceSprites[FaceLevel];
        _armor.sprite = _armorSprites[ArmorLevel];
        _wings.sprite = _wingsSprites[WingsLevel];

    }

}
