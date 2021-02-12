using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    int Health;
    public int ID;
    public GameManager gameManager;

    Vector3 newPos;
    bool move = false;
    public bool isDebug = false;

    public float step;

    Image img;
    //Animator animator;
    Animation anim;


    Text texthealth;

    
    // Start is called before the first frame update
    void Start()
    {
        texthealth = GetComponentInChildren<Text>();
        texthealth.text = Health.ToString();
        //animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
        changeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position = Vector2.Lerp(transform.position, newPos, .25f);
            if (transform.position == newPos) move = false;
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ammo")
        {

            TakeDamage();
        }
    }

    public void setParram(int healthcount, Vector3 posittion, int id, GameManager game,float sstep)
    {
        Health = healthcount;
        transform.position = posittion;
        ID = id;
        gameManager = game;
        img = gameObject.GetComponent<Image>();
        step = sstep;
        

    }
    public void setParram(int healthcount,int id, GameManager game)
    {
        Health = healthcount;
        ID = id;
        //gameManager = game;
        img = gameObject.GetComponent<Image>();

        changeColor();

    }
    public void movePosition()
    {
        Vector3 screenposition = Camera.main.WorldToScreenPoint(transform.position);
        screenposition.y -= step;
        //transform.position = Vector2.Lerp(transform.position,Camera.main.ScreenToWorldPoint(screenposition),1);
        newPos = Camera.main.ScreenToWorldPoint(screenposition);
        move = true;
        if (isDebug)
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(screenposition));
            Debug.Log(screenposition);
        }
        //if (screenposition.y <= 150) gameManager.GameOver();
    }

    void changeColor()
    {
        img.color = gameManager.colors[Health];
        //Debug.Log(img.color);  
    }
    public void TakeDamage()
    {
     
        Health--;
        Debug.Log("TakeDamage");
        if (Health <= 0)
        {
            Debug.Log("Health = 0");
            gameManager.DestroyedBlock(gameObject);
        }
        else
        {

        texthealth.text = Health.ToString();
        changeColor();
        //animator.Play("blockHit");
        anim.Play("blockHit");


        }

    }

    public void SetSize(float size)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size,size,0);
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        box.size = new Vector2(size, size);
    }
  
        
        
        
        
}
