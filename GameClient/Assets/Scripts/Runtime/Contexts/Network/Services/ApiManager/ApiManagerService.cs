using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor.Tools.DebugX.Runtime;
using ProtoBuf;
using Riptide;
using Riptide.Utils;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.Networking;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
    public class ApiManagerService : MonoBehaviour
    {
        public static ApiManagerService instance;
        private const int SPRITE_CLEAR_TIME = 10;
        
        private string ROOT ="http://178.128.246.17/api";

        //Gorseller tekrar tekrar indirilmemesi icin cache'de tutuluyor
        public Dictionary<string, SpriteCache> Sprites = new Dictionary<string, SpriteCache>();
        public Queue<IEnumerator> DownloadQueue = new Queue<IEnumerator>();

        private Coroutine Processing;

        private void Awake()
        {
            if (instance==null)
                instance = this;
            else
                Destroy(gameObject);
            
            //oyun basladigindan 5dk sonra calismaya baslar, her 200 saniyede bir tekrar eder.
            InvokeRepeating(nameof(ClearCache), 320, 200);
        }

        #region ImageDownload
        private void OnApplicationQuit()
        {
            CancelInvoke(nameof(ClearCache));
        }

        public void ClearQueue()
        {
            DownloadQueue.Clear();
        }
        //Cache'de tutulan gorseller SPRITE_CLEAR_TIME dakikalik aralarla siliniyor.
        private void ClearCache()
        {
            Debug.Log("Sprite Cache Cleared");

            var itemsToRemove = Sprites.Where(c => DateTime.Now.Subtract(c.Value.LastUsage).TotalMinutes > SPRITE_CLEAR_TIME).ToArray();

            foreach (var item in itemsToRemove)
                Sprites.Remove(item.Key);
        }

        private void Update()
        {
            if (Processing == null && DownloadQueue.Count > 0)
            {
                Processing = StartCoroutine(DownloadQueue.Dequeue());
            }
        }
        public void DownloadImage(string url, Action<Sprite> callback, bool immediate = false)
        {
            if (Sprites.ContainsKey(url))
            {
                Sprites[url].LastUsage = DateTime.Now;
                callback(Sprites[url].Data);
            }
            else if (immediate)
            {
                var List = DownloadQueue.ToArray();
                DownloadQueue.Clear();
                DownloadQueue.Enqueue(Download(url, callback));
                foreach (var item in List)
                {
                    DownloadQueue.Enqueue(item);
                }
            }
            else
            {
                DownloadQueue.Enqueue(Download(url, callback));
            }
        }

        private IEnumerator Download(string url, Action<Sprite> callback)
        {
            if (Sprites.ContainsKey(url))
            {
                Sprites[url].LastUsage = DateTime.Now;
                callback(Sprites[url].Data);
            }
            else if (!string.IsNullOrEmpty(url))

            {
                var www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();
                if (!(www.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError) && www.responseCode == 200)
                {
                    try
                    {
                        var gonnaRemoveTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                        var image = Sprite.Create(gonnaRemoveTex, new Rect(0, 0, gonnaRemoveTex.width, gonnaRemoveTex.height), new Vector2(0, 0));
                        Sprites[url] = new SpriteCache() { Data = image };
                        callback(image);
                    }
                    catch (Exception e)
                    {
                        callback(null);
                        Debug.LogError("download error : " + e.Message);
                    }
                }
            }

            Processing = null;
        }
        #endregion
        #region ApiRequest

        public void Request<T>(string path, RequestType requestType, Action<HttpResponse<T>> onComplete = null)
        {
            Request<T>(path, requestType, onComplete: onComplete);
        }

        public void Request<T>(string path, RequestType requestType, Dictionary<string, object> map = null, Action<HttpResponse<T>> onComplete = null)
        {
            Dictionary<string, object> tempMap;
            if (map == null)
            {
                tempMap = new Dictionary<string, object>();
            }
            else
            {
                tempMap = map;
            }

            switch (requestType)
            {
                case RequestType.GET:
                    Get<T>(path, tempMap, (response) =>
                    {
                        if (onComplete != null)
                        {
                            onComplete.Invoke(response);
                        }
                    });

                    break;
                case RequestType.PUT:

                    Put<T>(path, tempMap, (response) =>
                    {
                        if (onComplete != null)
                        {
                            onComplete.Invoke(response);
                        }
                    });

                    break;
                case RequestType.POST:

                    Post<T>(path, tempMap, (response) =>
                    {
                        if (onComplete != null)
                        {
                            onComplete.Invoke(response);
                        }
                    });
                    break;
                default:
                    break;
            }
        }
        
        public void Post<T>(string path, Dictionary<string, object> parameters, Action<HttpResponse<T>> callback)
        {
            StartCoroutine(Send<T>(RequestType.POST, path, parameters, callback));
        }

        public void Put<T>(string path, Dictionary<string, object> parameters, Action<HttpResponse<T>> callback)
        {
            StartCoroutine(Send<T>(RequestType.PUT, path, parameters, callback));
        }

        public void Get<T>(string path, Dictionary<string, object> parameters, Action<HttpResponse<T>> callback)
        {
            StartCoroutine(Send<T>(RequestType.GET, path, parameters, callback));
        }
        
        private IEnumerator Send<T>(RequestType type, string path, Dictionary<string, object> parameters, Action<HttpResponse<T>> callback)
        {
            UnityWebRequest www;
            ///HTTP POST
            if (type == RequestType.POST)
            {
                www = new UnityWebRequest(ROOT + path, "POST");
                byte[] jsonToSend = new UTF8Encoding().GetBytes(convertToJson(parameters));
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                

            }
            ///HTTP PUT
            else if (type == RequestType.PUT)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(convertToJson(parameters));
                www = UnityWebRequest.Put(ROOT + path, bytes);
                www.SetRequestHeader("X-HTTP-Method-Override", "POST");
                www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
            }
            ///HTTP GET
            else
            {
                www = UnityWebRequest.Get(ROOT + path + "?" + QueryString(parameters));
            }

            if (!PlayerPrefs.GetString("token", "").Equals(""))
                www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

            www.timeout = 10;

            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Failed To Request:" +ROOT+ path + " parameters: " + QueryString(parameters) + " result: " + www.downloadHandler.text + " code: " + www.responseCode);
                callback(new HttpResponse<T>
                {
                    Error = new HttpError() { code = 500 }
                });
            }
            else
            {
                if (www.responseCode >= 200 && www.responseCode <= 400)
                {
                    try
                    {
                        //Burada eğer objesiz düz array geldiyse hataya düşmemesi için bir manuel ekleme
                        if (www.downloadHandler.text.StartsWith('['))
                        {
                            callback(new HttpResponse<T>
                            {
                                Result = JsonUtility.FromJson<T>("{\"root\":" + www.downloadHandler.text + "}")
                            });
                        }
                        else
                        {
                            callback(new HttpResponse<T>
                            {
                                Result = JsonUtility.FromJson<T>(www.downloadHandler.text)
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Hata: " + e);
                        callback(new HttpResponse<T>
                        {
                            Object = JsonUtility.FromJson<T>(www.downloadHandler.text)
                        });
                    }
                }
                else
                {
                    callback(new HttpResponse<T>
                    {
                        Error = JsonUtility.FromJson<HttpError>(www.downloadHandler.text)
                    });
                }
            }
        }

        public string convertToJson(Dictionary<string, object> parameter)
        {
            var json = new StringBuilder();

            json.Append("{");

            foreach (var item in parameter)
            {
                json.Append("\"").Append(item.Key).Append("\":");
                if (item.Value.GetType() == typeof(int) || item.Value.GetType() == typeof(bool))
                {
                    json.Append(item.Value);
                }
                else
                {
                    json.Append("\"").Append(item.Value).Append("\"");
                }

                json.Append(",");
            }
            if (json.Length > 1)
            {
                json.Remove(json.Length - 1, 1);
            }

            json.Append("}");

            return json.ToString();
        }

        /// <summary>
        /// Burası dictionary değerlerini string olarak key=value şeklinde göstermeye yarar.
        /// </summary>
        /// <param name="dict">String'e dönüştürülecek Dictionary</param>
        /// <returns> Key=Value&Key=Value şeklinde string döndürür</returns>
        public static string QueryString(IDictionary<string, object> dict)
        {
            //dict.Add("v", "1.0");
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list.ToArray());
        }
        #endregion

    }
    public class SpriteCache
    {
      public Sprite Data;
      public DateTime LastUsage = DateTime.Now;
    }

    public enum RequestType
    {
      GET,
      PUT,
      POST
    }
    public class HttpResponse<T>
    {
      public T Result;
      public HttpError Error;
      public object Object;
    }
    [Serializable]
    public class HttpError
    {
      public int code;
    }

}