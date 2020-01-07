﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Behaviour : MonoBehaviour
{

    private SpriteRenderer Sprite;
    public float ImputForce = 800;
    private CircleCollider2D CircleBomb;
    private Rigidbody2D RigiBomb;

    private GameObject slingshot;
    Vector2 SlingPos;
    Vector2 RedPos;
    Vector2 MaxPos;
    bool released = false;

    private bool IsDead = false;
    public GameObject BirdDeadAnim;
    public GameObject Explode;

    private Transform ThisPos;
    public Transform GCamera;

    float Lerp1 = 0;

    bool Exploted = false;

    private int DelayExp = 500;

    ScoreGM GameCont;


    private Transform Up;
    private Transform Down;
    private Transform Left;
    private Transform Right;

    private float MaxUp;
    private float MaxDown;
    private float MaxLeft;
    private float MaxRight;

    private Transform ExplosionArea;


    // Start is called before the first frame update
    void Start()
    {

        CircleBomb = GetComponent<CircleCollider2D>();
        RigiBomb = GetComponent<Rigidbody2D>();
        RigiBomb.isKinematic = true;

        slingshot = GameObject.Find("slingshot_1");
        SlingPos = Camera.main.ScreenToWorldPoint(slingshot.transform.position);

        Sprite = GetComponent<SpriteRenderer>();

        ThisPos = this.transform;
        GameCont = GameObject.Find("GameManager").GetComponent<ScoreGM>();

        Up = GameObject.Find("Sky").transform;
        Down = GameObject.Find("Floor").transform;
        Left = GameObject.Find("Border_L").transform;
        Right = GameObject.Find("Border_R").transform;

        MaxUp = Up.position.y;
        MaxDown = Down.position.y;
        MaxLeft = Left.position.x;
        MaxRight = Right.position.x;

        //ExplosionArea = GameObject.Find("Exp_Area_Effect").transform;

    }

    // Update is called once per frame
    void Update()
    {
        MaxPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - slingshot.transform.position;

        ExplosionArea = transform;

        if (Sprite)
        {
            if (Input.GetMouseButton(0) && !released && MaxPos.sqrMagnitude < 2.5f)
            {

                RigiBomb.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }

            if (Input.GetMouseButtonUp(0) && !released)
            {
                RedPos = Camera.main.ScreenToWorldPoint(this.transform.position); //hello, Im not ne

                released = true;
                RigiBomb.isKinematic = false;
                RigiBomb.AddForce((SlingPos - RedPos) * ImputForce, ForceMode2D.Impulse);


            }

            if (RigiBomb.velocity.magnitude < 2.0f && released)
            {

                StartCoroutine(DestroyAtTime(3));

            }
            else
            {

                StopCoroutine(DestroyAtTime(3));

            }

            if (released && !Exploted && Input.GetMouseButtonDown(0))
            {

                Exploted = true;

                Explosion();

            }

            if (transform.position.y > MaxUp || transform.position.y < MaxDown)
            {

                Dead();

            }
            else if (transform.position.x < MaxLeft || transform.position.x > MaxRight)
            {

                Dead();

            }

        }

        if (released && !IsDead)
        {
            Lerp1 = Lerp1 + 0.005f;

            if (Lerp1 > 1)
            {

                Lerp1 = 1f;

            }

            GCamera = GameObject.Find("Main Camera").GetComponent<Transform>();


            GCamera.transform.position = Vector2.Lerp(GCamera.transform.position, ThisPos.position, Lerp1);


            GCamera.transform.position += new Vector3(0, 0, -10);

        }


    }

    void Explosion()
    {

        var Bomb = Instantiate(Explode, transform.position, Quaternion.identity);

        CircleBomb.isTrigger = true;
        CircleBomb.usedByEffector = true;

        if (!IsDead)
        {
            
            IsDead = true;
            GameCont.DelayAnotherBird = 100;
            Destroy(Sprite);
            Destroy(gameObject, 0.30f);
            Destroy(Bomb, 0.25f);

            DelayExp += -1;

            if (DelayExp <= 0)
            {

                GCamera.transform.position = new Vector3(0, 0, -10);

            }
        }


    }


    private void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "Wood")
        {

            ImputForce += -100.0f;

        }
        else if (coll.gameObject.tag == "Rock")
        {

            ImputForce += -200.0f;

        }

    }

    IEnumerator DestroyAtTime(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        Dead();
        GCamera.transform.position = new Vector3(0, 0, -10);
        //Destroy(this.gameObject);
        StopCoroutine(DestroyAtTime(3));

    }

    void Dead()
    {

        if (DelayExp == 100)
        {

            Destroy(Sprite);
            Destroy(CircleBomb);
            Destroy(RigiBomb);
            if (!IsDead)
            {

                var Puff = Instantiate(BirdDeadAnim, this.transform.position, Quaternion.identity);
                IsDead = true;
                GCamera.transform.position = new Vector3(0, 0, -10);
                Destroy(this.gameObject, 0.5f);
                //Destroy(this.gameObject, Audio.clip.length);
                Destroy(Puff, 0.5f);

            }
        }
    }
}