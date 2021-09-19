using System;
using UnityEngine;

namespace RPG.CharaStats
{
    public class BaseCharaStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startCharaLevel = 1;
        [SerializeField] CharaClass charaClass; // our ienum
        [SerializeField] CharaProgression charaProgSO = null; // our scriptableobject
        [SerializeField] GameObject lvUpParticle = null;
        [SerializeField] bool shouldUseModifiers = false;

        int currentLevel = 0; // invalid here, must initialize afterward

        public event Action onLevelUp; //new

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            // cannot move ths to Awake() bs we are accessing another class, would be too soon
            currentLevel = CalculateLevel();
        }

        // OnEnable can be called multiple times on a class
        private void OnEnable() // called after Awake() for the same method
        {
            if (experience != null)
            {
                // when onExperienceGained is called in Experience script, UpdateLevel will execute as it is subscribed v
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                // unsubscribing so that we don't get callbacks while disabled
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() // bug in here somewhere
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel) // may need to tweak logic if it's decided that levels can go down also
            {
                currentLevel = newLevel; // updating currentLevel to reflect new
                LvUpEffect();
                onLevelUp();
            }           
        }

        private void LvUpEffect()
        {
            Instantiate(lvUpParticle, transform); // calls for transform of the parent fyi
        }

        public float GetStatFromProg(UpCharaStats stat)// whatever user inputs when calling is the stat they will recieve
        {
            // just float + float                                       ..then make into 1.whatever percentage
            return (GetBaseStatFromProg(stat) + GetAdditiveMod(stat)) * (1 + GetPercentageMod(stat)/100);
        }       

        private float GetBaseStatFromProg(UpCharaStats stat)
        {
            return charaProgSO.GetNewStatFromProg(charaClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1) // since 0 is not a valid num here
            {
                currentLevel = CalculateLevel(); // ensures our currentLevel is cached and ready
            }
            return currentLevel;
        }

        private float GetAdditiveMod(UpCharaStats stat) // stat will be whatever we pass in when calling this method
        {
            if (!shouldUseModifiers) return 0; // for our enemies that shouldn't, player has this ticked

            float total = 0; // total of all of our modifiers
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) // in multiple ImodifierProviders
            {
                foreach (float modifierCount in provider.GetAdditiveModsI(stat))
                {
                    total += modifierCount; // adding a whatever float value of whatever float modifier is to total
                }
            }
            return total; // returns 0 if no modifiers found
        }

        private float GetPercentageMod(UpCharaStats stat) // used above in GetStatFromProg
        {
            if (!shouldUseModifiers) return 0;

            float total = 0; // total of all of our modifiers
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) // in multiple ImodifierProviders
            {
                foreach (float modifierCount in provider.GetPercentageModsI(stat))
                {
                    total += modifierCount; // adding a whatever float value of whatever float modifier is to total
                }
            }
            return total; // returns 0 if no modifiers found
        }

        private int CalculateLevel()
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