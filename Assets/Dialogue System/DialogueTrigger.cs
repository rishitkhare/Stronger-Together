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
        buildArray(BeforeText, out BeforeDialogue);
        if(AfterText != null)
        {
            buildArray(AfterText, out AfterDialogue);
        }

        DialogueManager.Instance.StartDialogue(BeforeDialogue);
        waitForTrigger();
    }

    private void Update()
    {

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

    private void waitForTrigger()
    {
        bool success = true;
        while(success)
        {
            if(Input.GetKeyDown(advanceTextKey))
            {
                DialogueManager.Instance.DisplayNextSentence(out success);
            }
        }
    }

}
