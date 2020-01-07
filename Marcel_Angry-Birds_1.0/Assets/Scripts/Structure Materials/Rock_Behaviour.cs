﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Behaviour : MonoBehaviour
{
    

    private Collider2D ColliRock;
    private Rigidbody2D RigiRock;
    public float HP = 200;
    private float MaxHP;
    public float Impact = 10f;

    ScoreGM GameCont;

    public SpriteRenderer SpriteRend;
    bool Ungravitie;


    private Transform Up;
    private Transform Down;
    private Transform Left;
    private Transform Right;

    private float MaxUp;
    private float MaxDown;
    private float MaxLeft;
    private float MaxRight;


    // Start is called before the first frame update
    void Start()
    {

        ColliRock = GetComponent<Collider2D>();
        RigiRock = GetComponent<Rigidbody2D>();
        MaxHP = HP;

        GameCont = GameObject.Find("GameManager").GetComponent<ScoreGM>();
        SpriteRend = this.GetComponent<SpriteRenderer>();

        if (SpriteRend.color == new Color(255, 0, 0, 230))
        {

            Ungravitie = true;

        }
        else
        {

            Ungravitie = false;

        }


        Up = GameObject.Find("Sky").transform;
        Down = GameObject.Find("Floor").transform;
        Left = GameObject.Find("Border_L").transform;
        Right = GameObject.Find("Border_R").transform;

        MaxUp = Up.position.y;
        MaxDown = Down.position.y;
        MaxLeft = Left.position.x;
        MaxRight = Right.position.x;

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y > MaxUp || transform.position.y < MaxDown)
        {

            HP = 0;

        }
        else if (transform.position.x < MaxLeft || transform.position.x > MaxRight)
        {

            HP = 0;

        }

        if (HP <= 0)
        {

            GameCont.score += 500;
            Destroy(this.gameObject);

        }

        if (Ungravitie == true)
        {

            if (RigiRock.transform.position.y >= 4)
            {

                RigiRock.AddForce(RigiRock.velocity * new Vector2(1, 1 / -50) + new Vector2(Random.value, -10));

            }
            else if (RigiRock.transform.position.x <= -5.5)
            {

                RigiRock.AddForce(new Vector2(-RigiRock.velocity.x + 20, 0));

            }
            else if (RigiRock.transform.position.x >= 5.5)
            {

                RigiRock.AddForce(new Vector2(-RigiRock.velocity.x - 20, 0));

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "Bird" || coll.gameObject.tag == "Pig" || coll.gameObject.tag == "Wood")
        {

            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 8;
            HP += -Damage;


        }
        else if (coll.gameObject.tag == "Rock")
        {
            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 11.0f;
            HP += -Damage;
        }
        else if (coll.gameObject.tag == "Floor" && RigiRock.velocity.y * RigiRock.mass < 2.0f)
        {

            float Damage = Mathf.Pow(coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 15, 0.5f);
            HP += -Damage;

        }




    }
}