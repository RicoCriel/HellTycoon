using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonManager : MonoBehaviour
{
    private List<DemonFear> _demons = new List<DemonFear>();



    // Add enemy to the list
    public void AddDemon(DemonFear demon)
    {
        _demons.Add(demon);
    }

    // Remove enemy from the list
    public void RemoveDemon(DemonFear demon)
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
        return _demons;
    }

    // TODO: call this every x amount of time
    public void TickFear()
    {
        foreach (var demon in _demons)
        {
            // TODO: Cehck which layer, decrease fear accordingly
            demon.DecreaseFear(2 * demon.GetLayer());
        }
    }
}
