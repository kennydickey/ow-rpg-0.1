using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RPG.CharaStats
{
    [CreateAssetMenu(fileName = "CharaProgSO", menuName = "CharaStats/NewCharaProgression", order = 0)]
    public class CharaProgression : ScriptableObject
    {
        [SerializeField] CharaProgressionClass[] charaClasses = null; // field of class created below

        public int GetHealthFromProg(CharaClass charaClass, int level)
        {
            foreach(CharaProgressionClass charaProgressionClass in charaClasses)
            {
                if(charaProgressionClass.charaClass == charaClass) // when called, just finding which class
                {
                    //return charaProgressionClass.health[level - 1]; // returns health amount in correct array slot
                }
            }

            return 0;

        }

        [System.Serializable]
        class CharaProgressionClass
        {
            public CharaClass charaClass; // this is our enum which displays itself as a dropdown of classes
            public Multistat stats;

            //public int[] health; // just an array of ints for now called health

        }

        [System.Serializable]
        class Multistat
        {
            NewCharaStat newStat; // we will define whether this is health, exp points, etc
            public float[] levels;
        }
       
    }
}