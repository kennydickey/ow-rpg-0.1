using UnityEngine;

namespace RPG.CharaStats
{
    public class BaseCharaStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startCharaLevel = 1;
        [SerializeField] CharaClass charaClass; // our ienum
        [SerializeField] CharaProgression charaProgSO = null; // our scriptableobject

        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                print(GetLevel());
            }
        }

        public float GetStatFromProg(UpCharaStats stat)// whatever user inputs when calling is the stat they will recieve
        {
            return charaProgSO.GetNewStatFromProg(charaClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startCharaLevel; // for enemies mostly, just exits function
            float currentXp = experience.GetExp();

            // getting second to last level by querying how many things are in level array
            int penultimateLevel = charaProgSO.GetLevels(charaClass, UpCharaStats.ExpToLevelUp); 
            for(int level = 1; level <= penultimateLevel; level++) // going over first to penultimate level
            {
                // get exp to levelup for particular level
                float getExpToLevelUp = charaProgSO.GetNewStatFromProg(charaClass, UpCharaStats.ExpToLevelUp, level);
                if(getExpToLevelUp > currentXp) // if we currently have less exp than required to levelup..
                {
                    return level; // then stay at this level and exit function
                }
            }
            //otherwise.. if currentXp is greater, therefore nothing was returned above, then return ultimate level v
            return penultimateLevel + 1;
        }
    }
}