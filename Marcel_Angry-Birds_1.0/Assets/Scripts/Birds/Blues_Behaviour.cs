using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blues_Behaviour : MonoBehaviour
{

    private SpriteRenderer Sprite;
    public float ImputForce = 800;
    private CircleCollider2D CircleBlues;
    private Rigidbody2D RigiBlues;

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
    private bool Triplicated;
    private bool Collided;

    private Rigidbody2D RBCl1;
    private Rigidbody2D RBCl2;
    public bool MainBird = true;
    private readonly float Desviation = 1.3101f;


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

        CircleBlues = GetComponent<CircleCollider2D>();
        RigiBlues = GetComponent<Rigidbody2D>();
        if (MainBird)
        {
            RigiBlues.isKinematic = true;
        }
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

    }

    // Update is called once per frame
    void Update()
    {
        MaxPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - slingshot.transform.position;

        if (Sprite)
        {
            if (Input.GetMouseButton(0) && !released && MaxPos.sqrMagnitude < 2.5f)
            {

                RigiBlues.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }

            if (Input.GetMouseButtonUp(0) && !released)
            {
                RedPos = Camera.main.ScreenToWorldPoint(this.transform.position); 

                released = true;
                RigiBlues.isKinematic = false;
                RigiBlues.AddForce((SlingPos - RedPos) * ImputForce, ForceMode2D.Impulse);


            }

            if (RigiBlues.velocity.magnitude < 2.0f && released)
            {

                StartCoroutine(DestroyAtTime(3));

            }
            else
            {

                StopCoroutine(DestroyAtTime(3));

            }

            if (released && !Triplicated && !Collided && Input.GetMouseButtonDown(0))
            {

                Triplicated = true;

                
                CloneDown();
                CloneUp();

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
        if (released && !IsDead && MainBird)
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

    void CloneUp()
    {

        var Bl1 = Instantiate(this, transform.position + new Vector3(0, 0.29f, 0), Quaternion.identity);

        Bl1.MainBird = false;
        Bl1.released = true;
        Bl1.Triplicated = true;

        RBCl1 = Bl1.GetComponent<Rigidbody2D>();
        RBCl1.isKinematic = false;


        RBCl1.velocity = RigiBlues.velocity;

        RBCl1.velocity = RigiBlues.velocity + new Vector2(0, Desviation);

    }

    void CloneDown()
    {


        var Bl2 = Instantiate(this, transform.position + new Vector3(0, -0.29f, 0), Quaternion.identity);

        Bl2.MainBird = false;
        Bl2.released = true;
        Bl2.Triplicated = true;

        RBCl2 = Bl2.GetComponent<Rigidbody2D>();
        RBCl2.isKinematic = false;


        //RBCl2.velocity = RigiBlues.velocity;

        RBCl2.velocity = RigiBlues.velocity + new Vector2(0, -Desviation);

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {

        if (released)
        {

            Collided = true;

        }

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
        Destroy(CircleBlues);
        Destroy(RigiBlues);
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
