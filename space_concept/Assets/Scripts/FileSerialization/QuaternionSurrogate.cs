﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

namespace Custom{
	namespace Utility{
		public class QuaternionSurrogate : ISerializationSurrogate{

			public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context){
				Quaternion q = (Quaternion)obj;
				info.AddValue ("x", q.x);
				info.AddValue ("y", q.y);
				info.AddValue ("z", q.z);
				info.AddValue ("w", q.w);
			}

			public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector){
				Quaternion q;
				q.x = (float)info.GetValue ("x", typeof(float));
				q.y = (float)info.GetValue ("y", typeof(float));
				q.z = (float)info.GetValue ("z", typeof(float));
				q.w = (float)info.GetValue ("w", typeof(float));
				obj = q;
				return obj;
			}
		}
	}
}
