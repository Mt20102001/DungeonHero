using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Player : MonoBehaviour
{
    #region Public Value
    public Animator animator;

    #endregion

    #region Private Value
    [SerializeField]
    private GameObject Player;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack");
            Player.GetComponent<Movement_Player>().CanMove = false;
            Invoke("WaitAttack", 0.8f);
        }
    }

    private void WaitAttack()
    {
        Player.GetComponent<Movement_Player>().CanMove = true;
    }
}
