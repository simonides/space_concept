using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
namespace Custom{
	namespace Utility{
		public sealed class Vector3Surrogate : ISerializationSurrogate
		{
			public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) 
			{
				Vector3 v = (Vector3) obj;
				info.AddValue( "x", v.x );
				info.AddValue( "y", v.y );
				info.AddValue( "z", v.z );
			}
			
			public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) 
			{
				Vector3 v;
				v.x = (float)info.GetValue("x", typeof(float));
				v.y = (float)info.GetValue("y", typeof(float));
				v.z = (float)info.GetValue("z", typeof(float));
				obj = v;
				return obj;
			}
		}
	}
}