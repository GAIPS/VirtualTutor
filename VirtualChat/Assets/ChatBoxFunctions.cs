using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxFunctions : MonoBehaviour {

    [SerializeField]
    ContentSizeFitter contentSizeFitter;
    //[SerializeField]
    //Text showHideButtonText;
    [SerializeField]
    Transform messageParentPanel;
    [SerializeField]
    GameObject newMessagePrefab;
    [SerializeField]
    Transform buttonParentPanel;
    [SerializeField]
    GameObject newButtonsPrefab;
    bool greeting = false;
    StringBuilder sb = new StringBuilder(".");
    GameObject last;
    int p = 0;

    bool isChatShowing = false;
    string message = "";
    int number = 0;
    UserInfo.UserData user;
    public static ChatBoxFunctions Instance;

    public void Start()
    {
        GameObject log = GameObject.Find("LoginMoodle");
        user = log.GetComponent("UserData") as UserInfo.UserData;
        Instance = this;
        AddAPoint();
        Invoke("replying", 4.0f);
        ToggleChat();
    }

    //public void Update()
    //{
    //    UserInfo.UserData user = log.GetComponent("User Data") as UserInfo.UserData;
    //    if (!user.readyForRead) 
    //    {

    //    }
    //}


    /*
    * Abrir e fechar chat (não está em uso)
    */
    public void ToggleChat()
    {
        isChatShowing = !isChatShowing;
        if(isChatShowing) {
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            //showHideButtonText.text = "Hide Chat";
        } else {
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            //showHideButtonText.text = "Show Chat";
        }
    }

    /*
     * Definir mensagem a enviar
     */
    public void SetMessage (string message)
    {
        this.message = message;
    }

    /*
     * Mostra mensagem no chat
     */
    public void ShowMessage()
    {
        if (message != "")
        {
            Message(message);
            p = 0;
            sb = new StringBuilder(".");
            AddAPoint();
            Invoke("replying", 4.0f);
        }
        GameObject f = GameObject.Find("Panel");
        f.GetComponent<Scrollbar>().value = 0;
    }
    
    /*
     * Resposta do tutor virtual
     */
    public void replying()
    {
        if (!greeting) { 
            responseMessage("Tutor: " + user.giveData(), "Olá!");
            greeting = !greeting;
        } else
        {
            if (number == 0)
            {
                responseMessage("Tutor: Em que te posso ajudar?", "Tenho dúvidas!", "Não obrigado!");
            }
            else if (number == 1)
            {
                responseMessage("Tutor: Estou a ser util?", "Sim", "Não", "Responder mais tarde");
            }
            else
            {
                responseMessage("Tutor: Deseja continar?", "Sim", "Não");
                number = 0;
            }
            number++;
        }
    }

    /*
     * Resposta para um botão
     */
    public void responseMessage(String m, String ms)
    {
        Message(m);
        CreateButton(ms);
    }

    /*
     * Resposta para dois botões
     */
    public void responseMessage(String m, String ms, String ms2)
    {
        Message(m);
        CreateButton(ms, ms2);
    }

    /*
     * Resposta para três botões
     */
    public void responseMessage(String m, String ms, String ms2, string ms3)
    {
        Message(m);
        CreateButton(ms, ms2, ms3);
    }

    /*
     * Apresentar mensagem
     */
    public void Message(String m)
    {
        GameObject response = (GameObject)Instantiate(newMessagePrefab);
        response.transform.SetParent(messageParentPanel);
        response.transform.SetSiblingIndex(messageParentPanel.childCount - 2);
        response.GetComponent<MessageFuntions>().ShowMessage(m);
        last = response;
    }

    /*
     * Criação de botões (com um)
     */
    private void CreateButton(string m)
    {
        GameObject response = (GameObject)Instantiate(newButtonsPrefab);
        response.transform.SetParent(buttonParentPanel);
        response.transform.SetSiblingIndex(buttonParentPanel.childCount - 1);
        response.GetComponent<ButtonsFuctions>().ShowMessage(m);
        response.GetComponent<ButtonsFuctions>().AddFunction();
    }

    /*
     * Criação de botões (com dois)
     */
    private void CreateButton(string m, string m2)
    {
        GameObject response = (GameObject)Instantiate(newButtonsPrefab);
        response.transform.SetParent(buttonParentPanel);
        response.transform.SetSiblingIndex(buttonParentPanel.childCount - 1);
        response.GetComponent<ButtonsFuctions>().ShowMessage(m, m2);
        response.GetComponent<ButtonsFuctions>().AddFunction();
    }

    /*
     * Criação de botões (com três)
     */
    private void CreateButton(string m, string m2, string ms3)
    {
        GameObject response = (GameObject)Instantiate(newButtonsPrefab);
        response.transform.SetParent(buttonParentPanel);
        response.transform.SetSiblingIndex(buttonParentPanel.childCount - 1);
        response.GetComponent<ButtonsFuctions>().ShowMessage(m, m2, ms3);
        response.GetComponent<ButtonsFuctions>().AddFunction();
    }

    /*
     * Mostrar 3 pontinhos
     */
    public void AddAPoint()
    {
        if(p>0 && p<=3)
            Destroy(last);
        if (p < 3)
        {
            Message(sb.ToString());
            sb.Append(".");
            Invoke("AddAPoint", 1.0f);
        }
        p++;
    }
}
