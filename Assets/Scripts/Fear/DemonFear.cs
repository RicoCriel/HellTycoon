using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFear : MonoBehaviour
{
    private int _fearLevel = 0;
    private int _currentLayer;
    public int FearLevel => _fearLevel;

    public int GetFearLevel()
    {
        return _fearLevel;
    }
    public int GetLayer()
    {
        return _currentLayer;
    }

    public void SetLayer(int layer)
    {
        _currentLayer = layer;
    }

    public void IncreaseFear(int amount)
    {
        _fearLevel += amount;
    }

    public void DecreaseFear(int amount)
    {
        _fearLevel -= amount;
    }

}
