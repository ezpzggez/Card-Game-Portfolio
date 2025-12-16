using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class FieldMonsterScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public CardScriptableObjectScript fieldmonsterscriptableobjectinstance;



    public SpriteRenderer cardspriterenderer;
    public TMP_Text attackvalue;
    public TMP_Text healthvalue;


    private LineRenderer linerenderercomponent;

    private GameObject linearrowheadgameobject;

    private GameObject hovercircle;

    public NetworkThingsScript networkthingsscriptinstance;


    public int myhealth;

    public int mydmg;

    public int fieldmonsterid;
    //identify field monsters by tag(enemy vs mine) and id (which one)


    private bool ishovering;


    private bool didIattackthisturn = false;

    public GameLogicScript gamelogicscriptinstance;

















    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        cardspriterenderer.sprite = fieldmonsterscriptableobjectinstance.cardsprite;

        attackvalue.text = fieldmonsterscriptableobjectinstance.attack;

        healthvalue.text = fieldmonsterscriptableobjectinstance.health;



        myhealth = int.Parse(fieldmonsterscriptableobjectinstance.health);

        mydmg = int.Parse(fieldmonsterscriptableobjectinstance.attack);






        networkthingsscriptinstance = FindAnyObjectByType<NetworkThingsScript>();

        gamelogicscriptinstance = FindAnyObjectByType<GameLogicScript>();

        linerenderercomponent = FindAnyObjectByType<LineRenderer>(FindObjectsInactive.Include);

        linearrowheadgameobject = FindAnyObjectByType<islinearrowhead>(FindObjectsInactive.Include).gameObject;

        hovercircle = FindAnyObjectByType<ishovercircle>(FindObjectsInactive.Include).gameObject;


        networkthingsscriptinstance.startmyturndelegate += onturnstart;



    }

    // Update is called once per frame
    void Update()
    {
        if (ishovering == true)
        {
            hovercircle.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            hovercircle.transform.position += new Vector3(0f, 0.3f, 0f);
        }
    }


    public void OnPointerDown(PointerEventData asdf) //so things like this work on every single fieldmonster, so you need if (ismyturn) and things like that for every logic
    {

        if (networkthingsscriptinstance.ismyturn == true && tag == "MyFieldMonsterTag")
        {

            linerenderercomponent.gameObject.SetActive(true);

            linerenderercomponent.positionCount = 2;

            linerenderercomponent.SetPosition(0, transform.position);

            linerenderercomponent.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(asdf.position.x, asdf.position.y, 10)));







            linearrowheadgameobject.SetActive(true);

            linearrowheadgameobject.transform.position = linerenderercomponent.GetPosition(1);
        }





    }

    public void OnDrag(PointerEventData asdf)
    {

        if (networkthingsscriptinstance.ismyturn == true && tag == "MyFieldMonsterTag")
        {

            linerenderercomponent.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(asdf.position.x, asdf.position.y, 10)));


            linearrowheadgameobject.transform.position = linerenderercomponent.GetPosition(1);



            //rotation logic 

            Vector2 direction = (linearrowheadgameobject.transform.position - transform.position).normalized;

            Quaternion torotation = Quaternion.LookRotation(Vector3.forward, direction);
            //lookrotation just takes two parameters(forwad and upward) and change the transform.forward and transform.upward to that direction.
            //so in this case, I left the forward as it is, (default 0,0,1) and upward direction to the direction of where the arrow is facing.

            linearrowheadgameobject.transform.rotation = torotation;

            networkthingsscriptinstance.isdragging = true;

        }


    }


    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (tag == "EnemyFieldMonsterTag" && networkthingsscriptinstance.isdragging == true)  //so the tag of the gameobject (fieldmonster) that this script is attached to
        //isdragging == true only when the dragging started from an ally fieldmonster
        {
            hovercircle.SetActive(true);
            ishovering = true;
        }
    }


    public void OnPointerExit(PointerEventData eventdata)
    {
        hovercircle.SetActive(false);
        ishovering = false;
    }


    public void OnEndDrag(PointerEventData eventdata)
    {

        linerenderercomponent.gameObject.SetActive(false);

        linearrowheadgameobject.SetActive(false);




        if (networkthingsscriptinstance.ismyturn == true && tag == "MyFieldMonsterTag" && didIattackthisturn == false)
        {


            RaycastHit2D hitinfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(eventdata.position.x, eventdata.position.y, 10)), Vector2.zero);

            if (hitinfo.collider != null && hitinfo.collider.gameObject.GetComponent<isfieldmonster>() != null && hitinfo.collider.gameObject.CompareTag("EnemyFieldMonsterTag"))
            {
                Debug.Log("is ending drag on enemy field monster");


                FieldMonsterScript enemyfieldmonsterscript = hitinfo.collider.GetComponent<FieldMonsterScript>();



                // Debug.Log("enemy health: " + enemyfieldmonsterscript.myhealth);
                // Debug.Log("enemy attack: " + enemyfieldmonsterscript.mydmg);

                // Debug.Log("my health: " + myhealth);
                // Debug.Log("my attack: " + mydmg);



                int resultingmyhealth = myhealth - enemyfieldmonsterscript.mydmg;

                int resultingenemyhealth = enemyfieldmonsterscript.myhealth - mydmg;


                StartCoroutine(animateattack(gameObject.transform, enemyfieldmonsterscript.transform, 0.1f));



                //change my health
                myhealth = resultingmyhealth; //update value

                healthvalue.text = myhealth.ToString(); //update text shown in game


                //change enemy health(on my screen)

                enemyfieldmonsterscript.myhealth = resultingenemyhealth; //update value
                enemyfieldmonsterscript.healthvalue.text = enemyfieldmonsterscript.myhealth.ToString(); //update text shown in game



                //give fight info to other game instance
                networkthingsscriptinstance.aftercardfightServerRpc(fieldmonsterid, enemyfieldmonsterscript.fieldmonsterid);


                didIattackthisturn = true;


                // if (enemyfieldmonsterscript.myhealth <= 0)
                // {
                //     Destroy(enemyfieldmonsterscript.gameObject);
                // }


                // if (myhealth <= 0)
                // {
                //     Destroy(gameObject);
                // }



            }


            else if (hitinfo.collider != null && hitinfo.collider.gameObject.GetComponent<ishero>() != null && hitinfo.collider.gameObject.CompareTag("EnemyHeroTag"))
            {
                Debug.Log("is ending drag on enemy hero");

                GameObject enemyhero = hitinfo.collider.gameObject;




                //the hero healths are stored and updated in nedtworkthingsscript
                networkthingsscriptinstance.enemyherohealth -= mydmg;


                //animation
                StartCoroutine(animateattack(gameObject.transform, enemyhero.transform, 0.1f));



                //update enemy hero health on my screen


                enemyhero.GetComponentInChildren<TMP_Text>().text = networkthingsscriptinstance.enemyherohealth.ToString();




                //give fight info to other game instance
                networkthingsscriptinstance.afterheroattackServerRpc(fieldmonsterid);








                didIattackthisturn = true;


                //gameover
                if (networkthingsscriptinstance.enemyherohealth <= 0)
                {


                    networkthingsscriptinstance.herohealth0ServerRpc();


                    StartCoroutine(resetgamecoroutine());


                    
                }




            }






        }

        //end dragging no matter if I attacked or not, at the end of drag
        networkthingsscriptinstance.isdragging = false;




    }


    private IEnumerator resetgamecoroutine()
    {
        //wait for the youwonui coroutine to finish
        yield return StartCoroutine(networkthingsscriptinstance.youwonuicoroutine());

        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public IEnumerator animateattack(Transform attackertransform, Transform defendertransform, float moveonewayseconds)
    {
        Vector3 startpos = attackertransform.position;
        Vector3 endpos = defendertransform.position - new Vector3(0, 1, 0);


        //gonna use interpolation (lerp) for which 0 gonna be 0% and 0.5 gonna be 50% and 1 gonna be 100% posiiton between the points
        for (float t = 0; t < 1f; t += Time.deltaTime / moveonewayseconds)
        {
            attackertransform.position = Vector3.Lerp(startpos, endpos, t);
            yield return null;
        }
        attackertransform.position = endpos;





        for (float t = 0; t < 1f; t += Time.deltaTime / moveonewayseconds)
        {
            attackertransform.position = Vector3.Lerp(endpos, startpos, t);
            yield return null;
        }
        attackertransform.position = startpos;




        if (defendertransform.GetComponent<FieldMonsterScript>()?.myhealth <= 0)
        {
            Destroy(defendertransform.gameObject);
        }


        if (attackertransform.GetComponent<FieldMonsterScript>()?.myhealth <= 0)
        {
            Destroy(attackertransform.gameObject);
        }



    }


    public void onturnstart()
    {
        Debug.Log($"fieldmonster {fieldmonsterscriptableobjectinstance.cardname} recognized that the owner's turn started");

        didIattackthisturn = false;
    }



    void OnDisable()
    {
        switch (fieldmonsterid)
        {
            case 1:
                gamelogicscriptinstance.mysidefieldmonsterposition1occupied = false;
                break;
            case 2:
                gamelogicscriptinstance.mysidefieldmonsterposition2occupied = false;
                break;
            case 3:
                gamelogicscriptinstance.mysidefieldmonsterposition3occupied = false;
                break;
            case 4:
                gamelogicscriptinstance.mysidefieldmonsterposition4occupied = false;
                break;
            case 5:
                gamelogicscriptinstance.mysidefieldmonsterposition5occupied = false;
                break;
            case 6:
                gamelogicscriptinstance.mysidefieldmonsterposition6occupied = false;
                break;
            case 7:
                gamelogicscriptinstance.mysidefieldmonsterposition7occupied = false;
                break;
        }

        if (networkthingsscriptinstance != null)
        {
            networkthingsscriptinstance.startmyturndelegate -= onturnstart;
        }
    }




    







}
