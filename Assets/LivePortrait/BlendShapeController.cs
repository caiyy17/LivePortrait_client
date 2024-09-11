using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BlendShapeController : MonoBehaviour
{
    // 将对象的SkinnedMeshRenderer拖到此变量中
    public SkinnedMeshRenderer skinnedMeshRenderer;

    // 创建一个与JSON对应的类
    [System.Serializable]
    public class BlendShapeData
    {
        public int Frame;
        public string Time;
        public float eyeSquintRight;
        public float eyeBlinkRight;
        public float eyeWideRight;
        public float eyeLookUpRight;
        public float eyeLookDownRight;
        public float eyeLookOutRight;
        public float eyeLookInRight;
        public float eyeSquintLeft;
        public float eyeBlinkLeft;
        public float eyeWideLeft;
        public float eyeLookUpLeft;
        public float eyeLookDownLeft;
        public float eyeLookOutLeft;
        public float eyeLookInLeft;
        public float browDownLeft;
        public float browDownRight;
        public float browInnerUp;
        public float browOuterUpRight;
        public float browOuterUpLeft;
        public float noseSneerLeft;
        public float noseSneerRight;
        public float cheekSquintLeft;
        public float cheekSquintRight;
        public float cheekPuff;
        public float mouthUpperUpRight;
        public float mouthUpperUpLeft;
        public float mouthSmileLeft;
        public float mouthSmileRight;
        public float mouthFrownLeft;
        public float mouthFrownRight;
        public float mouthLowerDownLeft;
        public float mouthLowerDownRight;
        public float mouthPressLeft;
        public float mouthPressRight;
        public float mouthPucker;
        public float mouthShrugLower;
        public float mouthLeft;
        public float mouthRight;
        public float mouthClose;
        public float mouthFunnel;
        public float mouthDimpleLeft;
        public float mouthDimpleRight;
        public float mouthRollUpper;
        public float mouthRollLower;
        public float mouthStretchLeft;
        public float mouthStretchRight;
        public float mouthShrugUpper;
        public float tongueOut;
        public float jawOpen;
        public float jawForward;
        public float jawRight;
        public float jawLeft;
    }
    public List<string> blendShapeNames = new List<string>(){"eyeSquintRight", "eyeBlinkRight", "eyeWideRight", "eyeLookUpRight", "eyeLookDownRight", "eyeLookOutRight", "eyeLookInRight", "eyeSquintLeft", "eyeBlinkLeft", "eyeWideLeft", "eyeLookUpLeft", "eyeLookDownLeft", "eyeLookOutLeft", "eyeLookInLeft", "browDownLeft", "browDownRight", "browInnerUp", "browOuterUpRight", "browOuterUpLeft", "noseSneerLeft", "noseSneerRight", "cheekSquintLeft", "cheekSquintRight", "cheekPuff", "mouthUpperUpRight", "mouthUpperUpLeft", "mouthSmileLeft", "mouthSmileRight", "mouthFrownLeft", "mouthFrownRight", "mouthLowerDownLeft", "mouthLowerDownRight", "mouthPressLeft", "mouthPressRight", "mouthPucker", "mouthShrugLower", "mouthLeft", "mouthRight", "mouthClose", "mouthFunnel", "mouthDimpleLeft", "mouthDimpleRight", "mouthRollUpper", "mouthRollLower", "mouthStretchLeft", "mouthStretchRight", "mouthShrugUpper", "tongueOut", "jawOpen", "jawForward", "jawRight", "jawLeft"};
    // 文件路径
    public TextAsset jsonFile;

    void Start()
    {
        // 读取JSON并解析
        string jsonData = jsonFile.text;
        BlendShapeData blendShapeData = JsonUtility.FromJson<BlendShapeData>(jsonData);

        // 设置BlendShape
        SetBlendShapes(blendShapeData);
    }

    void SetBlendShapes(BlendShapeData data)
    {
        // 使用数据来设置BlendShape
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeSquintRight"), data.eyeSquintRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeBlinkRight"), data.eyeBlinkRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeWideRight"), data.eyeWideRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookUpRight"), data.eyeLookUpRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookDownRight"), data.eyeLookDownRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookOutRight"), data.eyeLookOutRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookInRight"), data.eyeLookInRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeSquintLeft"), data.eyeSquintLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeBlinkLeft"), data.eyeBlinkLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeWideLeft"), data.eyeWideLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookUpLeft"), data.eyeLookUpLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookDownLeft"), data.eyeLookDownLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookOutLeft"), data.eyeLookOutLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("eyeLookInLeft"), data.eyeLookInLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("browDownLeft"), data.browDownLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("browDownRight"), data.browDownRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("browInnerUp"), data.browInnerUp * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("browOuterUpRight"), data.browOuterUpRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("browOuterUpLeft"), data.browOuterUpLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("noseSneerLeft"), data.noseSneerLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("noseSneerRight"), data.noseSneerRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("cheekSquintLeft"), data.cheekSquintLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("cheekSquintRight"), data.cheekSquintRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("cheekPuff"), data.cheekPuff * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthUpperUpRight"), data.mouthUpperUpRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthUpperUpLeft"), data.mouthUpperUpLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthSmileLeft"), data.mouthSmileLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthSmileRight"), data.mouthSmileRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthFrownLeft"), data.mouthFrownLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthFrownRight"), data.mouthFrownRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthLowerDownLeft"), data.mouthLowerDownLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthLowerDownRight"), data.mouthLowerDownRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthPressLeft"), data.mouthPressLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthPressRight"), data.mouthPressRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthPucker"), data.mouthPucker * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthShrugLower"), data.mouthShrugLower * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthLeft"), data.mouthLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthRight"), data.mouthRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthClose"), data.mouthClose * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthFunnel"), data.mouthFunnel * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthDimpleLeft"), data.mouthDimpleLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthDimpleRight"), data.mouthDimpleRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthRollUpper"), data.mouthRollUpper * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthRollLower"), data.mouthRollLower * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthStretchLeft"), data.mouthStretchLeft * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthStretchRight"), data.mouthStretchRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("mouthShrugUpper"), data.mouthShrugUpper * 100);
        // skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("tongueOut"), data.tongueOut * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("jawOpen"), data.jawOpen * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("jawForward"), data.jawForward * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("jawRight"), data.jawRight * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("jawLeft"), data.jawLeft * 100);
        
        
    }
}
