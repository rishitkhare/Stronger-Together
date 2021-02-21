using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class SceneTransition : MonoBehaviour {
    public ElevatorScript greenElevator;
    public ElevatorScript purpleElevator;

    //used for triggering end scene dialogue
    private DialogueTrigger dialogueTrigger;

    private bool resetLevel;

    private bool nextLevel;

    private Animator anim;

    private readonly int FadeToBlackHash = Animator.StringToHash("Fade to Black");

    // Start is called before the first frame update
    void Start() {
        // gets components
        anim = gameObject.GetComponent<Animator>();
        if(DialogueManager.Instance != null) {
            dialogueTrigger = DialogueManager.Instance.gameObject.GetComponent<DialogueTrigger>();
        }
    }

    // Update is called once per frame
    void Update() {
        if(DialogueManager.Instance != null) {
            if (greenElevator.IsOpen && purpleElevator.IsOpen
                && !DialogueManager.Instance.LoadNextSceneOnDialogueEnd) {
                DialogueManager.Instance.LoadNextSceneOnDialogueEnd = true;
                dialogueTrigger.PlayLevelEndDialogue();
            }
        }
        else if(!nextLevel && greenElevator.IsOpen && purpleElevator.IsOpen) {
            GoToNextLevel();
        }
        if(nextLevel) {
            LevelEnd();
        }
        else if(resetLevel) {
            FadeToBlackAndReload();
        }
    }

    public void RestartLevel() {
        Light2D light = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        light.color = Color.red;
        resetLevel = true;
    }

    public void GoToNextLevel() {
        AudioManager.instance.Play("Climbing Upstairs");
        nextLevel = true;
    }

    private void LevelEnd() {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("TransitionScene")) {
            //load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        anim.SetTrigger(FadeToBlackHash);
    }

    private void FadeToBlackAndReload() {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("TransitionScene")) {
            //reload current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        anim.SetTrigger(FadeToBlackHash);
    }
}
