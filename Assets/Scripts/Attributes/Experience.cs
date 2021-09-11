using System;
using System.Collections;
using System.Collections.Generic;
using RPG.CharaStats;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    // Script for the Player. Not using for enemies right now
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public void GainExperience(float addExperience) // called in Health.cs
        {
            experiencePoints += addExperience;
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
