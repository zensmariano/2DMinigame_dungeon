using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;


public class Server : MonoBehaviour
{

    public int port = 6666;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener serverListener;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            serverListener = new TcpListener(IPAddress.Any, port);
            serverListener.Start();

            StartListening();
            serverStarted = true;
        }
        catch(Exception ex)
        {
            Debug.Log("Socket Error: " + ex.Message);
        }
    }
    private void Update()
    {
        if (!serverStarted)
            return;
        foreach( ServerClient c in clients)
        {
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if(data!= null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }
        }
        for(int i = 0; i< disconnectList.Count -1; i++)
        {
            // Tell players somebody has disconnected
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }
    private void StartListening()
    {
        serverListener.BeginAcceptTcpClient(AcceptTcpClient, serverListener);
    }
    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        string allUsers = "";
        foreach (var c in clients)
        {
            allUsers += c.clientName + '|';
        }
        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        StartListening();
        
        BroadCast("SWHO|"+allUsers,new List<ServerClient>{clients[clients.Count -1]});

        Debug.Log("Somebody has connected!");
    }
    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }
    // send from server
    private void BroadCast(string data, List<ServerClient> cl)
    {
        foreach (var sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception ex)
            {
                Debug.Log("Write error:" + ex.Message);
            }

            
        }
        
    }
    // Server Read
    private void OnIncomingData(ServerClient c,string data)
    {
        Debug.Log("Server"+data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.clientName = aData[1];
                c.isHost = aData[2] != "0";
                BroadCast("SCNN|"+c.clientName, clients);
                break;
        }
    }
}
public class ServerClient
{
    public string clientName;
    public TcpClient tcp;
    public bool isHost;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}