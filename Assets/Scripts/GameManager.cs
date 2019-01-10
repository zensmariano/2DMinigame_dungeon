using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;

    public GameObject ServerPrefab;
    public GameObject ClientPrefab;

    public InputField nameInput;
	// Use this for initialization
	void Start () {

        Instance = this;
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);
	}
    public void ConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
        Debug.Log("Connect");
    }
    public void HostButton()
    {
        try{
            Server s = Instantiate(ServerPrefab).GetComponent<Server>();
            s.Init();
            Client c = Instantiate(ClientPrefab).GetComponent<Client>();
            c.isHost = true;
            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Host";
            c.ConnectToServer("127.0.0.1", 6666);

        }
        catch(Exception e)
        {

        }
        mainMenu.SetActive(false);
        serverMenu.SetActive(true);
        Debug.Log("Connect");
    }
    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (hostAddress == "")
            hostAddress = "127.0.0.1";

        try
        {
            Client c = Instantiate(ClientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Host";
            c.ConnectToServer(hostAddress, 6666);
            connectMenu.SetActive(false);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);

        Server s = FindObjectOfType<Server>();
        if(s!=null)
            Destroy(s.gameObject);
        Client c = FindObjectOfType<Client>();
        if(c!=null)
            Destroy(c.gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
