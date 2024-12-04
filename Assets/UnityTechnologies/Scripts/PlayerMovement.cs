using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float sprintSpeed; // Minor code added for speed boost. It is activated by pressing the sprint key - LF.
    public float inactivityTimeout = 5f; // Time after which the character stops moving (5 seconds) - CB
    public int spacebarPressRequired = 3; // Number of spacebar presses needed to resume movement - CB

    private float timeSinceLastMove = 0f;
    private int spacebarPressCount = 0;
    private bool canMove = true;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }

            timeSinceLastMove = 0f;
        }
        else
        {
            m_AudioSource.Stop();

            timeSinceLastMove += Time.deltaTime;
        }

        
        if (timeSinceLastMove >= inactivityTimeout)
        {
            canMove = false;
            m_Animator.SetBool("IsWalking", false);
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        if (Input.GetKey(KeyCode.LeftShift)) //Sprinting code - LF
        {
            m_Movement *= sprintSpeed;
        }
    }

    void OnAnimatorMove()
    {
        if (canMove)
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
    }

    void Update()
    {

        if (!canMove && Input.GetKeyDown(KeyCode.Space))
        {
            spacebarPressCount++;


            if (spacebarPressCount >= spacebarPressRequired)
            {
                canMove = true;
                spacebarPressCount = 0;
                timeSinceLastMove = 0f;
            }
        }
    }
}
