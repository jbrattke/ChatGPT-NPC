using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravity = 9.8f;
    public float airControl = 10f;
    public float interactDistance = 4f;
    CharacterController controller;
    MouseLook mouseLook;
    Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mouseLook = GetComponentInChildren<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseLook.isUIActive)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;

      
        if (controller.isGrounded)
        {
            moveDirection = input;

            moveDirection.y = 0.0f;
        }
        else
        {
            // we are midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        // apply gravity 
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

        NPCInteraction();
    }

    void NPCInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
            {
                if (hit.collider.CompareTag("NPC"))
                {
                    hit.collider.GetComponent<NPCBehavior>().StartConversation();
                }
            }
        }
    }
}