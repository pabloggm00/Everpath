using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public CharacterController cc;
    public float speed;
    public float moreSpeed;
    float horizontalInput;
    float verticalInput;
    Vector2 movement;

    bool facingRight = true;
    bool pararAtras;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    
    // Update is called once per frame
    void Update()
    {
        GetMoveInputs();
        Move();
    }

    #region Movimiento

    void GetMoveInputs()
    {
        if (!DialogueSystem.instance.isConversacion)
        {
            
            movement = InputManager.playerControls.Player.Move.ReadValue<Vector2>();

            horizontalInput = movement.x * speed;
            verticalInput = movement.y * speed;


            if(horizontalInput < 0f && facingRight || horizontalInput > 0f && !facingRight)
            {
                FlipCharacter();
            }

            if (Gamepad.all.Count > 0)
            {
                switch (InputManager.GetControlDeviceType())
                {
                    case InputManager.ControlDeviceType.KeyboardAndMouse:
                        if (!Keyboard.current.sKey.IsPressed() && (Keyboard.current.dKey.IsPressed() || Keyboard.current.aKey.IsPressed()))
                        {
                            anim.SetBool("isWalkSide", true);
                            anim.SetBool("pararAtras", false);
                            pararAtras = false;
                        }
                        else
                        {
                            anim.SetBool("isWalkSide", false);
                        }
                        break;
                    case InputManager.ControlDeviceType.Gamepad:
                        if (!Gamepad.current.leftStick.down.IsActuated() && (Gamepad.current.leftStick.right.IsActuated() || Gamepad.current.leftStick.left.IsActuated()))
                        {
                            anim.SetBool("isWalkSide", true);
                            anim.SetBool("pararAtras", false);
                            pararAtras = false;
                        }
                        else
                        {
                            anim.SetBool("isWalkSide", false);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (!Keyboard.current.sKey.IsPressed() && (Keyboard.current.dKey.IsPressed() || Keyboard.current.aKey.IsPressed()))
                {
                    anim.SetBool("isWalkSide", true);
                }
                else
                {
                    anim.SetBool("isWalkSide", false);
                }
            }
            
            

            if (movement.y > 0 && movement.y != 0)
            {
                anim.SetBool("isWalkBack", true);
                anim.SetBool("pararAtras", true);
                pararAtras = true;
                anim.SetBool("isWalkFront", false);
            }
            else if(movement.y < 0 && movement.y != 0)
            {
                anim.SetBool("isWalkFront", true);
                anim.SetBool("pararAtras", false);
                pararAtras = false;
                anim.SetBool("isWalkBack", false);
            }

            if (movement.y == 0)
            {
                anim.SetBool("isWalkFront", false);
                anim.SetBool("isWalkBack", false);

                if (pararAtras)
                {
                    anim.SetBool("isIdleBack", true);
                }
                else
                {
                    anim.SetBool("isIdleBack", false);
                }
                
            }
            
        }
        else
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
        
    }

    void FlipCharacter()
    {

        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
       
    }

    void Move()
    {


        //MOVIMIENTO CHARACTER CONTROLLER
        cc.Move(new Vector3(horizontalInput, 0f, verticalInput) * Time.deltaTime);

        //MOVIEMIENTO RIGIDBODY

        /*float horizontalVelocity = rb.velocity.x;
        float verticalVelocity = rb.velocity.z;

        horizontalVelocity += horizontalInput;
        verticalVelocity += verticalInput;

        rb.velocity = new Vector3(horizontalVelocity, 0f, verticalVelocity);*/


    }

    public void GetRunInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Run();
        }
        else if (context.canceled)
        {
            speed -= moreSpeed;
        }
    }

    void Run()
    {
        speed += moreSpeed;
    }

    #endregion
}
