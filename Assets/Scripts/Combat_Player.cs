using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Player : MonoBehaviour
{
    #region Public Value
    public Animator animator;
    public bool CanAttack;

    #endregion

    #region Private Value

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && CanAttack)
        {
            animator.SetTrigger("Attack");

            // Stop moving until animations attack done
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                gameObject.GetComponent<Movement_Player>().CanMove = false;
                Invoke("CanMovement", 0.9f);
            } 
        }
    }

    private void CanMovement()
    {
        gameObject.GetComponent<Movement_Player>().CanMove = true;
    }

}
