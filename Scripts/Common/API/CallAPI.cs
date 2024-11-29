using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using HVPUnityBase.Base.DesignPattern;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using GameNamespace;
using Unity.VisualScripting.Antlr3.Runtime;
using SimpleJSON;
public class CallAPI : MonoBehaviour
{
    public string username;
    public string password;
    public string jsonBody;
    public string token;

    Response response;
    PostData postData;
    void Start()
    {
        postData = new PostData();
        response = new Response();
    }
    public void ButtonRegister()
    {
        ResetData();
        //register
        StartCoroutine(Regisger("http://localhost:5000/api/register", jsonBody));
    }
    public void ButtonLogin()
    {
        ResetData();
        //login
        StartCoroutine(Login("http://localhost:5000/api/login", jsonBody));
    }
    public void ButtonChangeData()
    {
        ResetData();
        //login
        StartCoroutine(ChangeData("http://localhost:5000/api/patch", jsonBody, token));
    }
    public void ResetData()
    {
        postData.username = this.username;
        postData.password = this.password;

        // Chuyển đối tượng thành JSON
        this.jsonBody = JsonUtility.ToJson(postData);
    }
    IEnumerator Login(string url, string jsonBody)
    {
        Debug.Log("Sending JSON: " + jsonBody); // In payload gửi từ Unity

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            token = response.token;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    IEnumerator Regisger(string url, string jsonBody)
    {
        Debug.Log("Sending JSON: " + jsonBody); // In payload gửi từ Unity

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error: {request.error}, Response: {request.downloadHandler.text}");
        }
    }
    IEnumerator ChangeData(string url, string jsonBody, string token)
    {

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {token}");
        Debug.Log("Sending JSON: " + jsonBody + " Sending token: " + token); // In payload gửi từ Unity
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error: {request.error}, Response: {request.downloadHandler.text}");
        }
    }
}
public class API : SingletonMonoBehaviour<CallAPI> { }
