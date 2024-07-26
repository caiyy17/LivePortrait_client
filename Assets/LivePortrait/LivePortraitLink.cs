using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

public class LivePortraitLink : MonoBehaviour
{
    private ClientWebSocket webSocket;
    private CancellationTokenSource cts;

    private bool isWaitingForResponse = false;
    private bool isApplicationQuitting = false;
    
    public RenderTexture renderTexture;
    public RenderTexture outTexture;
    public RawImage rawImage;
    public TextMeshProUGUI fpsDisplay;
    private float deltaTime = 0.0f;

    async void Start()
    {
        webSocket = new ClientWebSocket();
        cts = new CancellationTokenSource();

        await webSocket.ConnectAsync(new Uri("ws://127.0.0.1:5000/ws"), cts.Token);
        StartCoroutine(CaptureAndSendRoutine());
        StartReceiving();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    IEnumerator CaptureAndSendRoutine()
    {
        yield return new WaitForEndOfFrame();
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        byte[] imageBytes = texture2D.EncodeToJPG(75);
        yield return StartCoroutine(InitImageToPythonServer(imageBytes));
        yield return StartCoroutine(SendImageToPythonServer(imageBytes));
        
        while (true)
        {
            if (!isWaitingForResponse)
            {
                yield return new WaitForEndOfFrame();
                CaptureAndSendImage();
            }
            yield return null; // 等待下一帧
        }
    }
    
    async void CaptureAndSendImage()
    {
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        byte[] imageBytes = texture2D.EncodeToJPG(75);
        var buffer = new ArraySegment<byte>(imageBytes);

        if (webSocket.State == WebSocketState.Open)
        {
            isWaitingForResponse = true;
            await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, cts.Token);
        }
    }
    
    async void StartReceiving()
    {
        float startTime, fps;
        startTime = Time.time;
        fps = 0;
        
        var buffer = new byte[1024 * 1024];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
            if(isApplicationQuitting){
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
            else
            {
                var receivedBytes = new byte[result.Count];
                Array.Copy(buffer, receivedBytes, result.Count);

                Texture2D receivedTexture = new Texture2D(2, 2);
                receivedTexture.LoadImage(receivedBytes);
                rawImage.texture = receivedTexture;
                ShowProcessedTexture(receivedTexture);
                
                
                deltaTime = Time.time - startTime;
                if(fps == 0){
                    fps = 1.0f / deltaTime;
                }
                else{
                    fps = (1.0f / deltaTime) * 0.1f + fps * 0.9f;
                }
                fpsDisplay.text = $"Update FPS: {fps:0.}";
                startTime = Time.time;
                
                // 允许发送下一帧
                isWaitingForResponse = false;
            }
        }
    }

    // IEnumerator CaptureRenderTexture()
    // {
    //     yield return new WaitForEndOfFrame();
    //
    //     RenderTexture.active = renderTexture;
    //     Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    //     texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //     texture2D.Apply();
    //
    //     byte[] imageBytes = texture2D.EncodeToJPG(75);
    //
    //     // 将 imageBytes 发送给 Python 服务
    //     yield return StartCoroutine(InitImageToPythonServer(imageBytes));
    //     yield return StartCoroutine(SendImageToPythonServer(imageBytes));
    //
    //     float startTime, fps;
    //     startTime = Time.time;
    //     fps = 0;
    //     while (true)
    //     {
    //         yield return new WaitForEndOfFrame();
    //
    //         RenderTexture.active = renderTexture;
    //         texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    //         texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //         texture2D.Apply();
    //
    //         imageBytes = texture2D.EncodeToJPG(75);
    //         // 将 imageBytes 发送给 Python 服务
    //         yield return StartCoroutine(SendImageToPythonServer(imageBytes));
    //         // 记录结束时间并计算帧率
    //         deltaTime = Time.time - startTime;
    //         if(fps == 0){
    //             fps = 1.0f / deltaTime;
    //         }
    //         else{
    //             fps = (1.0f / deltaTime) * 0.1f + fps * 0.9f;
    //         }
    //         fpsDisplay.text = $"Update FPS: {fps:0.}";
    //         startTime = Time.time;
    //     }
    // }

    IEnumerator InitImageToPythonServer(byte[] imageBytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, "screenshot.png", "image/jpeg");

        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/init", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log( "Init success" );
            }
        }
    }

    IEnumerator SendImageToPythonServer(byte[] imageBytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, "screenshot.png", "image/jpeg");
    
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/process", form))
        {
            yield return www.SendWebRequest();
    
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] resultBytes = www.downloadHandler.data;
    
                // 处理返回的数据
                Texture2D resultTexture = new Texture2D(2, 2);
                resultTexture.LoadImage(resultBytes);
                // 显示在 Unity 中
                ShowProcessedTexture(resultTexture);
                Debug.Log( "Process success" );
            }
        }
    }

    void ShowProcessedTexture(Texture2D texture)
    {
        Graphics.Blit(texture, outTexture);
        // 将处理后的纹理显示在 Unity 中的某个对象上，例如一个 RawImage 组件
        rawImage.texture = texture;
        
    }
    
    private async void OnApplicationQuit()
    {
        // Stop all coroutines
        isApplicationQuitting = true;
        StopAllCoroutines();
        // 等待1秒，确保 Python 服务有足够的时间处理完所有请求
        await Task.Delay(1000);
        if (webSocket != null)
        {
            if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
            {
                try
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application quitting", CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Debug.LogError("WebSocket close error: " + ex.Message);
                }
            }
            webSocket.Dispose();
        }
        cts.Cancel();
    }
}