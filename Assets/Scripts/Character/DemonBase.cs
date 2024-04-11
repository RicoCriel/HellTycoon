using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBase : MonoBehaviour
{
    [SerializeField] private DemonHandler _demonHandler;

    [SerializeField] private DemonFear _demonFear;

    [SerializeField] private DemonSettings _demonSettings;

    public DemonBehaviourBase _demonBehaviourBase;



    public DemonHandler DemonHandler
    {
        get
        {
            return _demonHandler;
        }
    }

    public DemonFear DemonFear
    {
        get
        {
            return _demonFear;
        }
    }

    public DemonSettings DemonSettings
    {
        get
        {
            return _demonSettings;
        }
    }


    public void InitEffect(HellLayer hellLayer)
    {
        switch (hellLayer)
        {
            case HellLayer.Limbo:
                _demonBehaviourBase = new LimboBehaviour();
                break;
            case HellLayer.Lust:
                _demonBehaviourBase = new LustBehaviour();
                break;
            case HellLayer.Gluttony:
                _demonBehaviourBase = new GluttonyBehaviour();
                break;
            case HellLayer.Greed:
                _demonBehaviourBase = new GreedBehaviour();
                break;
            case HellLayer.Wrath:
                _demonBehaviourBase = new WrathBehaviour();
                break;
            case HellLayer.Heresy:
                _demonBehaviourBase = new HeresyBehaviour();
                break;
            case HellLayer.Violence:
                _demonBehaviourBase = new ViolenceBehaviour();
                break;
            case HellLayer.Fraud:
                _demonBehaviourBase = new FraudBehaviour();
                break;
            case HellLayer.Treachery:
                _demonBehaviourBase = new TreacheryBehaviour();
                break;
        }
    }
}
