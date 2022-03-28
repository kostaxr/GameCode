using UnityEngine;
using System.Collections;

public class Enemy()
{
   private health = 100;
   
    AudioSource audioSource;

   void Start()
   {
       AudioSource = GetComponent<AudioSource>();
   }


   void OnCollisionEnter(Collision collision)
   {
       if (other.tag = "Player")
       {
           health -= 30;
       }
   }
}