using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CharaStats
{
    public class ExpDisplay : MonoBehaviour
    {
        Experience experience;
        BaseCharaStats baseCharaStats;

        // Start is called before the first frame update
        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            baseCharaStats = GameObject.FindWithTag("Player").GetComponent<BaseCharaStats>();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetExp().ToString());
        }
    }
}
