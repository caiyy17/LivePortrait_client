using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

public class UnityRecieve_iFacialMocap : MonoBehaviour
{
	static UdpClient udp;
	Thread thread;
	SkinnedMeshRenderer meshTarget;
	List<SkinnedMeshRenderer> meshTargetList;
	string faceObjGrpName = "";
	string objectNames = "";
	List<GameObject> objectArray;

	private string messageString = "";
	public int LOCAL_PORT = 50003;

    //////////////////////////////////////////////////
    // Custom blendshape structure
    //////////////////////////////////////////////////
    private BSWeight currentBSWeight;

    public class BSWeight{
        public Vector3 headPos = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 headRot = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 eyeLPos = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 eyeLRot = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 eyeRPos = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 eyeRRot = new Vector3(0.0f, 0.0f, 0.0f);
        public float[] face = new float[54];
    }

	// Start is called 
	void Start()
	{
		udp = new UdpClient(LOCAL_PORT);
		udp.Client.ReceiveTimeout = 4;

		thread = new Thread(new ThreadStart(ThreadMethod));
		thread.Start();

        currentBSWeight = new BSWeight();
        for(int i = 0; i < 54; i++){
            currentBSWeight.face[i] = 0.0f;
        }
	}

	// Update is called once per frame
	void Update()
	{
		try
		{
			string[] strArray1 = messageString.Split('=');

			if (strArray1.Length == 2)
			{
				//blendShapes)
				foreach (string message in strArray1[0].Split('|'))
				{
					string[] strArray2 = message.Split('-');

					if (strArray2.Length == 1)
					{
						string[] strArray3 = strArray2[0].Split('!');

						if (strArray3[0] == "faceObjGrp")
						{
							if (faceObjGrpName != strArray3[1])
							{
								faceObjGrpName = strArray3[1];
								meshTargetList = new List<SkinnedMeshRenderer>();
								GameObject faceObjGrp = GameObject.Find(faceObjGrpName);
								if (faceObjGrp != null)
								{
									List<GameObject> list = GetAllChildren.GetAll(faceObjGrp);

									foreach (GameObject obj in list)
									{
										meshTarget = obj.GetComponent<SkinnedMeshRenderer>();
										if (meshTarget != null)
										{
											if(HasBlendShapes(meshTarget)==true)
											{
												meshTargetList.Add(meshTarget);
											}
										}
									}
								}
							}
						}
					}
					else if (strArray2.Length == 2)
					{
						string mappedShapeName = strArray2[0];
						float weight = float.Parse(strArray2[1], CultureInfo.InvariantCulture);

						for (int i = 0; i < meshTargetList.Count; i++)
						{
							int index = meshTargetList[i].sharedMesh.GetBlendShapeIndex(mappedShapeName);
							if (index > -1)
							{
								meshTargetList[i].SetBlendShapeWeight(index, weight);
							}
						}
					}
				}
				string objectNamesNow = GetCombineNames(strArray1[1]);

				if (objectNamesNow != objectNames)
				{
					UpdateObjectArray(strArray1[1]);
				}


				int objectArrayIndex = 0;
				//jointNames
				foreach (string message in strArray1[1].Split('|'))
				{
					string[] strArray2 = message.Split('#');

					if (strArray2.Length == 2)
					{
						string[] commaList = strArray2[1].Split(',');
						string[] objNameList = commaList[3].Split(':');
						for (int j = 0; j < objNameList.Length; j++)
						{
                            if(objNameList[j] == "eyeBall_L" || objNameList[j] == "eyeBall_R")
                            {
                                if (objectArray[objectArrayIndex] != null)
                                {
                                    if (strArray2[0].Contains("Position"))
                                    {
                                        objectArray[objectArrayIndex].transform.localPosition = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        objectArray[objectArrayIndex].transform.localEulerAngles = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                            objectArrayIndex++;

//////////////////////////////////////////////////
// Update custom blendshapes
//////////////////////////////////////////////////
                            // Debug.Log(objNameList[j]);
                            if(objNameList[j] == "eyeBall_L"){
                                if (strArray2[0].Contains("Position"))
                                {
                                    currentBSWeight.eyeLPos = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    currentBSWeight.eyeLRot = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                            }
                            else if(objNameList[j] == "eyeBall_R"){
                                if (strArray2[0].Contains("Position"))
                                {
                                    currentBSWeight.eyeRPos = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    currentBSWeight.eyeRRot = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                            }
                            else if(objNameList[j] == "head"){
                                if (strArray2[0].Contains("Position"))
                                {
                                    currentBSWeight.headPos = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    currentBSWeight.headRot = new Vector3(float.Parse(commaList[0], CultureInfo.InvariantCulture), float.Parse(commaList[1], CultureInfo.InvariantCulture), float.Parse(commaList[2], CultureInfo.InvariantCulture));
                                }
                            }
//////////////////////////////////////////////////
// Update custom blendshapes
//////////////////////////////////////////////////
						}
					}
				}
			}
		}
		catch
		{ }
	}

	void ThreadMethod()
	{
		//Process once every 5ms
		long next = DateTime.Now.Ticks + 50000;
		long now;

		while (true)
		{
			try
			{
				IPEndPoint remoteEP = null;
				byte[] data = udp.Receive(ref remoteEP);
				messageString = Encoding.ASCII.GetString(data);
			}
			catch
			{
			}

			do
			{
				now = DateTime.Now.Ticks;
			}
			while (now < next);
			next += 50000;
		}
	}

	private string GetCombineNames(string strArray)
	{
		StringBuilder sb = new StringBuilder("");
		foreach (string message in strArray.Split('|'))
		{
			if (message != "")
			{
				string[] strArray1 = message.Split('#');
				string[] commaList = strArray1[1].Split(',');
				sb.Append(commaList[3]);
				sb.Append(",");
			}

		}
		return sb.ToString();
	}

	private void UpdateObjectArray(string strArray)
	{
		objectArray = new List<GameObject>();
		foreach (string message in strArray.Split('|'))
		{
			if (message != "")
			{
				string[] strArray1 = message.Split('#');
				string[] commaList = strArray1[1].Split(',');
				string[] objNameList = commaList[3].Split(':');
				for (int i = 0; i < objNameList.Length; i++)
				{
					GameObject bufObj = GameObject.Find(objNameList[i]);

					objectArray.Add(bufObj);

					
				}
			}
		}
	}

	public string GetMessageString()
	{
		return messageString;
	}

	void OnApplicationQuit()
	{
		thread.Abort();
	}

	public void StopUDP()
	{
		udp.Close();
		thread.Abort();
	}

	void Stop()
	{
		try
		{
			StopUDP();
		}
		catch (IOException)
		{
		}
	}

	private bool HasBlendShapes(SkinnedMeshRenderer skin)
	{
		if (!skin.sharedMesh)
		{
			return false;
		}
		if (skin.sharedMesh.blendShapeCount <= 0)
		{
			return false;
		}
		return true;
	}
}
public static class GetAllChildren
{
	public static List<GameObject> GetAll(this GameObject obj)
	{
		List<GameObject> allChildren = new List<GameObject>();
		allChildren.Add(obj);
		GetChildren(obj, ref allChildren);
		return allChildren;
	}

	public static void GetChildren(GameObject obj, ref List<GameObject> allChildren)
	{
		Transform children = obj.GetComponentInChildren<Transform>();
		if (children.childCount == 0)
		{
			return;
		}
		foreach (Transform ob in children)
		{
			allChildren.Add(ob.gameObject);
			GetChildren(ob.gameObject, ref allChildren);
		}
	}
}