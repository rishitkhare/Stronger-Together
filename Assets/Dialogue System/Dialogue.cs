using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    [TextArea(3, 10)]
    public string initialString;
    
    //speaker name
    [HideInInspector]
    public string name;

    [HideInInspector]
    public Sprite portrait;

    [HideInInspector]
    public string dialogText;

    //This constructor creates a dialogue by parsing a single line of the txt.
    public Dialogue(string parseText) {
        initialString = parseText;

        Parse();
    }

    public void Parse() {

        //Get name of portrait and use sprite indexer to find the sprite
        string portraitName = initialString.Substring(0, initialString.IndexOf(';'));
        portrait = DialogueManager.Instance.spriteArray.GetSprite(portraitName);

        //use the portrait name to find the name of the person speaking
        if (portraitName.Contains("_")) {
            //if the portrait has a name such as "spy_angry", truncate the portrait name
            name = portraitName.Substring(0, portraitName.IndexOf('_'));
        }
        else {
            name = portraitName;
        }


        //add 2 to the index because we want to eliminate "; "
        dialogText = initialString.Substring(initialString.IndexOf(';') + 2);
    }
}