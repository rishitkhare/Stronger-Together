using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    //speaker name
    public string name;

    public Sprite portrait;

    [TextArea(3, 10)]
    public string[] sentences;

}