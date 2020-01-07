using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig_Behaviour : MonoBehaviour
{

    private Collider2D ColliPig;
    private Rigidbody2D RigiPig;
    public float HP = 30;
    private float MaxHP;
    public float Impact = 10f;

    ScoreGM GameCont;


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

        ColliPig = GetComponent<Collider2D>();
        RigiPig = GetComponent<Rigidbody2D>();
        MaxHP = HP;

        GameCont = GameObject.Find("GameManager").GetComponent<ScoreGM>();


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
        if (HP <= 0)
        {

            GameCont.score += 1000;
            Destroy(this.gameObject);

        }

        if (transform.position.y > MaxUp || transform.position.y < MaxDown)
        {

            HP = 0;

        }
        else if (transform.position.x < MaxLeft || transform.position.x > MaxRight)
        {

            HP = 0;

        }

    }


    private void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "Pig" || coll.gameObject.tag == "Wood")
        {

            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            HP += -Damage;


        }
        else if (coll.gameObject.tag == "Rock")
        {

            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 20;
             HP += -Damage;

        }
        else if (coll.gameObject.tag == "Bird" || coll.gameObject.tag == "Floor")
        {

            HP = 0;

        }
    }
}
