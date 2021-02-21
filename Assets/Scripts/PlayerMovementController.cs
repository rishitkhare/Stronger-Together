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

    private bool lockPlayerMovement;

    SimpleRigidbody collisionHandler;
    Animator anim;
    SpriteRenderer sp;
    NoiseEmitter myEmitter;

    Vector2 direction = Vector2.down;

    #region AnimatorHashes
    //optimizing the animator
    int isMovingHash = Animator.StringToHash("IsMoving");
    int DirectionXHash = Animator.StringToHash("DirectionX");
    int DirectionYHash = Animator.StringToHash("DirectionY");
    #endregion AnimatorHashes
    public float footStepsNoise;
    [HideInInspector]
    public bool IsInteractingWithComputer { get; set; }

    #region Event Methods
    public void LockMovement() {
        lockPlayerMovement = true;
    }

    public void UnlockMovement() {
        lockPlayerMovement = false;
    }

    #endregion Event Methods

    void Start()
    {
        collisionHandler = gameObject.GetComponent<SimpleRigidbody>();
        myEmitter = gameObject.GetComponent<NoiseEmitter>();
        anim = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw(inputHorizontalName), Input.GetAxisRaw(inputVerticalName));
        input.Normalize();
        float sneaknoise = 0;

        // We probably should've just used the one lockPlayerMovement boolean,
        // but I'm afraid of what I'm gonna break
        //
        // - Rishu
        if(!lockPlayerMovement && !IsInteractingWithComputer)
        {
            if (Input.GetKey(sneakButton))
            {
                collisionHandler.SetVelocity((input * Speed) / sneakFactor);
                sneaknoise = sneakNoiseReduction;
            }
            else
            {
                collisionHandler.SetVelocity(input * Speed);
            }
            if(collisionHandler.GetVelocity() != Vector2.zero)
            {
                myEmitter.createNoise.Invoke(new Vector3(transform.position.x, transform.position.y, footStepsNoise - sneaknoise), gameObject);
            }
            AnimateCharacter(input);
        }
        else {
            //do not retain velocity once locked
            anim.SetBool(isMovingHash, false);
            collisionHandler.SetVelocity(Vector2.zero);
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
        else {
            anim.SetBool(isMovingHash, true);
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
