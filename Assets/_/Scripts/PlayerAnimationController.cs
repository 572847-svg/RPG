using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
        { 


        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }


        // Example 2: Triggering via Movement Input (Alternative)
        // Uncomment this section if you want it to trigger when the player moves
       
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
       
        // If there is movement input, set Is Running to true
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
 
    }
}


