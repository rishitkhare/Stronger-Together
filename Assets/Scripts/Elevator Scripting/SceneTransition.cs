using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    public ElevatorScript greenElevator;
    public ElevatorScript purpleElevator;

    private Animator anim;

    private readonly int FadeToBlackHash = Animator.StringToHash("Fade to Black");

    // Start is called before the first frame update
    void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (greenElevator.IsOpen && purpleElevator.IsOpen) {
            LevelEnd();
        }
    }

    private void LevelEnd() {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("TransitionScene")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        anim.SetTrigger(FadeToBlackHash);
    }
}
