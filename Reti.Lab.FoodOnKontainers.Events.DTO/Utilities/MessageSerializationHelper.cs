using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Events.DTO.Utilities
{
    public class MessageSerializationHelper
    {
        public static string SerializeObject(object objToSerialize)
        {
            return JsonConvert.SerializeObject(objToSerialize);
        }


        public static T DeserializeObject<T>(string strToDeserialize) where T : class
        {
            return JsonConvert.DeserializeObject<T>(strToDeserialize);
        }


        public static byte[] SerializeObjectToBin(object objToSerialize)
        {
            string objSerialized = JsonConvert.SerializeObject(objToSerialize);
            return Encoding.UTF8.GetBytes(objSerialized);
        }

        public static T DeserializeObjectFromBin<T>(byte[] objToDeserialize) where T : class
        {
            string strToDeserialize = Encoding.UTF8.GetString(objToDeserialize);

            return JsonConvert.DeserializeObject<T>(strToDeserialize);
        }

    }
}
