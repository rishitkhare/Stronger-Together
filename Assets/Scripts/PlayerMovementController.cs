using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 1.0f;
    public string inputHorizontalName;
    public float sneakFactor;
    public float sneakNoiseReduction;
    public KeyCode sneakButton;
    public string inputVerticalName;

    SimpleRigidbody collisionHandler;
    Animator anim;
    SpriteRenderer sp;

    Vector2 direction;

    #region AnimatorHashes
    //optimizing the animator
    int isMovingHash = Animator.StringToHash("IsMoving");
    int DirectionXHash = Animator.StringToHash("DirectionX");
    int DirectionYHash = Animator.StringToHash("DirectionY");
    #endregion AnimatorHashes
    public float footStepsNoise;
    [HideInInspector]
    public bool IsInteractingWithComputer { get; set; }
    void Start()
    {
        collisionHandler = gameObject.GetComponent<SimpleRigidbody>();
        anim = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw(inputHorizontalName), Input.GetAxisRaw(inputVerticalName));
        input.Normalize();

        if(!IsInteractingWithComputer)
        {
            if (Input.GetKey(sneakButton))
            {
                collisionHandler.SetVelocity((input * Speed) / sneakFactor);
            }
            else
            {
                collisionHandler.SetVelocity(input * Speed);
            }


            if (collisionHandler.GetVelocity() != Vector2.zero)
            {
                
            }


            AnimateCharacter(input);
        }
        else {
            //animator is no longer moving
            anim.SetBool(isMovingHash, false);
        }
    }

    private void AnimateCharacter(Vector2 input)
    {
        if (input.Equals(Vector2.zero))
        {
            anim.SetBool(isMovingHash, false);
        }
        else if (Mathf.Abs(input.x) == 1f || Mathf.Abs(input.x) == 0f) //ignore diagonals
        {
            direction = input;
            anim.SetBool(isMovingHash, true);
            anim.SetFloat(DirectionXHash, direction.x);
            anim.SetFloat(DirectionYHash, direction.y);
        }

        if(direction.x > 0)
        {
            sp.flipX = false;
        }
        else
        {
            sp.flipX = true;
        }
    }
}
