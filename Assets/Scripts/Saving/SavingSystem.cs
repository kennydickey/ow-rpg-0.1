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
                stream.WriteByte(102); // decimal value of 102, f according to ascii table, saves to file
                stream.WriteByte(0xc3); // Hex example, starts with 0x, prints a stylized A..
                stream.WriteByte(0x80); // .. according to utf8 table
                                        // Alternatively..
                byte[] byteArray = Encoding.UTF8.GetBytes("¡ yeah um µ");
                stream.Write(byteArray, 0, byteArray.Length);
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
                byte[] buffer = new byte[stream.Length]; // create a buffer and specify it's size
                stream.Read(buffer, 0, buffer.Length);
                print(Encoding.UTF8.GetString(buffer));
            }             
        }

        private string GetPathFromFile(string saveFile)
        {
            // .sav just to mark it as indeed a save file
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}