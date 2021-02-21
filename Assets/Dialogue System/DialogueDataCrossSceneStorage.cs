using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class utilizes DontDestroyOnLoad in order to remember whether a player
// has read dialogue before and prevent dialogue they've already seen from appearing.
// Normally, the data would be lost when the scene is reloaded (which happens when the player dies)
public class DialogueDataCrossSceneStorage : MonoBehaviour
{
    public static DialogueDataCrossSceneStorage Instance;

    private bool[] StartDialoguesRead;
    private bool[] EndDialoguesRead;

    // Awake is called before Start() (which is called before first frame update)
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        StartDialoguesRead = new bool[SceneManager.sceneCountInBuildSettings];
        EndDialoguesRead = new bool[SceneManager.sceneCountInBuildSettings];
    }

    // Marks the current scene's starting dialogue as already heard.
    public void AlreadyHeardStart() {
        StartDialoguesRead[SceneManager.GetActiveScene().buildIndex] = true;
    }

    // Marks the current scene's ending dialogue as already heard.
    public void AlreadyHeardEndDialogue() {
        EndDialoguesRead[SceneManager.GetActiveScene().buildIndex] = true;
    }

    // Returns whether or not the current scene's start dialogue has been played before
    public bool CurrentStartDialogueRead() {
        return StartDialoguesRead[SceneManager.GetActiveScene().buildIndex];
    }

    // Returns whether or not the current scene's end dialogue has been played before
    public bool CurrentEndDialogueRead() {
        return EndDialoguesRead[SceneManager.GetActiveScene().buildIndex];
    }
}
