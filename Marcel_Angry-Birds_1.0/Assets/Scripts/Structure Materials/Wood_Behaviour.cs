using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood_Behaviour : MonoBehaviour
{

    private Collider2D ColliWood;
    private Rigidbody2D RigiWood;
    public float HP = 100;
    private float MaxHP;
    public float Impact = 10f;

    //GameObject Score = GameObject.Find("GameManager");
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

        ColliWood = GetComponent<Collider2D>();
        RigiWood = GetComponent<Rigidbody2D>();
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
        
        if(HP <= 0)
        {

            GameCont.score += 300;
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
        
        if(coll.gameObject.tag == "Bird" || coll.gameObject.tag == "Pig" || coll.gameObject.tag == "Wood")
        {

            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            HP += -Damage;


        }

        if (coll.gameObject.tag == "Rock")
        {
            float Damage = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 20;
            HP += -Damage;
        }
        else if (coll.gameObject.tag == "Floor" && (RigiWood.velocity.y * RigiWood.mass) < 2.0f)
        {

            float Damage = Mathf.Pow(coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 13, 0.5f);
            HP += -Damage;

        }




    }
}
