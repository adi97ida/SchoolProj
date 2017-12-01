using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public string connection = "Server=tcp:monitoringsys.database.windows.net,1433;Initial Catalog=MonitoringSystemm;Persist Security Info=False;User ID=sysadmin;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<DataPack> GetData()
        {
            List < DataPack> result =null;
            const string selectString = "select * from dbo.sensor_data";
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
                            for (int i = 0; i < 6; i++)
                            {
                                var data = reader.GetSqlValue(i);
                            }
                            pack.CurrentTime = reader.GetDateTime(0);
                            pack.IR1 = reader.GetInt32(1);
                            pack.IR1 = reader.GetInt32(2);
                            pack.Temperature = reader.GetDouble(3);
                            pack.Humidity = reader.GetInt32(4);
                            pack.Name = reader.GetString(5);
                            result.Add(pack);
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return result;
                    }
                }
            }
        }

        public string SaveData(DataPack data)
        {
            const string insert = "insert into dbo.sensor_data(CurrentTime, IRSensor1, IRSensor2, TempSensor, HumiditySensor, Name) values (@CurrentTime, @IRSensor1, @IRSensor2, @TempSensor, @HumiditySensor, @Name)";
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

        



    }
}
