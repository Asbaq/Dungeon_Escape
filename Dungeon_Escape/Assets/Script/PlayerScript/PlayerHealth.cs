using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool respawning = false;
    public float TimetoRespawn = 0f;
    public float currentRespawnTime = 0f;
    public bool active = true;
    public Vector2 respawnPoint;
    private float x = 2f; 
    [SerializeField] private float startinghealth;
    public float currentHealth {get; private set; }
     private Animator anim;
    [SerializeField] private GameObject AttackArea;
    [SerializeField] private AudioSource GateSound;
    [SerializeField] private AudioSource Checkpoints;
    [SerializeField] private AudioSource EvilSound;
    private void Awake()
    {
        currentHealth = startinghealth;
        anim = GetComponent<Animator>();
    }

     void Start()
    {
        respawnPoint = transform.position;   
    }

    void Update()
    {  
        GetComponent<PlayerRespwan>().checkrespwaning();

        if(!active)
        {
            return;
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startinghealth);

         if(currentHealth > 0)
         {
           anim.SetTrigger("hurt");
           GetComponent<PlayerMovement>().enabled = false;
         }
         else if(currentHealth <=0)
         {
             GetComponent<PlayerRespwan>().playerDefeated();
         }
    }

     private IEnumerator WaitForSceneLoad1() 
     {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("Level_1_UI"); 
     }

     private IEnumerator WaitForSceneLoad2() 
     {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("Level_2_UI"); 
     }

     private IEnumerator WaitForSceneLoad3() 
     {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("Level_3_UI"); 
     }

     private IEnumerator WaitForSceneLoad4() 
     {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("Complete_Game"); 
     }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.tag == "Enemy" && AttackArea.activeInHierarchy == false)
        {
            TakeDamage(1);
            if(currentHealth > 0)
            {
            respawning = true;
            }
        }

        if(other.tag == "Ladders")
        {
            anim.SetTrigger("Climb");
        }

        if(other.tag == "MonsterCollider")
        {
            EvilSound.Play();
        }
        
        if (other.tag == "Checkpoints")
        {
            Checkpoints.Play();
            respawnPoint = other.transform.position;
        }

        if (other.tag == "End1" )
        {   
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad1());
        }

        if (other.tag == "End2")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad2());
        }

        if (other.tag == "End3")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad3());
        }

        if (other.tag == "End4")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad4());
        }
    }
}
