using UnityEngine;
using System.Collections;
using nsUtil.nsLogicUtil;

[System.Serializable]
public class Hello {
	public int a;
	public int b;

	public override string ToString() {
		return string.Format("a={0},b={1}", a, b);
	}
}


public class NewBehaviourScript : MonoBehaviour {
	void Start () {

//		InstanceToXml();
//		XmlToInstance();

//		InstanceToBytes();
		BytesToInstance();
	}
	
	void InstanceToXml() {
		var hello = new Hello{a=10, b=20};
		var path = Application.dataPath + "/Resources/" + "hello.xml";
		SerializeClass<Hello>.SerializeToXml(path, hello);
	}

	void XmlToInstance() {
		var path = "hello";
		var textasset = Resources.Load<TextAsset>(path);
		var hello = new Hello();
		SerializeClass<Hello>.DeserializeFromXml(textasset, out hello);
		Debug.Log(hello);
	}

	void InstanceToBytes() {
		var hello = new Hello{a=10, b=20};
		var path = Application.dataPath + "/Resources/" + "hello_bytes.bytes";
		SerializeClass<Hello>.SerializeToBin(path, hello);
	}

	void BytesToInstance() {
		var path = "hello_bytes";
		var textasset = Resources.Load<TextAsset>(path);
		var hello = new Hello();
		SerializeClass<Hello>.DeserializeFromBin(textasset, out hello);
		Debug.Log(hello);
	}
}
