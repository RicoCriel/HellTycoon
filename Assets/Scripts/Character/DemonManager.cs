using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class DemonManager : MonoBehaviour
{
    private List<DemonBase> _demons = new List<DemonBase>();

    [SerializeField] private float _tickIntervalTime;


    private void Start()
    {
        InvokeRepeating("TickFear", 0f, _tickIntervalTime);
    }
    // Add enemy to the list
    public void AddDemon(GameObject demon)
    {
        _demons.Add(demon.GetComponent<DemonBase>());
    }

    // Remove enemy from the list
    public void RemoveDemon(DemonBase demon)
    {
        _demons.Remove(demon);
    }

    // Get total number of enemies
    public int GetEnemyCount()
    {
        return _demons.Count;
    }

    public List<DemonFear> GetDemonFears()
    {
        return _demons.Select(demon => demon.DemonFear).ToList();
    }

    public List<DemonHandler> GetDemonHandlers()
    {
        return _demons.Select(demon => demon.DemonHandler).ToList();
    }

    public void TickFear()
    {
        List<DemonFear> demonFears = GetDemonFears();
        foreach (var demonFear in demonFears)
        {
            //TODO: Make decy rate depeond on layer
            demonFear.DecreaseFear(demonFear.DecayRate);
        }
    }
}
