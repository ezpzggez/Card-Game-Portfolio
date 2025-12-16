using System;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{


    public Button createlobbybutton;

    public Button refreshlobbybutton;




    public GameObject lobbyprefab;

    public GameObject scrollviewcontent;

    public GameObject insidelobbybackground;

    public TMP_Text player1idtext;


    public GameObject player2slot;
    public TMP_Text player2idtext;

    public Button startgamebutton;


    public Button leavelobbybutton;


    public GameObject lobbybackgroundui;

    public GameObject insidelobbybackgroundui;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public async void Start()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) //safe guard for reload scene
        {
            await UnityServices.InitializeAsync(); //initialize unityservices
        }







        //AuthenticationService.Instance.SignedIn += () => { Debug.Log("signed in as" + AuthenticationService.Instance.PlayerId); };

        

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //sign in anonymously 
        }


        createlobbybutton.onClick.AddListener(oncreatelobbybuttonpressed);

        refreshlobbybutton.onClick.AddListener(onrefreshbuttonclicked);








    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void oncreatelobbybuttonpressed()
    {

        try
        {

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"relayjoincodekey", new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };




            string lobbyname = "mylobby";
            int maxplayers = 2;
            Lobby mylobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxplayers, options);

            Debug.Log("successfully created lobby : " + mylobby.Name + "  max players : " + mylobby.MaxPlayers);







            insidelobbybackground.SetActive(true);

            player1idtext.text = $"My ID : {AuthenticationService.Instance.PlayerId}";


            leavelobbybutton.onClick.AddListener(() => onleavelobbyclicked(mylobby.Id));

            player2slot.SetActive(false);





            var callbacks = new LobbyEventCallbacks();

            callbacks.PlayerJoined += onplayerjoined;

            callbacks.PlayerLeft += onplayerleave;

            var sub = await LobbyService.Instance.SubscribeToLobbyEventsAsync(mylobby.Id, callbacks);

            //this shit's kinda complicated but you're just subscribing to the lobby events only when you're the creator of the lobby 






            startgamebutton.onClick.AddListener(() => onstartbuttonclicked(mylobby.Id));


            

            





        }

        catch (Exception e)
        {
            Debug.Log(e);
        }


    }



    public void onplayerjoined(List<LobbyPlayerJoined> listoflobbyplayersjoined)
    {
        startgamebutton.gameObject.SetActive(true);



        player2slot.SetActive(true);

        player2idtext.text = $" joined player id : {listoflobbyplayersjoined[0].Player.Id}";
    }


    public void onplayerleave(List<int> asdf)
    {
        startgamebutton.gameObject.SetActive(false);

        player2slot.SetActive(false);
    }






    public async void onjoinlobbybuttonclicked(string lobbyid)
    {
        try
        {
            Lobby mylobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyid);

            Debug.Log($"successfully joined lobby!!: {mylobby.Name}");




            insidelobbybackground.SetActive(true);

            leavelobbybutton.onClick.AddListener(() => onleavelobbyclicked(mylobby.Id));

            player1idtext.text = $"host id : {mylobby.HostId}";


            player2slot.SetActive(true);

            player2idtext.text = $"my id : {AuthenticationService.Instance.PlayerId}";



            //subscribe to lobbyevents to be notified if lobbydata (the join relay code) has changed and if so change transport to that relay and startclient to that transport.
            LobbyEventCallbacks callbacks = new LobbyEventCallbacks();
            callbacks.LobbyChanged += onlobbychanged;


            await LobbyService.Instance.SubscribeToLobbyEventsAsync(mylobby.Id, callbacks);




        }

        catch (Exception e)
        {
            Debug.Log($"failed to join lobby, error is : {e}");
        }
    }


    public void onlobbychanged(ILobbyChanges changes)
    {
        if (changes.Data.Value.ContainsKey("relayjoincodekey"))
        {
            Debug.Log("lobby options has changed, specifically the relay code");

            joinrelay(changes.Data.Value["relayjoincodekey"].Value.Value);
        }
    }


    public async void onrefreshbuttonclicked()
    {
        try
        {
            QueryResponse querylobbiesresponse = await LobbyService.Instance.QueryLobbiesAsync();

            refreshlobbylist(querylobbiesresponse.Results);
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    public void refreshlobbylist(List<Lobby> listoflobbies)
    {
        foreach (Transform oldlobbyinstancetransform in scrollviewcontent.transform) //so apparently if you just do a foreach of a transform, then unity automatically selects all the children of the transform.
        {
            Destroy(oldlobbyinstancetransform.gameObject);
        }



        for (int i = 0; i < listoflobbies.Count; i++)
        {
            GameObject lobbyinstance = Instantiate(lobbyprefab, scrollviewcontent.transform); //instantiate a lobby for the lobbylist


            Lobby newlobby = listoflobbies[i]; //the actual lobby (from the data)





            TMP_Text lobbynametext = lobbyinstance.transform.Find("LobbyName").GetComponent<TMP_Text>();

            lobbynametext.text = newlobby.Name;

            TMP_Text lobbynumberofplayerstext = lobbyinstance.transform.Find("LobbyPlayerCount").GetComponent<TMP_Text>();

            lobbynumberofplayerstext.text = $"{newlobby.Players.Count} / {newlobby.MaxPlayers}";


            Button joinlobbybutton = lobbyinstance.transform.Find("JoinLobbyButton").GetComponent<Button>();

            joinlobbybutton.onClick.AddListener(() => onjoinlobbybuttonclicked(newlobby.Id));


            Debug.Log($"created lobby instance with {lobbynametext.text} , {lobbynumberofplayerstext.text} after refreshing");

        }
    }



    public async void onleavelobbyclicked(string lobbyid)
    {
        //leave lobby self
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(lobbyid, AuthenticationService.Instance.PlayerId);
            Debug.Log("Left Lobby: " + lobbyid);

            insidelobbybackground.SetActive(false); //deactive inside lobby ui 
        }


        catch (Exception e)
        {
            Debug.Log(e);
        }

    }




    public async void onstartbuttonclicked(string thelobbyid)
    {
        try
        {
            Debug.Log("start button clicked");

            Allocation relayallocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joincode = await RelayService.Instance.GetJoinCodeAsync(relayallocation.AllocationId);

            Debug.Log("created allocation");



            Dictionary<string, DataObject> lobbydata = new Dictionary<string, DataObject>();

            lobbydata.Add("relayjoincodekey", new DataObject(DataObject.VisibilityOptions.Member, joincode));

            UpdateLobbyOptions updateoptionslobby = new UpdateLobbyOptions();

            updateoptionslobby.Data = lobbydata;


            await LobbyService.Instance.UpdateLobbyAsync(thelobbyid, updateoptionslobby);

            Debug.Log("updated lobby data");



            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(relayallocation, "dtls"));




            NetworkManager.Singleton.StartHost();

            Debug.Log("started host");


            lobbybackgroundui.SetActive(false);
            insidelobbybackgroundui.SetActive(false);







        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    public async void joinrelay(string relayjoincode)
    {
        try
        {
            Debug.Log($"trying to join relay with " + relayjoincode);

            var allocation = await RelayService.Instance.JoinAllocationAsync(relayjoincode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));

            NetworkManager.Singleton.StartClient();

            Debug.Log("joined client");

            lobbybackgroundui.SetActive(false);
            insidelobbybackgroundui.SetActive(false);

            
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    
    

    


}
