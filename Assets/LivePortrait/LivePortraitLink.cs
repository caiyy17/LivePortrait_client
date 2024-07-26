using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LivePortraitLink : MonoBehaviour
{
    public RenderTexture renderTexture;
    public RenderTexture outTexture;
    public RawImage rawImage;
    public TextMeshProUGUI fpsDisplay;
    private float deltaTime = 0.0f;

    void Start()
    {
        StartCoroutine(CaptureRenderTexture());
    }

    void Update()
    {
    }

    IEnumerator CaptureRenderTexture()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        byte[] imageBytes = texture2D.EncodeToJPG(75);

        // 将 imageBytes 发送给 Python 服务
        yield return StartCoroutine(InitImageToPythonServer(imageBytes));
        yield return StartCoroutine(SendImageToPythonServer(imageBytes));

        float startTime, fps;
        startTime = Time.time;
        fps = 0;
        while (true)
        {
            yield return new WaitForEndOfFrame();

            RenderTexture.active = renderTexture;
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            imageBytes = texture2D.EncodeToJPG(75);
            // 将 imageBytes 发送给 Python 服务
            yield return StartCoroutine(SendImageToPythonServer(imageBytes));
            // 记录结束时间并计算帧率
            deltaTime = Time.time - startTime;
            if(fps == 0){
                fps = 1.0f / deltaTime;
            }
            else{
                fps = (1.0f / deltaTime) * 0.1f + fps * 0.9f;
            }
            fpsDisplay.text = $"Update FPS: {fps:0.}";
            startTime = Time.time;
        }
    }

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
            }
        }
    }

    void ShowProcessedTexture(Texture2D texture)
    {
        Graphics.Blit(texture, outTexture);
        // 将处理后的纹理显示在 Unity 中的某个对象上，例如一个 RawImage 组件
        rawImage.texture = texture;
        
    }
}
