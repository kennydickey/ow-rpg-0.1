using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.CharaStats
{
    [CreateAssetMenu(fileName = "CharaProgSO", menuName = "CharaStats/NewCharaProgression", order = 0)]
    public class CharaProgression : ScriptableObject
    {
        [SerializeField] CharaProgressionClass[] charaClasses = null; // field of class created below

        public int GetHealth(CharaClass charaClass, int level)
        {
            foreach(CharaProgressionClass charaProgressionClass in charaClasses)
            {
                if(charaProgressionClass.charaClass == charaClass)
                {
                    return charaProgressionClass.health[level - 1];
                }
            }

            return 0;

        }

        [System.Serializable]
        class CharaProgressionClass
        {
            public CharaClass charaClass;
            public int[] health; // just an array of ints for now called health

        }

    }
}