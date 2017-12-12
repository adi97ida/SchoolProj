using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Threading.Tasks;
using APIXULib;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;

namespace API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    public class Service : IService
    {
        public string connection = "Server=tcp:monitoringsys.database.windows.net,1433;Initial Catalog=MonitoringSystemm;Persist Security Info=False;User ID=sysadmin;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<DataPack> GetDataByPeriod(DateTime? dateStart, DateTime? dateEnd)
        {
            List<DataPack> result = null;
            string selectString = (dateStart != null || dateEnd != null)? $"select * from dbo.sensor_data where CurrentTime >= '{dateStart ?? new DateTime(2000,1,1)}' AND CurrentTime <= '{dateEnd ?? DateTime.Now}' order by CurrentTime asc": "select * from dbo.sensor_data order by CurrentTime asc";
            using (SqlConnection sqlServer = new SqlConnection(connection))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectString, sqlServer))
                {
                    try
                    {
                        sqlServer.Open();
                        var reader = selectCommand.ExecuteReader();
                        result = new List<DataPack>();
                        DataPack pack;
                        while (reader.Read())
                        {
                            pack = new DataPack();
                            pack.CurrentTime = reader.GetDateTime(0);
                            pack.IR1 = reader.GetInt32(1);
                            pack.IR1 = reader.GetInt32(2);
                            pack.Temperature = reader.GetDouble(3);
                            pack.Humidity = reader.GetInt32(4);
                            pack.Name = reader.GetString(5);
                            result.Add(pack);
                        }
                        sqlServer.Close();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                return result;
            }
        }
        
        private int GetLastCustomerNo()
        {
            int last_in;
            string name;
            const string getCustomerNos = "SELECT Name, PeopleInside FROM dbo.sensor_data WHERE ID=(SELECT max(ID) FROM dbo.sensor_data);";
            using (SqlConnection sqlServer = new SqlConnection(connection))
            {
                sqlServer.Open();
                using (SqlCommand selectData = new SqlCommand(getCustomerNos, sqlServer))
                {
                    try
                    {
                        SqlDataReader read = selectData.ExecuteReader();
                        read.Read();
                        name = read.GetString(0);
                        last_in = read.GetInt32(1);
                        read.Close();
                    }
                    catch
                    {
                        last_in = 0;
                    }
                }
                sqlServer.Close();
            }
            return last_in;
        }

        public string SaveData(DataPack data)
        {
            const string insert = "insert into dbo.sensor_data(CurrentTime, IRSensor1, IRSensor2, TempSensor, HumiditySensor, Name) values (@CurrentTime, @IRSensor1, @IRSensor2, @TempSensor, @HumiditySensor, @Name)";
            int last_customer_count = 0;
            if (data.Name != "STOP")
                last_customer_count = GetLastCustomerNo();
            using (SqlConnection sqlServer = new SqlConnection(connection))
            {
                sqlServer.Open();
                
                using (SqlCommand insertData = new SqlCommand(insert, sqlServer))
                {
                    insertData.Parameters.AddWithValue("@CurrentTime", DateTime.Now);
                    insertData.Parameters.AddWithValue("@IRSensor1", data.IR1);
                    insertData.Parameters.AddWithValue("@IRSensor2", data.IR2);
                    insertData.Parameters.AddWithValue("@TempSensor", data.Temperature);
                    insertData.Parameters.AddWithValue("@HumiditySensor", data.Humidity);
                    if(data.Name=="STOP")
                        insertData.Parameters.AddWithValue("@PeopleInside", 0);
                    else
                        insertData.Parameters.AddWithValue("@PeopleInside", (last_customer_count+data.IR1)-data.IR2);
                    
                    insertData.Parameters.AddWithValue("@Name", data.Name);


                    try
                        {
                            if (insertData.ExecuteNonQuery() != 0)
                                return string.Format("OK");

                        }
                        catch (Exception ex)
                        {
                            return string.Format("STOP: " + ex.Message);
                        }
                }
            }
            return "ERROR";
        }
        public WeatherModel GetWeather(string city, MethodType type, Days? day)
        {

            Days days = Days.One;
            string key = "b244896d727f4cf28aa113256170412";

            if (!day.HasValue)
                days = (Days)day;

            WeatherModel weather = new WeatherModel();
            IRepository repo = new Repository();

            if (type == MethodType.Current)
                return weather = repo.GetWeatherData(key, GetBy.CityName, city);
            else
                return weather = repo.GetWeatherData(key, GetBy.CityName, city, days);
            
        }
        
        public Staff Login(string username, string password)
        {
            const string select = "select * from dbo.user where username=''";
            return new Staff();
        }

        public List<Customers24H> CustomersInsidePer24H(DateTime dateStart, DateTime dateEnd)
        {
            List<Customers24H> c_today = new List<Customers24H>();
            Customers24H record;
            string get_records = $"SELECT PeopleInside, CurrentTime FROM dbo.sensor_data WHERE CurrentTime >= '{ dateStart }' AND CurrentTime<='{ dateEnd }' order by PeopleInside desc;";
            using (SqlConnection sqlServer = new SqlConnection(connection))
            {
                sqlServer.Open();
                using (SqlCommand selectData = new SqlCommand(get_records, sqlServer))
                {
                    try
                    {
                        SqlDataReader read = selectData.ExecuteReader();
                        while (read.Read())
                        {
                            record = new Customers24H();
                            record.CurrentTime = read.GetDateTime(1);
                            record.PeopleInside = read.GetInt32(0);
                            c_today.Add(record);
                        }
                        read.Close();
                    }
                    catch
                    {
                        
                    }
                }
                sqlServer.Close();
            }
            return c_today;
        }


    }
}
