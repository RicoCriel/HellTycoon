using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonExit : MonoBehaviour
{
    [SerializeField] EconManager EconManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == null) return;

        string tag = other.tag;
        if (tag == "Demon")
        {
            EconManager.AddMoney(DemonValue(other.gameObject));
            Destroy(other.gameObject);
        }
    }
    
    private int DemonValue(GameObject devil) 
    {
        return
        devil.GetComponent<DemonHandler>().HornLevel * EconManager.HornLevelValue +
            devil.GetComponent<DemonHandler>().BodyLevel * EconManager.BodyLevelValue +
                devil.GetComponent<DemonHandler>().FaceLevel * EconManager.FaceLevelValue +
                    devil.GetComponent<DemonHandler>().ArmorLevel * EconManager.ArmorLevelValue +
                        devil.GetComponent<DemonHandler>().WingsLevel * EconManager.WingLevelValue;
    }
}
