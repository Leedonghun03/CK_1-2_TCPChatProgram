using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Threading;

public class TCPClient : MonoBehaviour
{
    public InputField _Name;
    string Nikname;
    public InputField Chat;
    public Text getText;

    string SERVERIP = "127.0.0.1";
    const int SERVERPORT = 9000;
    const int BUFSIZE = 512;
    TcpClient m_Client;
    NetworkStream stream;

    // ������ ��ſ� ����� ����
    string data;

    private readonly Queue<string> textQueue = new Queue<string>();
    int retval;

    void Start()
    {

    }

    void Update()
    {
        while(textQueue.Count != 0)
        {
            getText.text += $"{textQueue.Dequeue()}\n";
        }
    }

    public void Client()
    {
        try
        {
            // ���� ����
            m_Client = new TcpClient(SERVERIP, SERVERPORT);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        Nikname = _Name.text;
        data = Nikname + "���� �����ϼ̽��ϴ�.";

        try
        {
            byte[] senddata = Encoding.Default.GetBytes(data);
            int size = senddata.Length;

            if (size > BUFSIZE) size = BUFSIZE;

            stream = m_Client.GetStream();

            Thread t = new Thread(GetMessage);
            t.Start();
            t.IsBackground = true;

            stream.Write(senddata, 0, size);
        }
        catch (Exception e)
        {
            Debug.Log(e);

            //��Ʈ�� �ݱ�
            stream.Close();
            // ���� �ݱ�
            m_Client.Close();
        }
    }

    void GetMessage()
    {
        byte[] buf = new byte[BUFSIZE];
        int nbytes;
        string output;

        while (true)
        {
            try
            {
                // ������ �ޱ�
                nbytes = stream.Read(buf, 0, buf.Length);
                output = Encoding.Default.GetString(buf, 0, nbytes);

                Debug.Log(output);
                textQueue.Enqueue(output);
            }
            catch (Exception e)
            {
                Debug.Log(e);

                //��Ʈ�� �ݱ�
                stream.Close();
                // ���� �ݱ�
                m_Client.Close();
                break;
            }
        }
    }

    // ������ ������ ���
    public void DataSender()
    {
        // ������ �Է�
        data = Nikname + ": " + Chat.text;

        try
        {
            // ������ ������ (�ִ� ���̸� BUFSIZE�� ����)
            byte[] senddata = Encoding.Default.GetBytes(data);
            int size = senddata.Length;

            if (size > BUFSIZE) size = BUFSIZE;

            stream.Write(senddata, 0, size);
        }
        catch (Exception e)
        {
            Debug.Log(e);

            stream.Close();
            m_Client.Close();
        }

        Chat.text = "";
    }

    private void OnApplicationQuit()
    {
        stream.Close();
        m_Client.Close();
    }
}
