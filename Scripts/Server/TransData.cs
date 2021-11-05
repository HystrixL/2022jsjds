using System;
using System.Text.Json;
using System.Windows;

namespace Co_Work.Network
{
    class TransData<T>
    {
        public TransData(T content,string clientGuid,string sourceRequest = "")
        {
            SourceRequest = sourceRequest;
            ClientGuid = clientGuid;
            Content = content;
        }

        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string SourceRequest { get; set; } = "";
        public string ClientGuid { get; set; }
        public DateTime SendData { get; set; } = DateTime.Now;
        public string Type { get; set; } = typeof(T).Name;
        public T Content { get; set; }
        public override string ToString()
        {
            return Type + ":" + JsonSerializer.Serialize(this);
        }
        public static TransData<T> Convert(string dataString)
        {
            //MessageBox.Show(dataString);
            return JsonSerializer.Deserialize<TransData<T>>(dataString.Replace(typeof(T).Name+":",""));
        }
    }
}