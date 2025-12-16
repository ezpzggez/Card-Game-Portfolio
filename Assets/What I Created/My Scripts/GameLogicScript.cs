using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{

    public NetworkThingsScript networkthingsscriptinstance;




    //here is where you need to change the cards  SO

    public List<CardScriptableObjectScript> listofSOinstances;

    public CardScriptableObjectScript mushroom;
    public CardScriptableObjectScript dragon;
    public CardScriptableObjectScript zombie;
    public CardScriptableObjectScript snake;




    public GameObject cardprefab;



    public Transform card1handtransform;
    public Transform card2handtransform;
    public Transform card3handtransform;
    public Transform card4handtransform;
    public Transform card5handtransform;






    public int numberoffieldmonsters = 0;


    public bool mysidefieldmonsterposition1occupied;
    public bool mysidefieldmonsterposition2occupied;
    public bool mysidefieldmonsterposition3occupied;
    public bool mysidefieldmonsterposition4occupied;
    public bool mysidefieldmonsterposition5occupied;
    public bool mysidefieldmonsterposition6occupied;
    public bool mysidefieldmonsterposition7occupied;








    public int numberofcardsindeck;

    public bool handcardposition1occupied = false;
    public bool handcardposition2occupied = false;
    public bool handcardposition3occupied = false;
    public bool handcardposition4occupied = false;
    public bool handcardposition5occupied = false;









    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 2 cards each
        listofSOinstances = new List<CardScriptableObjectScript> { mushroom, mushroom, dragon, dragon, zombie, zombie, snake, snake };

        numberofcardsindeck = listofSOinstances.Count;
        //number of cards in deck is the list length



        GameObject initialcard1 = Instantiate(cardprefab, card1handtransform.position, Quaternion.identity);

        int randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

        initialcard1.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];

        initialcard1.GetComponent<CardScript>().handcardid = 1;

        handcardposition1occupied = true;

        listofSOinstances.RemoveAt(randomindex);

        numberofcardsindeck = listofSOinstances.Count;






        GameObject initialcard2 = Instantiate(cardprefab, card2handtransform.position, Quaternion.identity);

        randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

        initialcard2.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];

        initialcard2.GetComponent<CardScript>().handcardid = 2;


        handcardposition2occupied = true;

        listofSOinstances.RemoveAt(randomindex);

        numberofcardsindeck = listofSOinstances.Count;






        GameObject initialcard3 = Instantiate(cardprefab, card3handtransform.position, Quaternion.identity);

        randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

        initialcard3.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];

        initialcard3.GetComponent<CardScript>().handcardid = 3;

        


        handcardposition3occupied = true;

        listofSOinstances.RemoveAt(randomindex);

        numberofcardsindeck = listofSOinstances.Count;






        GameObject initialcard4 = Instantiate(cardprefab, card4handtransform.position, Quaternion.identity);

        randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

        initialcard4.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];

        initialcard4.GetComponent<CardScript>().handcardid = 4;


        handcardposition4occupied = true;
        
        listofSOinstances.RemoveAt(randomindex);


        numberofcardsindeck = listofSOinstances.Count;






        GameObject initialcard5 = Instantiate(cardprefab, card5handtransform.position, Quaternion.identity);
        
        randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

        initialcard5.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];

        initialcard5.GetComponent<CardScript>().handcardid = 5;


        handcardposition5occupied = true;

        listofSOinstances.RemoveAt(randomindex);


        numberofcardsindeck = listofSOinstances.Count;





        Debug.Log(numberofcardsindeck);



        networkthingsscriptinstance.startmyturndelegate += onturnstartdrawcard;





    }

    // Update is called once per frame
    void Update()
    {

    }



    void onturnstartdrawcard()
    {
        List<bool> handpositionoccupiedlist = new List<bool> { handcardposition1occupied, handcardposition2occupied, handcardposition3occupied, handcardposition4occupied, handcardposition5occupied };
        for (int i = 1; i <= handpositionoccupiedlist.Count; i++)
        {
            if (handpositionoccupiedlist[i - 1] == false)
            {
                Debug.Log("hand position empty, trying to draw a card");

                if (listofSOinstances.Count > 0)
                {

                    switch (i)
                    {
                        case 1:

                            GameObject newcard1 = Instantiate(cardprefab, card1handtransform.position, Quaternion.identity);

                            int randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

                            newcard1.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];
                            //random card generation(needs to change to limited number of same cards)

                            listofSOinstances.RemoveAt(randomindex);

                            newcard1.GetComponent<CardScript>().handcardid = 1;

                            handcardposition1occupied = true;

                            break;

                        case 2:

                            GameObject newcard2 = Instantiate(cardprefab, card2handtransform.position, Quaternion.identity);

                            randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

                            newcard2.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];
                            //random card generation(needs to change to limited number of same cards)

                            listofSOinstances.RemoveAt(randomindex);


                            newcard2.GetComponent<CardScript>().handcardid = 2;

                            handcardposition2occupied = true;

                            break;

                        case 3:

                            GameObject newcard3 = Instantiate(cardprefab, card3handtransform.position, Quaternion.identity);

                            randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

                            newcard3.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];
                            //random card generation(needs to change to limited number of same cards)

                            listofSOinstances.RemoveAt(randomindex);


                            newcard3.GetComponent<CardScript>().handcardid = 3;

                            handcardposition3occupied = true;

                            break;

                        case 4:

                            GameObject newcard4 = Instantiate(cardprefab, card4handtransform.position, Quaternion.identity);

                            randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

                            newcard4.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];
                            //random card generation(needs to change to limited number of same cards)

                            listofSOinstances.RemoveAt(randomindex);


                            newcard4.GetComponent<CardScript>().handcardid = 4;

                            handcardposition4occupied = true;

                            break;

                        case 5:

                            GameObject newcard5 = Instantiate(cardprefab, card5handtransform.position, Quaternion.identity);

                            randomindex = UnityEngine.Random.Range(0, listofSOinstances.Count);

                            newcard5.GetComponent<CardScript>().scriptableobjectinstance = listofSOinstances[randomindex];
                            //random card generation(needs to change to limited number of same cards)

                            listofSOinstances.RemoveAt(randomindex);


                            newcard5.GetComponent<CardScript>().handcardid = 5;

                            handcardposition5occupied = true;

                            break;
                    }
                }

                else
                {
                    Debug.Log("deck is empty!! can't draw a card");
                }


                numberofcardsindeck = listofSOinstances.Count;
                Debug.Log($"after I draw card, the card left in deck is: {numberofcardsindeck}");


                break; //stop iteration statement at the first empty hand, so it doesn't iterate.

            }
            //if every hand position is taken, you just don't draw a card.
        }
    }




        





}
