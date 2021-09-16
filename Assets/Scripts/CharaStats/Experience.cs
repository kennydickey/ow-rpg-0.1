using System;
using System.Collections;
using System.Collections.Generic;
using RPG.CharaStats;
using RPG.Saving;
using UnityEngine;

namespace RPG.CharaStats
{
    // Script for the Player. Not using for enemies right now
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate(); // our delegte class
        //public event ExperienceGainedDelegate onExperienceGained; // new instance of our delegate
        // or..
        public event Action onExperienceGained; // called Action which is predefined instead of ExperienceGainedDelegate

        public void GainExperience(float addExperience) // called in Health.cs
        {
            experiencePoints += addExperience;
            onExperienceGained(); // will call everything in our delegate list when GainExperience() is called
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public float GetExp()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state; // state coming in cast as float to make sure we are getting the right type
        }
    }
}
