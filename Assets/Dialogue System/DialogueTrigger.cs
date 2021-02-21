using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public TextAsset BeforeText;
    public TextAsset AfterText;

    public KeyCode advanceTextKey;

    private Dialogue[] BeforeDialogue;
    private Dialogue[] AfterDialogue;


    private void Start()
    {

        //avoids nullReference shenanigans
        if(BeforeText != null) {
            buildArray(BeforeText, out BeforeDialogue);
        }

        if(AfterText != null)
        {
            buildArray(AfterText, out AfterDialogue);
        }

        PlayStartingDialogue();
    }

    private void Update()
    {
        if(DialogueManager.Instance != null) {


            if(Input.GetKeyDown(advanceTextKey) && !DialogueManager.Instance.IsTyping) {
                DialogueManager.Instance.DisplayNextSentence();
            }

            if(DialogueManager.Instance.IsTyping) {
                if(Input.GetKey(advanceTextKey)) {
                    DialogueManager.Instance.IsFaster = true;
                }
                else {
                    DialogueManager.Instance.IsFaster = false;
                }
            }
        }
    }

    private void buildArray(TextAsset file, out Dialogue[] output)
    {
        string[] temp = file.text.Split('\n');
        output = new Dialogue[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            output[i] = new Dialogue(temp[i]);
        }
    }

    public void PlayStartingDialogue() {
        Debug.Log("Play start");
        // don't replay the same dialogue
        if (DialogueDataCrossSceneStorage.Instance != null && !DialogueDataCrossSceneStorage.Instance.CurrentStartDialogueRead()) {
            DialogueDataCrossSceneStorage.Instance.AlreadyHeardStart();
            DialogueManager.Instance.StartDialogue(BeforeDialogue);
        }
    }

    public void PlayLevelEndDialogue() {
        // don't replay the same dialogue
        if(DialogueDataCrossSceneStorage.Instance != null && !DialogueDataCrossSceneStorage.Instance.CurrentEndDialogueRead()) {
            DialogueDataCrossSceneStorage.Instance.AlreadyHeardEndDialogue();
            DialogueManager.Instance.StartDialogue(AfterDialogue);
        }
    }
}
