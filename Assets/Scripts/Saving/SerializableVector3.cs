using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable] // all fields in this class will try to be serialized
    public class SerializableVector3 // ! Maybe not with Monobehaviour??
    {
        // for making our players position serializeable
        float x, y, z;

        public SerializableVector3(Vector3 vector3)  // Building our constructor within class of the same name
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
            
        }
    }
}
