using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Security.Cryptography;
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
        List<DataPack> GetDataByPeriod(DateTime? dateStart, DateTime? dateEnd);
        [OperationContract]
        WeatherModel GetWeather(string city, MethodType type, Days? day);
        [OperationContract]
        Staff Login(string username, string password);
        [OperationContract]
        List<Customers24H> CustomersInsidePer24H(DateTime dateStart, DateTime dateEnd);
        [OperationContract]
        string CreateStaffUser(Staff new_user);
        [OperationContract]
        Discount EditDiscount(Discount discount);
        [OperationContract]
        string RemoveDiscount(string id);
        [OperationContract]
        List<Discount> GetDiscounts();




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
    [DataContract]
    public class Staff
    {
        [DataMember]
        public string Username;
        [DataMember]
        public string Password;
        [DataMember]
        public string Full_Name;
        [DataMember]
        public string Email;
    }
    [DataContract]
    public class Customers24H
    {
        [DataMember]
        public int PeopleInside;
        [DataMember]
        public DateTime CurrentTime;
    }
    [DataContract]
    public class Discount
    {
        [DataMember]
        public string Title;
        [DataMember]
        public string Description;
        [DataMember]
        public DateTime DateValid;
        [DataMember]
        public Double Value;
    }
}
