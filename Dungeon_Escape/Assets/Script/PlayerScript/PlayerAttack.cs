using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private Animator anim;
    [SerializeField] private AudioSource Attacks;
    // bool boo;
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // boo = GetComponent<AttackButton>().Attacks();
        // Debug.Log(boo);
        // if(boo)
        // {
        //     Attack();
        // }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }

       // Attack();

        if(attacking)
        {
            timer += Time.deltaTime;

            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    public void Attack()
    {
        anim.SetTrigger("attack");
        Attacks.Play();
        attacking = true;
        attackArea.SetActive(attacking);
    }
}
