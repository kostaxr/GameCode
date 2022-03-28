using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
   private int health = 100;
   
    AudioSource audioSource;


   void Start()
   {
       audioSource = GetComponent<AudioSource>();
       gameObject.tag = "Enemy";

   }


   private void OnTriggerEnter(Collider other)
   {
       if (other.tag == "Player")
       {
           health -= 30;
       }
   }
}