using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAE_Behaviour : MonoBehaviour
{


    private SpriteRenderer Sprite;
    public float ImputForce = 800;
    private PolygonCollider2D PolyMAE;
    private Rigidbody2D RigiMAE;

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

        PolyMAE = GetComponent<PolygonCollider2D>();
        RigiMAE = GetComponent<Rigidbody2D>();
        

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

        released = true;
        RigiMAE.velocity = new Vector2(10, 10);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Sprite)
        {
       
            if (RigiMAE.velocity.magnitude < 2.0f && released)
            {

                StartCoroutine(DestroyAtTime(3));

            }
            else
            {

                StopCoroutine(DestroyAtTime(3));

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



    }

    private void OnCollisionEnter2D(Collision2D coll)
    {

        RigiMAE.gravityScale = 2.71818f;

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
        Destroy(PolyMAE);
        Destroy(RigiMAE);
        if (!IsDead)
        {

            var Puff = Instantiate(BirdDeadAnim, transform.position, Quaternion.identity);
            IsDead = true;
            GCamera.transform.position = new Vector3(0, 0, -10);
            Destroy(gameObject, 0.5f);
            Destroy(Puff, 0.5f);

        }

    }


}
