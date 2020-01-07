using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matilda_Behaviour : MonoBehaviour
{


    private SpriteRenderer Sprite;
    public float ImputForce = 800;
    private PolygonCollider2D PolyMatilda;
    private Rigidbody2D RigiMatilda;

    private GameObject slingshot;
    Vector2 SlingPos;
    Vector2 RedPos;
    Vector2 MaxPos;
    bool released = false;

    public GameObject egg;
    public GameObject MAE;

    private bool IsDead = false;
    public GameObject BirdDeadAnim;

    public bool Egg = false;

    bool Collided = false;


    private Transform ThisPos;
    public Transform GCamera;
    private float Lerp1 = 0f;

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

        PolyMatilda = GetComponent<PolygonCollider2D>();
        RigiMatilda = GetComponent<Rigidbody2D>();
        RigiMatilda.isKinematic = true;

        slingshot = GameObject.Find("slingshot_1");
        SlingPos = Camera.main.ScreenToWorldPoint(slingshot.transform.position);
        GCamera = GameObject.Find("Main Camera").GetComponent<Transform>();

        Sprite = GetComponent<SpriteRenderer>();

        ThisPos = transform;

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
        MaxPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - slingshot.transform.position;

        if (Sprite)
        {
            if (Input.GetMouseButton(0) && !released && MaxPos.sqrMagnitude < 2.5f)
            {

                RigiMatilda.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }

            if (Input.GetMouseButtonUp(0) && !released)
            {
                RedPos = Camera.main.ScreenToWorldPoint(this.transform.position);

                released = true;
                RigiMatilda.isKinematic = false;
                RigiMatilda.AddForce((SlingPos - RedPos) * ImputForce, ForceMode2D.Impulse);


            }

            if (RigiMatilda.velocity.magnitude < 1.0f && released)
            {

                StartCoroutine(DestroyAtTime(3));

            }
            else
            {

                StopCoroutine(DestroyAtTime(3));

            }

            if (released && !Egg && !Collided && Input.GetMouseButtonDown(0))
            {

                Egg = true;

                EggFun();

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

            GCamera.transform.position = Vector2.Lerp(GCamera.transform.position, ThisPos.position, Lerp1);


            GCamera.transform.position += new Vector3(0, 0, -10);

        }


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
        //Destroy(this.gameObject);
        StopCoroutine(DestroyAtTime(3));

    }

    void EggFun()
    {

        var EGG = Instantiate(egg, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
        var MatAE = Instantiate(MAE, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }

    void Dead()
    {

        Destroy(Sprite);
        Destroy(PolyMatilda);
        Destroy(RigiMatilda);
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
