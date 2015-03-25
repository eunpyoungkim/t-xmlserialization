using System;
using System.IO;
using nsUtil.nsLogicUtil;

namespace FFF {
	[System.Serializable]
	public class Hello {
		public int a;
		public int b;
		
		public override string ToString() {
			return string.Format("a={0},b={1}", a, b);
		}
	}
}

namespace hello
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			InstanceToBytes();
			BytesToInstance();
			Console.WriteLine ("Hello World!");
		}
		
		
		static void InstanceToBytes() {
			var hello = new FFF.Hello{a=10, b=20};
			var path = "hello_bytes.bytes";
			SerializeClass<FFF.Hello>.SerializeToBin(path, hello);
		}
		
		static void BytesToInstance() {
			var path = "hello_bytes.bytes";
			var bytes = File.ReadAllBytes(path);
			var hello = new FFF.Hello();
			SerializeClass<FFF.Hello>.DeserializeFromBin(bytes, out hello);
			Console.WriteLine(hello);
		}
	}
}
