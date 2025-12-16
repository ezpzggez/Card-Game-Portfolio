using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{

    private NetworkThingsScript networkthingsscriptinstance;
    public CardScriptableObjectScript scriptableobjectinstance;


    public CardScriptableObjectScript mushroom;
    public CardScriptableObjectScript dragon;
    public CardScriptableObjectScript zombie;
    public CardScriptableObjectScript snake;


    public List<CardScriptableObjectScript> SOlist;


    public GameObject fieldmonsterprefab;

    private Transform fieldplacement1;
    private Transform fieldplacement2;
    private Transform fieldplacement3;
    private Transform fieldplacement4;
    private Transform fieldplacement5;
    private Transform fieldplacement6;
    private Transform fieldplacement7;







    public SpriteRenderer cardspriterenderer;
    public TMP_Text cardnametext;
    public TMP_Text cardtraittext;
    public TMP_Text carddescriptiontext;
    public TMP_Text manacost;
    public TMP_Text attackvalue;
    public TMP_Text healthvalue;






    private Camera maincam;

    private Vector3 clickpos;


    private Vector3 originalpos;


    private GameLogicScript gamelogicscriptinstance;





    public int cardmanacost;

    public int carddmg;

    public int cardhealth;



    public int handcardid;











    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        gamelogicscriptinstance = FindAnyObjectByType<GameLogicScript>();

        networkthingsscriptinstance = FindAnyObjectByType<NetworkThingsScript>();

        maincam = Camera.main;

        originalpos = transform.position;




        cardspriterenderer.sprite = scriptableobjectinstance.cardsprite;

        cardnametext.text = scriptableobjectinstance.cardname;

        cardtraittext.text = scriptableobjectinstance.cardtrait;

        carddescriptiontext.text = scriptableobjectinstance.carddescription;

        manacost.text = scriptableobjectinstance.mana;

        attackvalue.text = scriptableobjectinstance.attack;

        healthvalue.text = scriptableobjectinstance.health;



        fieldplacement1 = GameObject.Find("Placement 1").transform;
        fieldplacement2 = GameObject.Find("Placement 2").transform;
        fieldplacement3 = GameObject.Find("Placement 3").transform;
        fieldplacement4 = GameObject.Find("Placement 4").transform;
        fieldplacement5 = GameObject.Find("Placement 5").transform;
        fieldplacement6 = GameObject.Find("Placement 6").transform;
        fieldplacement7 = GameObject.Find("Placement 7").transform;



        SOlist = new List<CardScriptableObjectScript> { mushroom, dragon, zombie, snake };


        cardmanacost = int.Parse(scriptableobjectinstance.mana);

        carddmg = int.Parse(scriptableobjectinstance.attack);

        cardhealth = int.Parse(scriptableobjectinstance.health);


    }

    // Update is called once per frame





    void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        transform.localPosition += new Vector3(0f, 3f, 0);

        GetComponent<SpriteRenderer>().sortingOrder = 200;
        cardspriterenderer.sortingOrder = 190;
        cardnametext.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        cardtraittext.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        carddescriptiontext.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        cardtraittext.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        manacost.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        attackvalue.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
        healthvalue.GetComponent<TextMeshPro>().renderer.sortingOrder = 210;
    }

    void OnMouseExit()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        transform.localPosition -= new Vector3(0f, 3f, 0);

        GetComponent<SpriteRenderer>().sortingOrder = 100;
        cardspriterenderer.sortingOrder = 90;
        cardnametext.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        cardtraittext.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        carddescriptiontext.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        cardtraittext.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        manacost.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        attackvalue.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
        healthvalue.GetComponent<TextMeshPro>().renderer.sortingOrder = 110;
    }

    void OnMouseDown()
    {
        clickpos = maincam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void OnMouseDrag()
    {
        transform.position = maincam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void OnMouseUp()
    {
        Vector3 releasepos = maincam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        if ((releasepos.y - clickpos.y > 3f) && (networkthingsscriptinstance.ismyturn == true) && (cardmanacost <= networkthingsscriptinstance.currentmana))
        {


            if (gamelogicscriptinstance.mysidefieldmonsterposition1occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition2occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition3occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition4occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition5occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition6occupied == true
            && gamelogicscriptinstance.mysidefieldmonsterposition7occupied == true
            )
            {
                Debug.Log("no more field slot left!!");
                transform.position = new Vector3(originalpos.x, originalpos.y + 3f, originalpos.z);
            }


            else
            {

                switch (handcardid)
                {
                    case 1:
                        gamelogicscriptinstance.handcardposition1occupied = false;
                        break;
                    case 2:
                        gamelogicscriptinstance.handcardposition2occupied = false;
                        break;
                    case 3:
                        gamelogicscriptinstance.handcardposition3occupied = false;
                        break;
                    case 4:
                        gamelogicscriptinstance.handcardposition4occupied = false;
                        break;
                    case 5:
                        gamelogicscriptinstance.handcardposition5occupied = false;
                        break;
                    default:
                        Debug.Log("this card played does not have cardid 1 through 5");
                        break;
                }





                if (gamelogicscriptinstance.mysidefieldmonsterposition1occupied == false)
                //INSTANTIATE AT POSITION 1, ID 1 !!!!! (IF POSITION 1 IS EMPTY)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement1.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 1;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(1, SOlist.IndexOf(scriptableobjectinstance)); //position 1, SOinstance index of whatever it is

                    gamelogicscriptinstance.mysidefieldmonsterposition1occupied = true;



                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition2occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement2.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 2;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(2, SOlist.IndexOf(scriptableobjectinstance)); //position 2, SOinstance index of whatever it is


                    gamelogicscriptinstance.mysidefieldmonsterposition2occupied = true;
                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition3occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement3.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 3;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(3, SOlist.IndexOf(scriptableobjectinstance));


                    gamelogicscriptinstance.mysidefieldmonsterposition3occupied = true;
                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition4occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement4.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 4;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(4, SOlist.IndexOf(scriptableobjectinstance));



                    gamelogicscriptinstance.mysidefieldmonsterposition4occupied = true;
                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition5occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement5.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 5;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(5, SOlist.IndexOf(scriptableobjectinstance));



                    gamelogicscriptinstance.mysidefieldmonsterposition5occupied = true;
                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition6occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement6.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 6;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(6, SOlist.IndexOf(scriptableobjectinstance));



                    gamelogicscriptinstance.mysidefieldmonsterposition6occupied = true;
                }

                else if (gamelogicscriptinstance.mysidefieldmonsterposition7occupied == false)
                {
                    //local logic
                    GameObject mymonster = Instantiate(fieldmonsterprefab, fieldplacement7.position, Quaternion.identity);
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = scriptableobjectinstance;
                    mymonster.tag = "MyFieldMonsterTag";
                    mymonster.GetComponent<FieldMonsterScript>().fieldmonsterid = 7;
                    gamelogicscriptinstance.numberoffieldmonsters++;

                    commonwheniplaycardfunction();

                    Destroy(gameObject);


                    networkthingsscriptinstance.cardplayedServerRpc(7, SOlist.IndexOf(scriptableobjectinstance));



                    gamelogicscriptinstance.mysidefieldmonsterposition7occupied = true;
                }
            }

            




        }


        else
        {
            transform.position = new Vector3(originalpos.x, originalpos.y + 3f, originalpos.z);
        }
            
    }








    public void commonwheniplaycardfunction()
    {
        networkthingsscriptinstance.currentmana -= cardmanacost;
        networkthingsscriptinstance.manatextgameobject.GetComponent<TMP_Text>().text = $"{networkthingsscriptinstance.currentmana.ToString()} / {networkthingsscriptinstance.maxmana}";

        foreach (GameObject manacrystal in networkthingsscriptinstance.manacrystalslist)
        {
            manacrystal.SetActive(false);
        }

        for (int i = 0; i < networkthingsscriptinstance.currentmana; i++)
        {
            networkthingsscriptinstance.manacrystalslist[i].SetActive(true);
        }
    }


    



}
