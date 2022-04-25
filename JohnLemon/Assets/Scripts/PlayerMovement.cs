using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    Vector3 m_Movement;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    public float turnSpeed = 20f;
    float idleTime = 0.4f;
    float timer = 0.0f;
    Quaternion m_Rotation = Quaternion.identity;
    AudioSource m_AudioSource;
    


    // Start is called before the first frame update
    void Start(){
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource> ();
        
    }

    // Update is called once per frame
    void FixedUpdate (){
        // called before the physics system solves any collisions and other interactions that have happened
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize(); 
        
        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f); // true when horizontal is non-zero
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f); // true when vertical is non-zero
       
        bool isWalking = hasHorizontalInput || hasVerticalInput; // true when horizontal or vertical is non-zero

        if(!isWalking){
            timer += Time.deltaTime; // activate timer
            if(timer >= idleTime){
                m_Animator.SetBool("IsWalking", false); // play IO animation onlt if stopped moving for more than 0.4 sec
                timer = 0f;
                m_AudioSource.Stop();
            }

        }
        else{
            m_Animator.SetBool("IsWalking", true); 
            timer = 0f;
           if (!m_AudioSource.isPlaying){
                m_AudioSource.Play();
            }
        }
        

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f); // where to rotate
        m_Rotation = Quaternion.LookRotation (desiredForward); // actual rotate


    }

    void OnAnimatorMove (){

        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);

    }


   
}
