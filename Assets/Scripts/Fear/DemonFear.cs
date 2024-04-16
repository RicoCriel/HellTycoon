using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DemonFear : MonoBehaviour
{
    [SerializeField] private float _fearLevel = 0;
    public float FearLevel => _fearLevel;

    [SerializeField] private int _layer;
    public int Layer => _layer;
    [SerializeField] private int _layertHightDiff = 100;

    

    public float DecayRate;

    public HellLayer InitHellLayer;

    private void Start()
    {
        SetLayer();
        LayerEffectManager.CheckInitLayer(this);
        this.gameObject.GetComponent<DemonBase>().InitEffect(InitHellLayer);
        //ApplyLayerEffect();
    }

    private void ApplyLayerEffect() 
    {
        switch (InitHellLayer)
        {
            case HellLayer.Limbo:
                //Advantage: Steady supply of souls allows for consistent production.
                //Disadvantage: Souls are low tier(low base price), requiring more upgrades to increase their value.
                break;
            case HellLayer.Lust:
                //Advantage: High demand for souls from this layer(there is always a base demand / floor sell price for souls from this layer).
                //Disadvantage: Low spawn rate or random / very unpredictable spawn rate.
                break;
            case HellLayer.Gluttony:
            //Advantage: Souls from this layer generate more fuel for the fear machines compared to normal souls.
            //Disadvantage: Fat souls get stuck in the machines from time to time(need to sent troops to fix blockage).
                break;
            case HellLayer.Greed:
            //Advantage: When souls form this layer are sold for a certain % higher than their average sell price they will get another % of bonus money.
            //Disadvantage: Souls from this layer will refuse to be sold for a less then average price.
                            break;
            case HellLayer.Wrath:
            //Advantage: When souls from this layer are turned into troops, they are more efficient are require less upkeep.
            //Disadvantage: Souls from this layer might start fighting with each other from time to time(troops will need to be sent to fix this)
                break;
            case HellLayer.Heresy:
            //Advantage: Souls from this layer have a lower resistance to fear, making them more efficient to upgrade.
            //Disadvantage: Souls from this layer may require special handling due to their tendency to question authority, leading to increased operational costs.
                break;
            case HellLayer.Violence:
            //Advantage: Strong souls that have naturally high starting value
            //Disadvantage: The souls need A LOT of fear to be adapted by the machines
                break;
            case HellLayer.Fraud:
            //Advantage: Because these souls are skilled in deception they can bel sold for very high prices.
            //Disadvantage: The more souls from this layer currently exist the higher the chance that they may attempt to deceive the player(showing faking sales numbers, forging on spy reports)
                break;
            case HellLayer.Treachery:
            //Advantage: Souls from this layer are A LOT better when turned into spies compared to regular souls.
            //Disadvantage: Every soul you have from this layer has a high chance of giving the enemy tycoons info about you
                break;
        }
    }

    


        public float GetFearLevel()
    {
        return _fearLevel;
    }
  

    public void IncreaseFear(float amount)
    {
        _fearLevel += amount;
    }

    public void DecreaseFear(float amount)
    {
        _fearLevel -= amount;
    }

    public void SetLayer()
    {
        float result = (transform.position.y - 50) / _layertHightDiff;
        _layer = Mathf.Abs(Mathf.RoundToInt(result));
    }


}
