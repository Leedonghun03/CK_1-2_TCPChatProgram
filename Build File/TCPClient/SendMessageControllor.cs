using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageControllor : MonoBehaviour
{
    public GameObject client;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            client.GetComponent<TCPClient>().DataSender();
        }
    }
}
