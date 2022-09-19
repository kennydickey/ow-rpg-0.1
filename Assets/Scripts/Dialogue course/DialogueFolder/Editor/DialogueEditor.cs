using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;

namespace RPG.Dialogue.Editor //child can access parent RPG.Dialogue // we do not want things to access editor class while game is running however
{
    // makes this class intoo an editor window
    public class DialogueEditor : EditorWindow
    {
        DialogueSO selectedDialogueSO = null;

        [MenuItem("Window/Dialogue Editor _m")] // can call from tab
        public static void ShowEditorWindow() //static belongs to the class, so DialogEtitors in general, not an instance of the class
        {
            //when creating editor from window tab..
            //false not a utility window, so it's dockable
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OpenOnlyEditorAsset(int instanceID,int line)
        {
            // convert instanceID to object cast as Dialogue SO, else function of 'as' retuns null
            DialogueSO dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;

            if(dialogue != null)
            {
                ShowEditorWindow(); // also can call from SO item
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            // alternative to [OnOpenAsset], can be done on a per instance basis
            //not calling fn at this point, only adding to list to be called with all in selectionChanged list
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            Debug.Log("onselectionchanged");
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;
            if(newDialogue != null)
            {
                selectedDialogueSO = newDialogue;
            }
            Repaint(); // method belongs to editor window, triggers  OnGui() to redraw ui constantly
        }

        private void OnGUI()
        {
            if(selectedDialogueSO == null)
            {
                EditorGUILayout.LabelField("no dialogue selected");
            }
            else
            {
                foreach(DialogueNode node in selectedDialogueSO.GetAllNodes())
                {
                    EditorGUILayout.LabelField(node.text);
                }
            }
            
            //EditorGUI.LabelField(new Rect(20, 20, 100, 100), "hello");
            EditorGUILayout.LabelField("um", "yeah");
            EditorGUILayout.LabelField("ok");
        }
    }

}

