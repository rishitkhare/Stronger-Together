using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneTransition : MonoBehaviour
{
    private bool nextLevel;

    private Animator anim;

    private readonly int FadeToBlackHash = Animator.StringToHash("Fade to Black");

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        nextLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextLevel)
        {
            LevelEnd();
        }
    }

    public void GoToNextLevel()
    {
        AudioManager.instance.Play("Climbing Upstairs");
        nextLevel = true;
    }

    private void LevelEnd()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TransitionScene"))
        {
            //load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        anim.SetTrigger(FadeToBlackHash);
    }
}
