using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<DemonFear> existingDemons = _demons
    .Where(demon => demon != null)
    .Select(demon => demon.GetComponent<DemonFear>())
    .Where(demonFear => demonFear != null)
    .ToList();
        return existingDemons.ConvertAll(rdemon => rdemon.GetComponent<DemonFear>());
    }

    public List<DemonHandler> GetDemonHandlers()
    {
        List<DemonHandler> existingDemons = _demons
   .Where(demon => demon != null)
   .Select(demon => demon.GetComponent<DemonHandler>())
   .Where(demonFear => demonFear != null)
   .ToList();
        return existingDemons.ConvertAll(rdemon => rdemon.GetComponent<DemonHandler>());
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
