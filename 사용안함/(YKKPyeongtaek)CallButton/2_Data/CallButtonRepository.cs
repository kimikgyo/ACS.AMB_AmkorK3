using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class CallButtonRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<CallButton> _callButtons = new List<CallButton>(); // cached data



        public CallButtonRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }

        // DB에서 모든 항목을 로드하여 _callButtons 에 캐싱해 둔다
        private void Load()
        {
            _callButtons.Clear();

            // DB에서 읽어와서 캐싱해 둔다
            using (var con = new SqlConnection(connectionString))
            {
                //var configs = con.Query<MissionConfig>("SELECT * FROM MissionConfigs WHERE UseFlag=1");
                var callButtons = con.Query<CallButton>("SELECT * FROM CallButtons").ToList();

                // 각 callbutton 데이터를 내부 리스트에 추가한다. 없으면 만들어서 추가한다
                for (int i = 0; i < ConfigData.CallButton_MaxNum; i++)
                {
                    int buttonIndex = i + 1;

                    var callButton = callButtons.SingleOrDefault(cb => cb.ButtonIndex == buttonIndex);
                    if (callButton != null)
                    {
                        callButton.LastAccessTime = DateTime.Now;
                        _callButtons.Add(callButton);
                    }
                    else
                    {
                        //var config = configs.SingleOrDefault(cfg => cfg.CallButtonIndex == buttonIndex);
                        //if (config != null)
                        //{
                        //    Add(new CallButton
                        //    {
                        //        ButtonIndex = config.CallButtonIndex,
                        //        ButtonName = config.CallButtonName,
                        //        IpAddress = config.CallButtonIpAddress,
                        //        MissionCount = 0,
                        //        LastAccessTime = DateTime.Now,
                        //    });
                        //}
                        //else
                        //{
                        //    Add(new CallButton
                        //    {
                        //        ButtonIndex = buttonIndex,
                        //        ButtonName = $"P{buttonIndex}",
                        //        IpAddress = $"192.168.1.{100 + buttonIndex:000}",
                        //        MissionCount = 0,
                        //        LastAccessTime = DateTime.Now,
                        //    });
                        //}
                    }
                }

                // 각 callbutton 의 logger 를 설정한다
                _callButtons.ForEach(x => x.logger = LogManager.GetLogger($"CallButton{x.ButtonIndex}"));
            }
        }

        public CallButton Add(CallButton model)
        {
            _callButtons.Add(model);

            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO CallButtons
                               ([ButtonIndex]
                               ,[ButtonName]
                               ,[IpAddress]
                               ,[ConnectionState]
                               ,[LastAccessTime]
                               ,[AccessElapsedTime]
                               ,[MissionCount]
                               ,[MissionStateText])
                           VALUES
                               (@ButtonIndex
                               ,@ButtonName
                               ,@IpAddress
                               ,@ConnectionState
                               ,@LastAccessTime
                               ,@AccessElapsedTime
                               ,@MissionCount
                               ,@MissionStateText);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                return model;
            }
        }

        public List<CallButton> GetAll()
        {
            return _callButtons.ToList();
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<CallButton>("SELECT * FROM CallButtons").ToList();
            }
        }

        public CallButton GetById(int id)
        {
            return _callButtons.SingleOrDefault(m => m.Id == id);
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<CallButton>("SELECT * FROM CallButtons WHERE Id=@id",
                    param: new { id = id }).FirstOrDefault();
            }
        }

        public CallButton GetByButtonIndex(int buttonIndex)
        {
            return _callButtons.SingleOrDefault(m => m.ButtonIndex == buttonIndex);
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<CallButton>("SELECT * FROM CallButtons WHERE ButtonIndex=@ButtonIndex",
                    param: new { ButtonIndex = buttonIndex }).FirstOrDefault();
            }
        }

        public CallButton GetByName(string name)
        {
            return _callButtons.SingleOrDefault(m => m.ButtonName == name);
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<CallButton>("SELECT * FROM CallButtons WHERE ButtonName=@buttonName",
                    param: new { buttonName = name }).FirstOrDefault();
            }
        }

        public void Remove(CallButton model)
        {
            _callButtons.Remove(model);

            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM CallButtons WHERE Id=@id",
                    param: new { id = model.Id });
            }
        }

        public void Update(CallButton model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string UPDATE_SQL = @"
                    UPDATE CallButtons 
                    SET 
                        ButtonIndex=@ButtonIndex, 
                        ButtonName=@ButtonName, 
                        IpAddress=@IpAddress, 
                        ConnectionState=@ConnectionState, 
                        LastAccessTime=@LastAccessTime, 
                        AccessElapsedTime=@AccessElapsedTime, 
                        MissionCount=@MissionCount, 
                        MissionStateText=@MissionStateText 
                    WHERE Id=@Id";

                con.Execute(UPDATE_SQL, param: model);
            }
        }

    }
}
