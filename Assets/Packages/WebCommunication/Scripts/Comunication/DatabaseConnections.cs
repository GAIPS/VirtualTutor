using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UserInfo;

public class DatabaseConnections
{
    [Serializable]
    public class Values
    {
        public List<jsonValues.logins> logins;
    }

    private Values v;
    private String location = "https://tutoria-virtual.uab.pt/webservices/teste/";
    private String sLocation = "https://tutoria-virtual.uab.pt/webservices/teste/moodlereplica/"; // secret location

    String secretKey = "mySecretKey";

    public UserData User;

    public Boolean finished, success = false;

    public Hashtable hashtable = new Hashtable();

    // Use this for initialization
    public DatabaseConnections(UserData user)
    {
        User = user;
    }

    public IEnumerator GetLogins(List<int> course)
    {
        finished = false;
        LinkedList<String> parameters;
        List<jsonValues.logins> logins = new List<jsonValues.logins>();
        foreach (int c in course)
        {
            parameters = new LinkedList<string>();
            parameters.AddLast(c.ToString());
            parameters.AddLast(User.id.ToString());


            WWW www = new WWW(location + "login.php?function=getlogin&userid=" + User.id + "&courseid=" + c + "&hash=" +
                              EncryptHash(parameters));
            yield return www;
            String result = www.text;

            String final = "{\"logins\":" + result + "}";

            v = JsonUtility.FromJson<Values>(final);
            foreach (jsonValues.logins l in v.logins)
                logins.Add(l);
        }

        hashtable.Add("getLogins", logins);
    }

    public IEnumerator InsertLogin(int course)
    {
        finished = false;
        int seq = v.logins.Count + 1;
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        LinkedList<String> parameters = new LinkedList<string>();
        parameters.AddLast(course.ToString());
        parameters.AddLast(seconds.ToString());
        parameters.AddLast(seq.ToString());
        parameters.AddLast(User.id.ToString());


        WWW www = new WWW(location + "login.php?function=putlogin&userid=" + User.id + "&courseid=" + course +
                          "&login=" + seconds + "&seqn=" + seq + "&hash=" + EncryptHash(parameters));
        yield return www;
        String result = www.text;

        finished = true;
        if (result.Contains("Record updated successfully"))
            if (!hashtable.ContainsKey("insertLogin"))
                hashtable.Add("insertLogin", true);

            else if (!hashtable.ContainsKey("insertLogin"))
                hashtable.Add("insertLogin", false);
    }

    public String EncryptHash(LinkedList<String> parameters)
    {
        MD5 md5 = MD5.Create();
        StringBuilder sb = new StringBuilder();
        foreach (String s in parameters)
        {
            sb.Append(s);
        }

        sb.Append(secretKey);

        byte[] toBytes = Encoding.ASCII.GetBytes(sb.ToString());


        byte[] hash = md5.ComputeHash(toBytes);
        sb.Remove(0, sb.Length);
        foreach (byte b in hash)
            sb.Append(b.ToString("X2"));

        return sb.ToString().ToLower();
    }

    public IEnumerator PrepareRequest(String fileName, Hashtable parameters)
    {
        return MakeRequest(fileName, parameters);
    }

    public IEnumerator PrepareRequests(String fileName, List<Hashtable> parameters)
    {
        return RequestCycle(fileName, parameters);
    }

    public IEnumerator RequestCycle(String fileName, List<Hashtable> parameters)
    {
        String function = null;
        List<String> results = new List<string>();
        foreach (Hashtable h in parameters)
        {
            if (h.ContainsKey("function"))
                function = h["function"].ToString();
            yield return MakeRequests(fileName, h, results);
        }

        if (function != null)
            hashtable.Add(function, results);
    }

    public IEnumerator MakeRequests(String fileName, Hashtable parameters, List<string> results)
    {
        Boolean secret = false;
        LinkedList<string> toHash = new LinkedList<string>();

        StringBuilder sb = new StringBuilder();
        //sb.Append(location + fileName + "?");


        foreach (var s in parameters.Keys.Cast<string>().OrderBy(c => c))
        {
            if (s.Contains("iden"))
            {
                foreach (var sin in parameters[s] as string[])
                {
                    sb.Append("iden[]=" + sin + "&");
                    toHash.AddLast(sin);
                }
            }
            else
            {
                if (s.Contains("secret"))
                {
                    secret = true;
                }
                else
                {
                    sb.Append(s + "=" + parameters[s] + "&");
                    if (!s.Equals("function"))
                    {
                        toHash.AddLast(parameters[s].ToString());
                    }
                }
            }
        }


        WWW www;
        sb.Append("hash=" + EncryptHash(toHash));
        if (secret)
        {
            //Debug.Log(sLocation + fileName + "?" + sb.ToString());
            www = new WWW(sLocation + fileName + "?" + sb);
        }
        else
        {
            //Debug.Log(location + fileName + "?" + sb.ToString());
            www = new WWW(location + fileName + "?" + sb);
        }

        yield return www;
        results.Add(www.text);
    }


    public IEnumerator MakeRequest(String fileName, Hashtable parameters)
    {
        Boolean secret = false;
        LinkedList<String> toHash = new LinkedList<string>();

        StringBuilder sb = new StringBuilder();
        //sb.Append(location + fileName + "?");


        foreach (String s in parameters.Keys.Cast<String>().OrderBy(c => c))
        {
            if (s.Contains("iden"))
            {
                foreach (String sin in parameters[s] as String[])
                {
                    sb.Append("iden[]=" + sin + "&");
                    toHash.AddLast(sin);
                }
            }
            else
            {
                if (s.Contains("secret"))
                {
                    secret = true;
                }
                else
                {
                    sb.Append(s + "=" + parameters[s] + "&");
                    if (!s.Equals("function"))
                    {
                        toHash.AddLast(parameters[s].ToString());
                    }
                }
            }
        }


        WWW www;
        sb.Append("hash=" + EncryptHash(toHash));
        if (secret)
        {
            //Debug.Log(sLocation + fileName + "?" + sb.ToString());
            www = new WWW(sLocation + fileName + "?" + sb);
        }
        else
        {
            //Debug.Log(location + fileName + "?" + sb.ToString());
            www = new WWW(location + fileName + "?" + sb);
        }

        yield return www;

        hashtable.Add(parameters["function"], www.text);
    }
}