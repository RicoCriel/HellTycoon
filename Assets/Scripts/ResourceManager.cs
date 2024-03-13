using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ScriptableObject
{
    [SerializeField] private int _tortureTokens;
    public int TortureTokens => _tortureTokens;

    public void AddCoins(int amount)
    {
        _tortureTokens += amount;
    }

    public void RemoveCoins(int amount)
    {
        _tortureTokens -= amount;
    }
}
