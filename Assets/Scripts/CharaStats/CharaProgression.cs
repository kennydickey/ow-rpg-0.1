using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.CharaStats
{
    [CreateAssetMenu(fileName = "CharaProgSO", menuName = "CharaStats/NewCharaProgression", order = 0)]
    public class CharaProgression : ScriptableObject
    {
        [SerializeField] CharaProgressionClass[] charaClasses = null; // field of class created below

        [System.Serializable]
        class CharaProgressionClass
        {
            [SerializeField] CharaClass characlass;
            [SerializeField] float[] health; // just an array of floats for now called health

        }

    }
}