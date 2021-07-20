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

            FileStream stream = File.Open(path, FileMode.Create);
            stream.WriteByte(102); // decimal value of 102, f according to ascii table, saves to file
            stream.WriteByte(0xc3); // Hex example, starts with 0x, prints a stylized A..
            stream.WriteByte(0x80); // .. according to utf8 table
            // Alternatively..
            byte[] byteArray = Encoding.UTF8.GetBytes("¡ yeah um µ");
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close(); // must do this to close any file handles that were created
        }

        public void Load(string saveFile)
        {
            print("loading " + GetPathFromFile(saveFile));
        }

        private string GetPathFromFile(string saveFile)
        {
            // .sav just to mark it as indeed a save file
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}