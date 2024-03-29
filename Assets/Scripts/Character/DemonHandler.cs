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
    [SerializeField] private SpriteRenderer _wingsL;

    [SerializeField] private Sprite[] _hornsSprites;
    [SerializeField] private Sprite[] _headSprites;
    [SerializeField] private Sprite[] _faceSprites;
    [SerializeField] private Sprite[] _armorSprites;
    [SerializeField] private Sprite[] _wingsSprites;

    

     private Camera _mainCamera;

    public int BodyLevel;
    public int FaceLevel;
    public int HornLevel;
    public int ArmorLevel;
    public int WingsLevel;

    [SerializeField] private MachineNode _currentMachineNode;
  


    private void Start()
    {
        BodyLevel = 0;
        FaceLevel = 0;
        HornLevel = 0;
        ArmorLevel = 0;
        WingsLevel = 0;
    }

 

    private void Awake()
    {
        _mainCamera = Camera.main;
        AngleToCamera();
    }

    private void OnEnable()
    {
        UpdateSprites();
    }


    private void Update()
    {
        AngleToCamera();
    }

    private void UpdateSprites()
    {
        if (_maxBodyLevel < BodyLevel) { BodyLevel = _maxBodyLevel - 1; }
        if (_maxFaceLevel < FaceLevel) { FaceLevel = _maxFaceLevel - 1; }
        if (_maxHornLevel < HornLevel) { HornLevel = _maxHornLevel - 1; }
        if (_maxArmorLevel < ArmorLevel) { ArmorLevel = _maxArmorLevel - 1; }
        if (_maxWingsLevel < WingsLevel) { WingsLevel = _maxWingsLevel - 1; }

        _horns.sprite = _hornsSprites[HornLevel];
        //Debug.Log(_horns.sprite);
        _head.sprite = _headSprites[BodyLevel];
        _face.sprite = _faceSprites[FaceLevel];
        _armor.sprite = _armorSprites[ArmorLevel];
        _wings.sprite = _wingsSprites[WingsLevel];
        _wingsL.sprite = _wingsSprites[WingsLevel];

    }

    private void AngleToCamera()
    {
        if (_mainCamera != null)
        {
            Vector3 lookAtDirection = _mainCamera.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(lookAtDirection);
        }
        else
        {
            Debug.LogWarning("No camera assigned");
        }
    }

    public void SetStats(MachineType machineType)
    {
        _currentMachineNode = _currentMachineNode.NextNodes[(int)machineType];

        BodyLevel = _currentMachineNode.Body;
        WingsLevel = _currentMachineNode.Wings;
        HornLevel = _currentMachineNode.Horns;
        ArmorLevel = _currentMachineNode.Armor;
        FaceLevel = _currentMachineNode.Face;

    }
}
