using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebInterface : MonoBehaviour {

    private WebserviceLogin login;

    public UserInfo.UserData user;

	// Use this for initialization
	void Start () {
        //login = new WebserviceLogin();
        //user = new UserInfo.UserData();
        //StartConnection();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartConnection()
    {
        Debug.Log(login.beginConnection());
    }
}
