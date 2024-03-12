using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    // head
    public SpriteRenderer Horns;
    public SpriteRenderer Head;
    public SpriteRenderer Face;


    //Body
    public SpriteRenderer Body;

    //Left arm
    public SpriteRenderer LeftShoulder;
    public SpriteRenderer LeftArm;
    public SpriteRenderer LeftHand;

    //Right arm
    public SpriteRenderer RightShoulder;
    public SpriteRenderer RightArm;
    public SpriteRenderer RightHand;

    //Left Leg
    public SpriteRenderer LeftLeg;
    public SpriteRenderer LeftFoot;


    //Right Leg
    public SpriteRenderer RightLeg;
    public SpriteRenderer RightFoot;

    // head
    public Sprite[] hornsSprite;
    public Sprite[] HeadSprite;
    public Sprite[] FaceSprite;


    //Body
    public Sprite[] BodySprite;

    //Left arm
    public Sprite[] LeftShoulderSprite;
    public Sprite[] LeftArmSprite;
    public Sprite[] LeftHandSprite;

    //Right arm
    public Sprite[] RightShoulderSprite;
    public Sprite[] RightArmSprite;
    public Sprite[] RightHandSprite;

    //Left Leg
    public Sprite[] LeftLegSprite;
    public Sprite[] LeftFootSprite;


    //Right Leg
    public Sprite[] RightLegSprite;
    public Sprite[] RightFootSprite;

    private int BodyLevel;
    private int FaceLevel;
    private int HornLevel;

    private void Start()
    {
        BodyLevel = 0;
        FaceLevel = 0;
        HornLevel = 0;
    }


    private void Update()
    {

    }

    public void UpgradeHorns()
    {
        
            if(HornLevel +1 < hornsSprite.Length)
            {
                HornLevel++;
                Horns.sprite = hornsSprite[HornLevel];
            }
        
    }

    public void DegradeHorns()
    {

        if (HornLevel  > 0)
        {
            HornLevel--;
            Horns.sprite = hornsSprite[HornLevel];
        }

    }
}
