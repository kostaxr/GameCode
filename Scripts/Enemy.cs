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

   void OnCollisionEnter(Collision collision)
   {
       if (collision.tag == "Player")
       {
           health -= 30;
       }
   }
}