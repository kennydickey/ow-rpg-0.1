using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RPG.CharaStats
{
    [CreateAssetMenu(fileName = "CharaProgSO", menuName = "CharaStats/NewCharaProgression", order = 0)]
    public class CharaProgression : ScriptableObject
    {
        [SerializeField] CharaProgressionClass[] charaClasses = null; // field of class created below

        public float GetNewStatFromProg(UpCharaStats stat, CharaClass charaClass, int level)
        {
            foreach(CharaProgressionClass charaProgressionClass in charaClasses)
            {
                if (charaProgressionClass.charaClass != charaClass) continue;// not what we're looking for, continue foreach

                foreach(Multistat multistat in charaProgressionClass.stats) // nested foreach
                {
                    if (multistat.newStat != stat) continue;
                    if (multistat.levels.Length < level) continue;

                    return multistat.levels[level - 1];
                    //return charaProgressionClass.health[level - 1]; // returns health amount in correct array slot
                }
            }

            return 0;

        }

        [System.Serializable]
        class CharaProgressionClass
        {
            public CharaClass charaClass; // this is our enum which displays itself as a dropdown of classes
            public Multistat[] stats; // temp ProgressionStat[] stats

            //public int[] health; // just an array of ints for now called health

        }

        [System.Serializable]
        class Multistat
        {
            public UpCharaStats newStat; // we will define whether this is health, exp points, etc // temp Stat stat
            public float[] levels;
        }
       
    }
}