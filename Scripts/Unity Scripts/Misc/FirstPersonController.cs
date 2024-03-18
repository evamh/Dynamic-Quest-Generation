using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

/**
    Singleton class to manage all first person controller logic 
    Can configure the movement speed, the jump speed and the mouseSensitivity
    ChatGPT helped quite a bit with this 
**/

public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController current;

    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float movementThreshold = 0.0f;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 velocity = Vector3.zero;
    private float gravity = 9.81f;

    // Animations
    Animator animator;

    [SerializeField] private Transform cameraTransform;
    public Transform PlayerTransform;

    private string animatorParameter;

    void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        animatorParameter = "isWalking";
        //offAnimatorParameter = "isRunning";

        PlayerTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!UIManager.current.IsActiveUIElement) Movement();
    }

    // Function to move the player
    private void Movement()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true; 

        float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed;
        float verticalMovement = Input.GetAxis("Vertical") * movementSpeed;

        // Apply jump
        if(Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpSpeed;
        }
        
        // Apply gravity
        if(!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        } 

        // Apply horizontal movement
        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement);
        movement = transform.rotation * movement;
        if(movement.magnitude > movementThreshold)
        {
            animator.SetBool(animatorParameter, true);
            //animator.SetBool(offAnimatorParameter, false);
        } else {
            animator.SetBool(animatorParameter, false);
            //animator.SetBool(offAnimatorParameter, true);
        }

        controller.Move(movement * Time.deltaTime);

        // Apply vertical movement
        verticalRotation += Input.GetAxis("VerticalRotation") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -20f, 20f);

        horizontalRotation = Input.GetAxis("HorizontalRotation") * mouseSensitivity;
        transform.Rotate(0f, horizontalRotation, 0f);

        // Apply velocity
        controller.Move(velocity * Time.deltaTime);
    }

    // Check if the cursor is over a game object 
    private bool CursorIsOverUIElement()
    {
        bool overGameObject = EventSystem.current.IsPointerOverGameObject();
        Debug.Log("[CursorIsOverUIElement] is over game object?: " + overGameObject);
        return overGameObject;
    }
}
