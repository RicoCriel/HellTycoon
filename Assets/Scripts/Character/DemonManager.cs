using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonManager : MonoBehaviour
{
    private List<GameObject> _demons = new List<GameObject>();

    [SerializeField] private float _tickIntervalTime;


    private void Start()
    {
        InvokeRepeating("TickFear", 0f, _tickIntervalTime);
    }
    // Add enemy to the list
    public void AddDemon(GameObject demon)
    {
        _demons.Add(demon);
    }

    // Remove enemy from the list
    public void RemoveDemon(GameObject demon)
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
        return _demons.ConvertAll(demon => demon.GetComponent<DemonFear>());
    }

    public List<DemonHandler> GetDemonHandlers()
    {
        return _demons.ConvertAll(demon => demon.GetComponent<DemonHandler>());
    }

    public void TickFear()
    {
        List<DemonFear> demonFears = GetDemonFears();
        foreach (var demonFear in demonFears)
        {
            demonFear.DecreaseFear(demonFear.Layer * demonFear.DecayRate);
        }
    }
}
