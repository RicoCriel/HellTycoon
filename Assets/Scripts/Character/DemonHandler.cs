using System;
using Buildings;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHandler : MonoBehaviour
{
    [SerializeField] private DemonStats _maxLevel;

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

    public DemonStats Level;

    [SerializeField] private MachineNode _currentMachineNode;


    private void Start()
    {
        Level.Body = 0;
        Level.Face = 0;
        Level.Wings = 0;
        Level.Horn = 0;
        Level.Armor = 0;

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
        if (_maxLevel.Body < Level.Body) { Level.Body = _maxLevel.Body - 1; }
        if (_maxLevel.Face < Level.Face) { Level.Face = _maxLevel.Face - 1; }
        if (_maxLevel.Horn < Level.Horn) { Level.Horn = _maxLevel.Horn - 1; }
        if (_maxLevel.Armor < Level.Armor) { Level.Armor = _maxLevel.Armor - 1; }
        if (_maxLevel.Wings < Level.Wings) { Level.Wings = _maxLevel.Wings - 1; }

        _horns.sprite = _hornsSprites[Level.Horn];
        //Debug.Log(_horns.sprite);
        _head.sprite = _headSprites[Level.Body];
        _face.sprite = _faceSprites[Level.Face];
        _armor.sprite = _armorSprites[Level.Armor];
        _wings.sprite = _wingsSprites[Level.Wings];
        _wingsL.sprite = _wingsSprites[Level.Wings];

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

        Level.Body = _currentMachineNode.Body;
        Level.Wings = _currentMachineNode.Wings;
        Level.Wings = _currentMachineNode.Horns;
        Level.Armor = _currentMachineNode.Armor;
        Level.Face = _currentMachineNode.Face;

    }
<<<<<<< HEAD
 }
=======

    [Serializable]
    public struct DemonStats
    {
        public int Body;
        public int Horn;
        public int Wings;
        public int Armor;
        public int Face;

        public DemonStats(int body, int horn, int wings, int armor, int face)
        {
            Body = body;
            Horn = horn;
            Wings = wings;
            Armor = armor;
            Face = face;
        }

        public DemonStats(int value)
        {
            Body = value;
            Horn = value;
            Wings = value;
            Armor = value;
            Face = value;
        }
    }
}
>>>>>>> bf8eec412c1da1c71f45125a945cb0fe97be2a33
