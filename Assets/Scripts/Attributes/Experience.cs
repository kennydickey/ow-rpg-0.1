using System.Collections;
using System.Collections.Generic;
using RPG.CharaStats;
using UnityEngine;

namespace RPG.Attributes
{
    // Script for the Player. Not using for enemies right now
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;

        public void GainExperience(float addExperience) // called in Health.cs
        {
            experiencePoints += addExperience;           
        }
    }
}
