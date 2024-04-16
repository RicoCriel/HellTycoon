using UnityEngine;
using UnityEngine.UI;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// A character in the turn base example. This is used to tag a character's element type. 
    /// </summary>
    public class TurnBaseCharacter : MonoBehaviour
    {
        public EElementType ElementType;
        public float Hp = 10;
        public float MaxHp = 10;

        public Image HpFillImage;

        public bool IsDead => Hp <= 0;

        public void TakeDamage(float damage)
        {
            Hp -= damage;
            if (Hp <= 0)
                Hp = 0;

            HpFillImage.fillAmount = Hp / MaxHp;
        }
    }
}