using UnityEngine;

namespace RPG.CharaStats
{
    public class BaseCharaStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startCharaLevel = 1;
        [SerializeField] CharaClass charaClass; // our ienum
        [SerializeField] CharaProgression charaProgSO = null; // our scriptableobject

        public float GetStatFromProg(UpCharaStats stat)// whatever user inputs when calling is the stat they will recieve
        {
            return charaProgSO.GetNewStatFromProg(charaClass, stat, startCharaLevel);
        }

        //public int GetLevel()
        //{
        //    float currentXp =  GetComponent<Experience>().GetExp();
        //    foreach()
        //}
    }
}