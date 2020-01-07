using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Behaviour : MonoBehaviour
{

    private SpriteRenderer Sprite;
    public float ImputForce = 800;
    private CircleCollider2D CircleBubble;
    private Rigidbody2D RigiBubble;

    private GameObject slingshot;
    Vector2 SlingPos;
    Vector2 RedPos;
    Vector2 MaxPos;
    bool released = false;

    private bool IsDead = false;
    public GameObject BirdDeadAnim;

    private Transform ThisPos;
    public Transform GCamera;

    float Lerp1 = 0;
    public bool Increased;


    private Transform Up;
    private Transform Down;
    private Transform Left;
    private Transform Right;

    private float MaxUp;
    private float MaxDown;
    private float MaxLeft;
    private float MaxRight;

    
    public int TIME = 300;

    public float Lerp2 = 0.0f;
    Vector2 LittleSc = new Vector2(0.14468f, 0.14468f);
    public bool Increasing = true;
    private readonly float IncSpeed = 0.1f;

    bool Little = true;

    public Sprite[] SpList;
    public Sprite ActSprite;
    SpriteRenderer SpriteRenderer;

    Vector2 radius;


    // Start is called before the first frame update
    void Start()
    {

        CircleBubble = GetComponent<CircleCollider2D>();
        RigiBubble = GetComponent<Rigidbody2D>();

        RigiBubble.isKinematic = true;
        
        slingshot = GameObject.Find("slingshot_1");
        SlingPos = Camera.main.ScreenToWorldPoint(slingshot.transform.position);

        Sprite = GetComponent<SpriteRenderer>();

        ThisPos = this.transform;


        Up = GameObject.Find("Sky").transform;
        Down = GameObject.Find("Floor").transform;
        Left = GameObject.Find("Border_L").transform;
        Right = GameObject.Find("Border_R").transform;

        MaxUp = Up.position.y;
        MaxDown = Down.position.y;
        MaxLeft = Left.position.x;
        MaxRight = Right.position.x;

        GCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        TIME += -1;

        MaxPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - slingshot.transform.position;

        if (Sprite)
        {
            if (Input.GetMouseButton(0) && !released && MaxPos.sqrMagnitude < 2.5f)
            {

                RigiBubble.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }

            if (Input.GetMouseButtonUp(0) && !released)
            {
                RedPos = Camera.main.ScreenToWorldPoint(this.transform.position);

                released = true;
                RigiBubble.isKinematic = false;
                RigiBubble.AddForce((SlingPos - RedPos) * ImputForce, ForceMode2D.Impulse);


            }

            if (RigiBubble.velocity.magnitude < 2.0f && released)
            {

                StartCoroutine(DestroyAtTime(3));

            }
            else
            {

                StopCoroutine(DestroyAtTime(3));

            }




            if (released && !Increased && Input.GetMouseButtonDown(0))
            {

                Increased = true;

                Little = false;
             
            }

            if (Increased)
            {

                if (Increasing)
                {

                    Lerp2 += IncSpeed;

                    if (Lerp2 >= 1)
                    {

                        Lerp2 = 1.0f;
                        TIME += -1;

                        if(TIME <= 0)
                        {

                            Increasing = false;

                        }

                    }

                }
                else
                {

                    Lerp2 += -IncSpeed;

                    if (Lerp2 < 0)
                    {

                        Lerp2 = 0.0f;
                        Little = true;

                    }

                }

            }

            if (Little)
            {

                ThisPos.localScale = Vector2.one;
                ActSprite = SpList[0];

            }
            else
            {

                ThisPos.localScale = Vector2.Lerp(LittleSc, Vector2.one, Lerp2);
                ActSprite = SpList[1];

            }

            SpriteRenderer.sprite = ActSprite;

            
            

            CircleBubble.radius = Mathf.Lerp(0.2037986f, 1.54484f, Lerp2);

            //0.2037986f + 1.341041f * Lerp2;

            CircleBubble.offset = Vector2.Lerp(new Vector2(0.01677719f, -0.1629785f), new Vector2(0.09f, 0), Lerp2);
            RigiBubble.mass = Mathf.Lerp(1, 1.5f, Lerp2);


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

            GCamera.transform.position = Vector2.Lerp(GCamera.transform.position, ThisPos.position, Lerp1);

            GCamera.transform.position += new Vector3(0, 0, -10);

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

        Destroy(Sprite);
        Destroy(CircleBubble);
        Destroy(RigiBubble);
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
