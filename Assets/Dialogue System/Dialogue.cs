using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue {

    //speaker name
    public string name;

    public Sprite portrait;

    public string dialogText;

    //This constructor creates a dialogue by parsing a single line of 
    public Dialogue(string parseText) {
        //Get name of portrait and use sprite indexer to find the sprite
        string portraitName = parseText.Substring(0, parseText.IndexOf(';'));
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
        dialogText = parseText.Substring(parseText.IndexOf(';') + 2);
    }
}