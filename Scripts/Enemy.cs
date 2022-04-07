using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
   private int health = 100;
   private float speed = 100f;

   
   AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameObject.tag = "Enemy";

    }
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.tag == "Log")
       {
           health -= 30;
       }
    }
}