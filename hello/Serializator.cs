using System.Collections;
using System.Threading; 

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace nsUtil
{
	namespace nsLogicUtil
	{ 
		/// <summary>
		/// 직렬화된 클래스를 bin, xml, Soap등으로 저장하고 로드하는 클래스. 기본적으로 스트림을 주고 받음.
		/// / 직렬화 대상은 [System.Serializable]  , 비직렬화 대상은 [System.NonSerialized] 
		/// / xml 속성으로 지정시 [XmlAttribute("속성명")]   , xml출력 제외항목은 [XmlIgnore]를 붙임
		/// / 주의점: Bin은 메모리 덩어리로 저장하고 불러들이기에 클래스의 초기화 셋팅을 동작을 무시하지만, 
		///           Xml은 클래스를 생성하고 값을 넣는 방식이라 클래스내의 초기화 셋팅 동작을함.
		///           (예) [System.NonSerialized] 속성의 int i = 10; 이라면 Bin은 역직렬화시 0으로 값이 될테고, Xml은 역직렬화시 10이 됨.
		/// </summary>
		/// <typeparam name="TN"></typeparam>
		public static class SerializeClass<TN> where TN : class
		{
			/// <summary>
			/// 직렬화 된 클래스를 지정위치로 바이너리로 저장함.
			/// </summary>
			/// <param name="filePath">경로 및 파일이름 지정</param>
			/// <param name="serializeClass">직렬화된 클래스</param>
			/// <returns>성공유무</returns>
			public static bool SerializeToBin(string filePath, TN serializeClass)
			{
				if (null == serializeClass)
				{
					return false;
				}
				
				try
				{
					SerializeFileCreate(filePath, (stream) =>
					                    {
						BinaryFormatter formatter = new BinaryFormatter();
						formatter.Serialize(stream, serializeClass);
					});
				}
				catch (System.Exception e)
				{
					
					if( true == File.Exists(filePath))
					{ File.Delete(filePath);  }
					
					return false;
				}
				
				return true;
			}
			
			
			
			/// <summary>
			/// SerializeToBin()의 역직렬화
			/// </summary>
			/// <param name="filePath"></param>
			/// <param name="serializeClass"></param>
			/// <returns></returns>
			public static bool DeserializeFromBin(string filePath, out TN serializeClass)
			{
				try
				{
					serializeClass = DeserializeFileOpen(filePath, (stream) =>
					                                     {
						BinaryFormatter formatter = new BinaryFormatter();
						return formatter.Deserialize(stream) as TN;
					});
				}
				catch (System.Exception e)
				{
					serializeClass = null;
					return false;
				}
				
				return true;
			}
			
			
			
			/// <summary>
			/// 유니티 어셋으로 부터 바이너리 역직렬화
			/// </summary>
			/// <param name="textAsset"></param>
			/// <param name="serializeClass"></param>
			/// <returns></returns>
			public static bool DeserializeFromBin(byte[] bytes, out TN serializeClass)
			{
				try
				{
					using (Stream stream = new MemoryStream( bytes))
					{
						BinaryFormatter formatter = new BinaryFormatter();
						serializeClass = formatter.Deserialize(stream) as TN;
					}
				}
				catch (System.Exception e)
				{
					serializeClass = null;
					return false;
				}
				
				return true;
			}
			
			
			
			private static void SerializeFileCreate(string filePath, System.Action<Stream> serializeAction)
			{
				using (Stream stream = new FileStream(filePath, FileMode.Create))
				{
					serializeAction(stream);
				}
			}
			
			private static TN DeserializeFileOpen(string filePath, System.Func<Stream, TN> serializeFn)
			{
				TN returnValue = null;
				using (Stream stream = new FileStream(filePath, FileMode.Open))
				{ returnValue = serializeFn(stream) as TN; }
				
				return returnValue;
			} 
		}
		
		
	}
}