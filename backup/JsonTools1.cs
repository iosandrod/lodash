﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace Algorithm
{
    public class JsonTools
    {
         public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream(); serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length]; stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }
        public static string ObjectToJson2(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static object JsonToObject2(string jsonString, object obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, obj.GetType());
        }
    }
}