using System.Collections;
using System.Collections.Generic;

namespace RPG.CharaStats
{
    public interface IModifierProvider // so far implemented in Fighter.cs
    {
        // Because Ienumerator does not allow foreach
        IEnumerable<float> GetAdditiveModI(UpCharaStats stat); // returns list of modifiers for a particular stat
    }
}