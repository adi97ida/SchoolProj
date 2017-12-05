using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using APIXULib;

namespace API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        string SaveData(DataPack data);
        [OperationContract]
        List<DataPack> GetData(DateTime? dateStart, DateTime? dateEnd);
        [OperationContract]
        WeatherModel GetWeather(string city, MethodType type, Days? day);

    }
    [DataContract]
    public class DataPack
    {
        [DataMember]
        public string Name;

        [DataMember]
        public int IR1;

        [DataMember]
        public int IR2;

        [DataMember]
        public int Humidity;

        [DataMember]
        public double Temperature;
        [DataMember]
        public DateTime CurrentTime;
    }
}
