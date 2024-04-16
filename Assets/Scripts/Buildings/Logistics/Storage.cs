using Splines;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using PopupSystem.Inheritors;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : BuildingFactoryBase
{
    private WorldSpacePopupStorage _popup;

    protected new void Awake()
    {
        base.Awake();

        _popup = GetComponentInChildren<WorldSpacePopupStorage>();

        StopSpawning();

        _popup.SoulProcessingPaused += PopupOnSoulProcessingPaused;
        _popup.SoulProcessingResumed += PopupOnSoulProcessingResumed;
        _popup.DestroyButtonClicked += OnDestroyButtonClicked;
    }

    private void PopupOnSoulProcessingResumed(object sender, PopupClickedEventArgs e)
    {
        ResumeProcessing();
    }

    private void PopupOnSoulProcessingPaused(object sender, PopupClickedEventArgs e)
    {
        StopSpawning();
    }

    public override void AddDemon(Queue<GameObject> DemonList, GameObject demon)
    {
        base.AddDemon(_processedDemonContainer, demon);
        if (_processedDemonContainer.Count > MaxDemons)
        {
            ExecuteMachineSpawningBehaviour();
        }
        _popup.SetSouls(_unprocessedDemonContainer.Count + _processedDemonContainer.Count);
    }

    private void OnDestroyButtonClicked(object sender, PopupSystem.PopupClickedEventArgs e)
    {
        Destroy(gameObject);
    }
}


