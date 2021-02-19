using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    public ElevatorScript greenElevator;
    public ElevatorScript purpleElevator;

    private bool resetLevel;

    private bool nextLevel;

    private Animator anim;

    private readonly int FadeToBlackHash = Animator.StringToHash("Fade to Black");

    // Start is called before the first frame update
    void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (greenElevator.IsOpen && purpleElevator.IsOpen) {
            nextLevel = true;
        }


        if(nextLevel) {
            LevelEnd();
        }
        else if(resetLevel) {
            FadeToBlackAndReload();
        }
    }

    public void RestartLevel() {
        resetLevel = true;
    }

    public void GoToNextLevel() {
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
        anim.SetTrigger(FadeToBlackHash)
    }
}
