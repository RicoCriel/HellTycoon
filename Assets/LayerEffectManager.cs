using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public enum HellLayer
{
    Limbo = 1,
    Lust = 2,
    Gluttony = 3,
    Greed = 4,
    Wrath = 5,
    Heresy = 6,
    Violence = 7,
    Fraud = 8,
    Treachery = 9
}


public class LayerEffectManager : MonoBehaviour
{
    static public void CheckInitLayer(DemonFear demon)
    {

        demon.InitHellLayer = (HellLayer)demon.Layer;

    }
}

