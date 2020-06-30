﻿using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public bool facingRight = true;
    public bool isGrounded = true;
    public int coinCount = 0;
    public int lifes = 3;
    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject IBUTTON;
    private Animator anim;
    public float forcaPulo;
    public float velocidadeMaxima;
    public GameObject player;
    public GameObject LastCheckpoint;
    
    
    
    void Start()
    {
        IBUTTON.GetComponent<Renderer>().enabled = false;
        anim = gameObject.GetComponent<Animator>();
        print("eae jogo bosta");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        anim.speed = 0.5f;
       
       //Movimentação lateral do personagem

       Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        float movimento =0;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
             movimento = Input.GetAxis("Horizontal");
        }
               

       rigidbody.velocity = new Vector2(movimento*velocidadeMaxima,rigidbody.velocity.y);

       if(movimento > 0)
       {
           FlipLeft();
	   } else if (movimento < 0)
       {
          FlipRight();
	   }
      
        //Movimentação vertical do personagem

        if (Input.GetKeyDown(KeyCode.W) && isGrounded == true  && isGrounded == true)      //move pra cima (pula)
        {
            Jump();
        }
       
        if (Input.GetKey(KeyCode.S) && isGrounded == false)         //move pra baixo quando estiver no ar
        {
            rb.velocity = new Vector2(rb.velocity.x,-8f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position = LastCheckpoint.transform.position;  
		}
    }
    
    
    public void ToogleVisibility(){

        Renderer rend = IBUTTON.GetComponent<Renderer>();
        if (rend.enabled == false)
            rend.enabled = true;
        else
            rend.enabled = false;
    }
    // Pulo
    void Jump()                         
    {
        rb.AddForce(new Vector2(rb.velocity.x,forcaPulo));      
    }

    void OnCollisionEnter2D(Collision2D collision)
    {  
          if (collision.gameObject.CompareTag("Spike"))
            transform.position = LastCheckpoint.transform.position;

        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Chest")      //quando bate em algo
            || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Plataforma") )
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            print("qtd de coins: " + coinCount);
        }
        if (collision.gameObject.CompareTag("Chest"))
        {
            ToogleVisibility();
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            lifes--;
            print("-1 de vida, vidas restantes: " + lifes);
            //if (lifes == 0)
            // {
            //    Destroy(gameObject);
            //}
            switch (lifes)
            {
                case 0:
                    Destroy(l1);
                    break;
                case 1:
                    Destroy(l2);
                    break;
                case 2:
                    Destroy(l3);
                    break;
                default:
                    print("");
                    break;
            }
        }
        if (collision.gameObject.CompareTag("Plataforma"))
           player.transform.parent = collision.gameObject.transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Chest")       //quando bate em algo
            || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Plataforma"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Plataforma"))
             player.transform.parent = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         //checkpoint

        if (collision.gameObject.CompareTag("Checkpoint"))         
        {
            Debug.Log("Colidiu com checkpoint: "+ collision.gameObject.name);
            LastCheckpoint = collision.gameObject;
		}   

        //barreira invisível que mata

        if (collision.gameObject.CompareTag("Kill"))                
        {
            transform.position = LastCheckpoint.transform.position;  
		}
        
        //controle de vidas (no momento está sendo ignorado)

        if (collision.gameObject.CompareTag("Impact"))
        {
            lifes--;
            print("-1 de vida, vidas restantes: " + lifes);
            //if (lifes == 0)
            //{
            //    Destroy(gameObject);
            //}
            switch (lifes)
            {
                case 2:
                    Destroy(l1);
                    break;
                case 1:
                    Destroy(l2);
                    break;
                case 0:
                    Destroy(l3);
                    break;
                default:
                    print("");
                    break;
            }
        }
        if (collision.gameObject.CompareTag("Plataforma") && isGrounded )
        {
         isGrounded = false;  
         player.transform.parent = collision.gameObject.transform;
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.transform.parent = null;
	}

    void FlipRight()
    {
        if (facingRight == true)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
    }
    void FlipLeft()
    {
        if (facingRight == false)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
    }
}  