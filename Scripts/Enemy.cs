using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
   private int health = 100;
   private float speed = 100f;
  
   AudioSource audioSource;
   private Transform player;
   void Awake()
   {
       transform.position = new Vector3(0.0f,0.0f,0.0f);
   }

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
     if (Vector3.Distance(transform.position,player.position)> 1.0f)
     {   
         var distance = speed*Time.deltaTime; 
         transform.position = Vector3.MoveTowards(transform.position,player.position,distance);
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