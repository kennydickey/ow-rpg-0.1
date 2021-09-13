using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RPG.CharaStats
{
    [CreateAssetMenu(fileName = "CharaProgSO", menuName = "CharaStats/NewCharaProgression", order = 0)]
    public class CharaProgression : ScriptableObject
    {
        [SerializeField] CharaProgressionClass[] charaClasses = null; // array of class created below

        // nested dictionary with CharaClass as the first key, and nested Dictionary as the value withit's own key, value
        Dictionary<CharaClass, Dictionary<UpCharaStats, float[]>> lookupTable = null; // initialization of lookupTable

        public float GetNewStatFromProg(CharaClass charaClass, UpCharaStats stat, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[charaClass][stat];
            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];

            //foreach(CharaProgressionClass charaProgressionClass in charaClasses)
            //{
            //    if (charaProgressionClass.charaClass != charaClass) continue;// not what we're looking for, continue foreach
            //    foreach(Multistat multistat in charaProgressionClass.stats) // nested foreach
            //    {
            //        if (multistat.newStat != stat) continue;
            //        if (multistat.levels.Length < level) continue;
            //        return multistat.levels[level - 1];
            //        //return charaProgressionClass.health[level - 1]; // returns health amount in correct array slot
            //    }
            //}
            //return 0;
        }

        //public int ge

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharaClass, Dictionary<UpCharaStats, float[]>>(); // new empty table

            // charaClasses is an array of CharaProgressionClass       vv
            foreach (CharaProgressionClass charaProgressionClass in charaClasses)
            {
                                            
                var statLookupTable = new Dictionary<UpCharaStats, float[]>();

                foreach(Multistat multistat in charaProgressionClass.stats)
                {
                    // newstat as the key       vv                   vv   levels as variable to create
                    statLookupTable[multistat.newStat] = multistat.levels;
                }
                                    //key vv
                lookupTable[charaProgressionClass.charaClass] = statLookupTable;
            }
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