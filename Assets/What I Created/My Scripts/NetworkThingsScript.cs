using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;





//since this is connected to a networkthingshandler gameobject, and that gameobject has a networkobject component, it will spawn automatically
//when you connect to the network. 
public class NetworkThingsScript : NetworkBehaviour

{

    public List<CardScriptableObjectScript> SOlist;

    public CardScriptableObjectScript mushroom;
    public CardScriptableObjectScript dragon;
    public CardScriptableObjectScript zombie;
    public CardScriptableObjectScript snake;



    public GameObject fieldmonsterprefab;

    public GameLogicScript gamelogicscriptinstance;

    public Transform opponentfieldtransform1;

    public Transform opponentfieldtransform2;

    public Transform opponentfieldtransform3;

    public Transform opponentfieldtransform4;

    public Transform opponentfieldtransform5;

    public Transform opponentfieldtransform6;

    public Transform opponentfieldtransform7;



    public GameObject gamestartimageui;


    public bool ismyturn;

    public GameObject myturnui;
    public GameObject enemyturnui;


    public GameObject endturnbutton;

    public GameObject enemyturnbutton;



    public GameObject manacrystal1;
    public GameObject manacrystal2;
    public GameObject manacrystal3;
    public GameObject manacrystal4;
    public GameObject manacrystal5;
    public GameObject manacrystal6;
    public GameObject manacrystal7;
    public GameObject manacrystal8;
    public GameObject manacrystal9;
    public GameObject manacrystal10;


    public GameObject manatextgameobject;

    public List<GameObject> manacrystalslist;



    public int currentmana = 0;

    public int maxmana = 0;


    public bool isdragging;


    public Action startmyturndelegate;

    public GameObject cardholderhero;
    public GameObject farmerhero;

    public GameObject myheroposition;

    public GameObject enemyheroposition;


    public int enemyherohealth = 30;
    public int myherohealth = 30;


    public GameObject myhero;

    public GameObject enemyhero;



    public GameObject manacrystalbackground;

    public GameObject manacrystaltextbackground;


    public GameObject youlostui;

    public GameObject youwonui;










    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SOlist = new List<CardScriptableObjectScript> { mushroom, dragon, zombie, snake };
        NetworkManager.Singleton.OnClientConnectedCallback += client1connectedfunction;

