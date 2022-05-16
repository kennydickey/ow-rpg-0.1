using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor //child can access parent RPG.Dialogue // we do not want things toaccess editor class however
{
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")] // can call from tab
        public static void ShowEditorWindow() //static belongs to the class, so DialogEtitors in general, not an instance of the class
        {
            //when creating editor from window tab..
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OpenOnlyEditorAsset(int instanceID,int line)
        {
            // convert instanceID to object cast as Dialogue SO, else function of 'as' retuns null
            DialogueSO dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;

            if(dialogue != null)
            {
                Debug.Log("opendialog");
                ShowEditorWindow(); // also can call from SO item
                return true;
            }
            return false;
        }

        private void OnGUI()
        {
            Debug.Log("ongui");
            Repaint(); // method belongs to editor window, call OnGui() to redraw ui constantly
        }
    }

}

