using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using World;

public class APIService : MonoBehaviour
{    
    private static string BASE_ENDPOINT = "https://us-central1-edtech2022-cd734.cloudfunctions.net/api/";
    private static string WORLDS_ENDPOINT = BASE_ENDPOINT + "worlds/";
    private static string PRE_SURVEY_ENDPOINT = BASE_ENDPOINT + "surveypre/";
    private static string POST_SURVEY_ENDPOINT = BASE_ENDPOINT + "surveypost/";
    private static string USER_ENDPOINT = BASE_ENDPOINT + "user/";

    public string access_token { get; set; }
    private static bool created = false;
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            // Navigated back to main menu so we can reset "selected world"
            access_token = null;
            Destroy(gameObject);
        }
    }


    private static APIService _instance;


    public static APIService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<APIService>();
                if (_instance == null)
                {
                    var gameObject = new GameObject();
                    gameObject.name = typeof(APIService).Name;
                    _instance = gameObject.AddComponent<APIService>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return _instance;
        }
    }

    private IEnumerator Get(string url, System.Action<string> callBack)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", "Bearer " + access_token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    callBack(jsonResult);
                }
            }
        }
    }

    private IEnumerator Post(string url, string data, System.Action<string> callBack)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, data))
        {
            www.SetRequestHeader("Authorization", "Bearer " + access_token);
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    callBack(jsonResult);
                }


            }

        }
    }

    private IEnumerator Put(string url, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(url, data))
        {
            www.SetRequestHeader("Authorization", "Bearer " + access_token);
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("done");
                }
            }


        }

    }
    private IEnumerator Delete(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            www.SetRequestHeader("Authorization", "Bearer " + access_token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("done");
                }
            }
        }
    }

    public void GetWorlds(System.Action<List<ServerWorld>> callBack)
    {
        StartCoroutine(Get(WORLDS_ENDPOINT, (jsonData) =>
        {
            Debug.Log(jsonData);
            List<ServerWorld> worlds = JsonConvert.DeserializeObject<List<ServerWorld>>(jsonData);
            callBack(worlds);
        }));
    }

    private static JsonSerializerSettings serializationSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    public void CreateWorld(ServerWorld world, System.Action<string> callBack)
    {
        string json = JsonConvert.SerializeObject(world, serializationSettings);
        Debug.Log(json);
        StartCoroutine(Post(WORLDS_ENDPOINT, json, callBack));
    }

    public void SendPresurvey(string surveyData, System.Action<string> callBack)
    {
        StartCoroutine(Post(PRE_SURVEY_ENDPOINT, surveyData, callBack));
    }

    public void SendPostsurvey(string surveyData, System.Action<string> callBack)
    {
        StartCoroutine(Post(POST_SURVEY_ENDPOINT, surveyData, callBack));
    }

    public void DeleteWorld(string id)
    {
        string url = WORLDS_ENDPOINT + id;
        StartCoroutine(Delete(url));
    }

    public void UpdateWorld(ServerWorld world, string id)
    {
        string url = WORLDS_ENDPOINT + id;
        string json = JsonConvert.SerializeObject(world);
        StartCoroutine(Put(url, json));
    }
}
