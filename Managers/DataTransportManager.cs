using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.Managers
{
    static class DataTransportManager
    {
        private static Dictionary<string, object?> data = new Dictionary<string, object?>();

		public static int SetData(params (string key, object value)[] items)
        {
            foreach(var item in items){
				if (data.ContainsKey(item.key))
				{
					data[item.key] = item.value;
				}
				else
				{
					data.Add(item.key, item.value);
				}
			}
            return items.Length;
		}
        public static object? GetData(string key)
        {
            return data.GetValueOrDefault(key, 0);
		}

        public static void ClearData()
        {
            data.Clear();
		}
	}
}
