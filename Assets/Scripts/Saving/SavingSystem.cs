using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// core(main) saving system

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            print("saving to " + path); //FromFile(defaultSaveFile)

            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                //stream.WriteByte(102); // decimal value of 102, f according to ascii table, saves to file
                //stream.WriteByte(0xc3); // Hex example, starts with 0x, prints a stylized A..
                //stream.WriteByte(0x80); // .. according to utf8 table
                                        // Alternatively..
                Transform playerTransform = GetPlayerTransform();
                // then serialize playerTransform..
                byte[] positionBuffer = SerializeVector(playerTransform.position);
                //byte[] byteArray = Encoding.UTF8.GetBytes("¡ yeah um µ");
                stream.Write(positionBuffer, 0, positionBuffer.Length);
                //stream.Write(byteArray, 0, byteArray.Length);
                // auto closes stream on exit of using
            }
            // must do this to close any file handles that were created vv
            // stream.Close(); no longer needed with using(Filestream)
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            print("loading " + path);
            using (FileStream stream = File.Open(path, FileMode.Open)) // bc .Create overwrites file
            {
                byte[] loadBuffer = new byte[stream.Length]; // create a buffer and specify it's size
                stream.Read(loadBuffer, 0, loadBuffer.Length);
                print(Encoding.UTF8.GetString(loadBuffer));

                Transform playerTransform = GetPlayerTransform();
                playerTransform.position = DeserializeVector(loadBuffer);
            }
        }


        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }

        private byte[] SerializeVector(Vector3 vector3) // produce byte array from a vector3
        {
            // takes in a Vector3 and converts into a byte array
            byte[] byteVectors = new byte[3 * 4]; // 3 floats * float size in bytes
            BitConverter.GetBytes(vector3.x).CopyTo(byteVectors, 0); //byteArray
            BitConverter.GetBytes(vector3.y).CopyTo(byteVectors, 4); //byteArray
            BitConverter.GetBytes(vector3.z).CopyTo(byteVectors, 8); //byteArray
            return byteVectors;
        }

        private Vector3 DeserializeVector(byte[] bufferArray) // produce our Vector3 from buffer
        {
            Vector3 resultVector3 = new Vector3();
            resultVector3.x = BitConverter.ToSingle(bufferArray, 0); //convert one byte(4 chars) starting from 0
            resultVector3.y = BitConverter.ToSingle(bufferArray, 4);
            resultVector3.z = BitConverter.ToSingle(bufferArray, 8);
            return resultVector3;
        }

        private string GetPathFromFile(string saveFile)
        {
            // .sav just to mark it as indeed a save file
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}