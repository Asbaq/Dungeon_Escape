using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespwan : MonoBehaviour
{
    public bool active = true;
    public Vector2 respawnPoint;
    public bool respawning = false;
    [SerializeField] private GameObject AttackArea;
     public float TimetoRespawn = 0f;
    public float currentRespawnTime = 0f;
    [SerializeField] private float startinghealth;
    public float currentHealth {get; private set; }
    private Animator anim;
    [SerializeField] private AudioSource PlayerDeath;
       void Awake()
    {
        respawnPoint = transform.position;   
        anim = GetComponent<Animator>();
    }
      void Start()
    {
        respawnPoint = transform.position;   
    }

     public void checkrespwaning()
    {
        if(GetComponent<PlayerHealth>().respawning)
        {
            currentRespawnTime += Time.deltaTime;
            if(currentRespawnTime >= TimetoRespawn)
            {
                currentRespawnTime = 0f;
                GetComponent<PlayerHealth>().respawning = false;
               RespawnPlayer();
            }
        }
    }

    public void playerDefeated()
    {
        GetComponent<PlayerMovement>().enabled = false;
        anim.SetTrigger("hurt");
        anim.SetTrigger("die");
        PlayerDeath.Play();
        StartCoroutine(WaitForSceneLoad());
        respawning = false;
    }

    public void RespawnPlayer()
    {
       transform.position = respawnPoint;
       GetComponent<PlayerMovement>().enabled = true;
    }

     private IEnumerator WaitForSceneLoad() 
     {
     yield return new WaitForSeconds(2);
     SceneManager.LoadScene("Game_Over"); 
     }
}