        manacrystalslist = new List<GameObject>
        {manacrystal1, manacrystal2, manacrystal3, manacrystal4, manacrystal5, manacrystal6, manacrystal7, manacrystal8, manacrystal9, manacrystal10};
    }

    // Update is called once per frame
    void Update()
    {

    }







    [ServerRpc(RequireOwnership = false)]
    public void cardplayedServerRpc(int fieldmonsterplacementnumberserverrpc, int SOindex, ServerRpcParams rpcparamsserver = default)
    {
        ulong senderid = rpcparamsserver.Receive.SenderClientId;

        if (senderid == 0)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 } //send to client id 1 if yourself is id 0
                }
            };

            cardplayedClientRpc(fieldmonsterplacementnumberserverrpc, SOindex, customclientrpcparams);
        }

        else if (senderid == 1)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 0 } //send to client id 0 if yourself is id 1
                }
            };

            cardplayedClientRpc(fieldmonsterplacementnumberserverrpc, SOindex, customclientrpcparams);
            //sending the client rpc to this specific clientrpc params means send it to this client only 
            //unity automatically knows who to send client rpc to, by reading the parameter clientrpcparams. (which is a struct)
        }
    }


    [ClientRpc]
    public void cardplayedClientRpc(int fieldmonsterplacementnumberclientrpc, int SOindex, ClientRpcParams rpcparamsclient = default)
    //1. 상대 카드 어디에 놓을건지 2. 무슨 카드인지 3. 어떤 client 한테 전달하는지(default = every client)
    {
        switch (fieldmonsterplacementnumberclientrpc)
        {
            case 1:
                GameObject opponentmonster1 = Instantiate(fieldmonsterprefab, opponentfieldtransform1.position, quaternion.identity);
                opponentmonster1.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster1.tag = "EnemyFieldMonsterTag";
                opponentmonster1.GetComponent<FieldMonsterScript>().fieldmonsterid = 1;
                break;
            case 2:
                GameObject opponentmonster2 = Instantiate(fieldmonsterprefab, opponentfieldtransform2.position, quaternion.identity);
                opponentmonster2.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster2.tag = "EnemyFieldMonsterTag";
                opponentmonster2.GetComponent<FieldMonsterScript>().fieldmonsterid = 2;
                break;
            case 3:
                GameObject opponentmonster3 = Instantiate(fieldmonsterprefab, opponentfieldtransform3.position, quaternion.identity);
                opponentmonster3.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster3.tag = "EnemyFieldMonsterTag";
                opponentmonster3.GetComponent<FieldMonsterScript>().fieldmonsterid = 3;
                break;
            case 4:
                GameObject opponentmonster4 = Instantiate(fieldmonsterprefab, opponentfieldtransform4.position, quaternion.identity);
                opponentmonster4.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster4.tag = "EnemyFieldMonsterTag";
                opponentmonster4.GetComponent<FieldMonsterScript>().fieldmonsterid = 4;
                break;
            case 5:
                GameObject opponentmonster5 = Instantiate(fieldmonsterprefab, opponentfieldtransform5.position, quaternion.identity);
                opponentmonster5.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster5.tag = "EnemyFieldMonsterTag";
                opponentmonster5.GetComponent<FieldMonsterScript>().fieldmonsterid = 5;
                break;
            case 6:
                GameObject opponentmonster6 = Instantiate(fieldmonsterprefab, opponentfieldtransform6.position, quaternion.identity);
                opponentmonster6.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster6.tag = "EnemyFieldMonsterTag";
                opponentmonster6.GetComponent<FieldMonsterScript>().fieldmonsterid = 6;
                break;
            case 7:
                GameObject opponentmonster7 = Instantiate(fieldmonsterprefab, opponentfieldtransform7.position, quaternion.identity);
                opponentmonster7.GetComponent<FieldMonsterScript>().fieldmonsterscriptableobjectinstance = SOlist[SOindex];
                opponentmonster7.tag = "EnemyFieldMonsterTag";
                opponentmonster7.GetComponent<FieldMonsterScript>().fieldmonsterid = 7;
                break;
            default:
                Debug.Log("enemy field is full!");
                break;
        }
    }




    public void client1connectedfunction(ulong clientid) //runs when client 1 connects to the game
    {
        if (clientid == 1) //once client 1 connects to the network
        {

            endturnbutton.GetComponent<Button>().onClick.AddListener(whenendturnbuttonisclicked); //subscribe to endturnbutton regardless of your clientid

            if (NetworkManager.Singleton.LocalClientId == 0)
            {
                StartCoroutine(startofgamecoroutinehost());
                ismyturn = true;
                Debug.Log("is it my turn?   " + ismyturn);

                endturnbutton.SetActive(true); //원래 두 버튼 모두 inactive 로 시작


                manacrystalbackground.SetActive(true); //원래 ui canvas 에서 inactive 로 시작
                manacrystaltextbackground.SetActive(true); //same






                manacrystal1.SetActive(true);
                manatextgameobject.GetComponent<TMP_Text>().text = "1/10";

                currentmana = 1;
                maxmana = 1;

                myhero = Instantiate(cardholderhero, myheroposition.transform.position, quaternion.identity);

                myhero.tag = "MyHeroTag";

                enemyhero = Instantiate(farmerhero, enemyheroposition.transform.position, quaternion.identity);

                enemyhero.tag = "EnemyHeroTag";




            }

            else if (NetworkManager.Singleton.LocalClientId == 1)
            {
                StartCoroutine(startofgamecoroutineclient());
                ismyturn = false;
                Debug.Log("is it my turn?   " + ismyturn);


                enemyturnbutton.SetActive(true);


                manacrystalbackground.SetActive(true); //원래 ui canvas 에서 inactive 로 시작
                manacrystaltextbackground.SetActive(true); //same


                myhero = Instantiate(farmerhero, myheroposition.transform.position, quaternion.identity);

                myhero.tag = "MyHeroTag";

                enemyhero = Instantiate(cardholderhero, enemyheroposition.transform.position, quaternion.identity);

                enemyhero.tag = "EnemyHeroTag";



            }

        }
    }



    IEnumerator handlegamestartimage()
    {
        gamestartimageui.SetActive(true);
        yield return new WaitForSeconds(1f);
        gamestartimageui.SetActive(false);
    }


    IEnumerator handlemyturnimage()
    {
        myturnui.SetActive(true);
        yield return new WaitForSeconds(1f);
        myturnui.SetActive(false);
    }

    IEnumerator handleenemyturnimage()
    {
        enemyturnui.SetActive(true);
        yield return new WaitForSeconds(1f);
        enemyturnui.SetActive(false);
    }


    IEnumerator startofgamecoroutinehost() // combines the upper two together in a iterator function, so that they run one by one instead of simultaneously. 
    {
        yield return StartCoroutine(handlegamestartimage());
        yield return StartCoroutine(handlemyturnimage());
    }


    IEnumerator startofgamecoroutineclient()
    {
        yield return StartCoroutine(handlegamestartimage());
        yield return StartCoroutine(handleenemyturnimage());
    }




    public void whenendturnbuttonisclicked()
    {
        endmyturnstartyourturnServerRpc();

        ismyturn = false;
        endturnbutton.SetActive(false);
        enemyturnbutton.SetActive(true);


        StartCoroutine(handleenemyturnimage());







        Debug.Log("is it my turn?  " + ismyturn);
    }





    [ServerRpc(RequireOwnership = false)]
    public void endmyturnstartyourturnServerRpc(ServerRpcParams rpcparamsserver = default)
    {
        ulong senderclientid = rpcparamsserver.Receive.SenderClientId;

        if (senderclientid == 0)
        {
            ClientRpcParams targetclientrpcparams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new ulong[] { 1 }
                }
            };


            endmyturnstartyourturnClientRpc(targetclientrpcparams);
        }

        if (senderclientid == 1)
        {
            ClientRpcParams targetclientrpcparams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new ulong[] { 0 }
                }
            };


            endmyturnstartyourturnClientRpc(targetclientrpcparams);
        }
    }


    [ClientRpc]
    public void endmyturnstartyourturnClientRpc(ClientRpcParams rpcparamsclient = default)
    {
        ismyturn = true;
        endturnbutton.SetActive(true);
        enemyturnbutton.SetActive(false);
        //when you press end my turn button, received client starts its turn and activates end turn button, deactivates enemy turn button(locally)
        //locally deactivate so that the game client will not be able to interact with the inappropriate button.

        StartCoroutine(handlemyturnimage());





        //set up mana related values AT THE START OF THE TURN (BELOW CODE)
        if (maxmana < 10)
        {
            maxmana++;
        }


        //activate mana crystal images
        for (int i = 0; i < maxmana; i++)
        {
            manacrystalslist[i].SetActive(true);
        }

        //deactivate mana crystal that's beyond current max mana
        for (int i = maxmana; i < 10; i++)
        {
            manacrystalslist[i].SetActive(false);
        }




        //count the mana crystals and set them up in text
        // int maxmanacrystalcounter = 0;

        // foreach (GameObject manacrystal in manacrystalslist)
        // {
        //     if (manacrystal.activeSelf == true)
        //     {
        //         maxmanacrystalcounter ++;
        //     }
        // }

        manatextgameobject.GetComponent<TMP_Text>().text = $"{maxmana} / {maxmana}";


        currentmana = maxmana;



        startmyturndelegate?.Invoke();


        Debug.Log("is it my turn?  " + ismyturn);
    }














    [ServerRpc(RequireOwnership = false)]
    public void aftercardfightServerRpc(int myfieldmonsterid, int enemyfieldmonsterid, ServerRpcParams rpcparamsserver = default)
    {
        ulong senderid = rpcparamsserver.Receive.SenderClientId;

        if (senderid == 0)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 } //send to client id 1 if yourself is id 0
                }
            };


            aftercardfightClientRpc(myfieldmonsterid, enemyfieldmonsterid, customclientrpcparams);
        }

        else if (senderid == 1)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 0 } //send to client id 0 if yourself is id 1
                }
            };


            aftercardfightClientRpc(myfieldmonsterid, enemyfieldmonsterid, customclientrpcparams);
        }
    }


    [ClientRpc]
    public void aftercardfightClientRpc(int enemyfieldmonsterid, int myfieldmonsterid, ClientRpcParams rpcparamsclient = default)
    {
        //for the receiver, the myfieldmonsterid is basically the enemy's field monster id, and 
        //enemy field monster id is basically my field monster id
        //so I just switched the parameters orders



        //have to declare as null cuz some weird shit reasons
        FieldMonsterScript myfieldmonsterscriptinstance = null;
        FieldMonsterScript enemyfieldmonsterscriptinstance = null;


        FieldMonsterScript[] arrayoffieldmonsterscripts = FindObjectsByType<FieldMonsterScript>(FindObjectsSortMode.None);

        foreach (FieldMonsterScript individualscript in arrayoffieldmonsterscripts)
        {
            if (individualscript.fieldmonsterid == myfieldmonsterid && individualscript.gameObject.CompareTag("MyFieldMonsterTag") == true)
            {
                myfieldmonsterscriptinstance = individualscript;

                break;
            }
        }

        foreach (FieldMonsterScript individualscript in arrayoffieldmonsterscripts)
        {
            if (individualscript.fieldmonsterid == enemyfieldmonsterid && individualscript.gameObject.CompareTag("EnemyFieldMonsterTag") == true)
            {
                enemyfieldmonsterscriptinstance = individualscript;

                break;
            }
        }



        int resultingmyhealth = myfieldmonsterscriptinstance.myhealth - enemyfieldmonsterscriptinstance.mydmg;

        int resultingenemyhealth = enemyfieldmonsterscriptinstance.myhealth - myfieldmonsterscriptinstance.mydmg;



        StartCoroutine(myfieldmonsterscriptinstance.animateattack(enemyfieldmonsterscriptinstance.gameObject.transform, myfieldmonsterscriptinstance.gameObject.transform, 0.1f));



        //change my health
        myfieldmonsterscriptinstance.myhealth = resultingmyhealth; //update value

        myfieldmonsterscriptinstance.healthvalue.text = myfieldmonsterscriptinstance.myhealth.ToString(); //update text shown in game


        //change enemy health(on my screen)
        enemyfieldmonsterscriptinstance.myhealth = resultingenemyhealth; //update value
        enemyfieldmonsterscriptinstance.healthvalue.text = enemyfieldmonsterscriptinstance.myhealth.ToString(); //update text shown in game





    }








    [ServerRpc(RequireOwnership = false)]
    public void afterheroattackServerRpc(int myfieldmonsterid, ServerRpcParams rpcparamsserver = default)
    {
        ulong senderid = rpcparamsserver.Receive.SenderClientId;

        if (senderid == 0)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 } //send to client id 1 if yourself is id 0
                }
            };


            afterheroattackClientRpc(myfieldmonsterid, customclientrpcparams);
        }

        else if (senderid == 1)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 0 } //send to client id 0 if yourself is id 1
                }
            };


            afterheroattackClientRpc(myfieldmonsterid, customclientrpcparams);
        }
    }


    [ClientRpc]
    public void afterheroattackClientRpc(int enemyfieldmonsterid, ClientRpcParams rpcparamsclient = default)
    {

        //have to declare as null cuz some weird shit reasons
        FieldMonsterScript enemyfieldmonsterscriptinstance = null;


        FieldMonsterScript[] arrayoffieldmonsterscripts = FindObjectsByType<FieldMonsterScript>(FindObjectsSortMode.None);

        foreach (FieldMonsterScript individualscript in arrayoffieldmonsterscripts)
        {
            if (individualscript.fieldmonsterid == enemyfieldmonsterid && individualscript.gameObject.CompareTag("EnemyFieldMonsterTag") == true)
            {
                enemyfieldmonsterscriptinstance = individualscript;

                break;
            }
        }

        //change my health value (internally)
        myherohealth -= enemyfieldmonsterscriptinstance.mydmg;



        StartCoroutine(enemyfieldmonsterscriptinstance.animateattack(enemyfieldmonsterscriptinstance.gameObject.transform, myhero.gameObject.transform, 0.1f));



        //change my health shown on my screen
        myhero.GetComponentInChildren<TMP_Text>().text = myherohealth.ToString();

    }








    [ServerRpc(RequireOwnership = false)]
    public void herohealth0ServerRpc(ServerRpcParams rpcparamsserver = default)
    {
        ulong senderid = rpcparamsserver.Receive.SenderClientId;

        if (senderid == 0)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 } //send to client id 1 if yourself is id 0
                }
            };


            herohealth0ClientRpc(customclientrpcparams);
        }

        else if (senderid == 1)
        {
            var customclientrpcparams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 0 } //send to client id 0 if yourself is id 1
                }
            };


            herohealth0ClientRpc(customclientrpcparams);
        }
    }


    [ClientRpc]
    public void herohealth0ClientRpc(ClientRpcParams rpcparamsclient = default)
    {
        StartCoroutine(resetgamecoroutine());
    }



    public IEnumerator youlostuicoroutine()
    {
        youlostui.SetActive(true);
        yield return new WaitForSeconds(5f);
        youlostui.SetActive(false);
    }

    public IEnumerator youwonuicoroutine()
    {
        youwonui.SetActive(true);
        yield return new WaitForSeconds(5f);
        youwonui.SetActive(false);
    }


    private IEnumerator resetgamecoroutine()
    {
        //wait for the youwonui coroutine to finish
        yield return StartCoroutine(youlostuicoroutine());

        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= client1connectedfunction;
        }

        if (endturnbutton != null)
        {
            var button = endturnbutton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveListener(whenendturnbuttonisclicked);
            }
        }
        
    }












}