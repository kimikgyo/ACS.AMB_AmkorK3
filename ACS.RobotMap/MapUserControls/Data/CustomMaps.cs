using Dapper;
using log4net;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace ACS.RobotMap
{
    internal static class CustomMaps
    {
        /*
         * 
            CREATE TABLE CustomMaps (
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [UpdateTime] [datetime] NULL,
	            [MapName] [varchar](50) NULL,
	            [MapImageData] [varchar](max) NULL
            )
         * 
         */

        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static object lockObj = new object();


        public static DateTime GetMapImageTime(string targetMapName)
        {
            lock (lockObj)
            {
                try
                {
                    using (var con = new SqlConnection(ConnectionStrings.DB1))
                    {
                        return con.QueryFirstOrDefault<DateTime>(@"SELECT TOP 1 UpdateTime FROM CustomMaps WHERE MapName=@mapName",
                            new { mapName = targetMapName });
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Info("MainForm/GetMapImageTime() Fail = " + ex.Message);
                    return default;
                }
            }
        }

        public static string GetMapImageData(string targetMapName)
        {
            lock (lockObj)
            {
                try
                {
                    using (var con = new SqlConnection(ConnectionStrings.DB1))
                    {
                        return con.QueryFirstOrDefault<string>("SELECT TOP 1 MapImageData FROM CustomMaps WHERE MapName = @mapName",
                            new { mapName = targetMapName });
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Info("MainForm/GetMapImageData() Fail = " + ex.Message);
                    return null;
                }
            }
        }

        public static void SetMapImageData(string targetMapName, string mapImageData)
        {
            lock (lockObj)
            {
                try
                {
                    // save encoded data
                    using (var con = new SqlConnection(ConnectionStrings.DB1))
                    {
                        con.Execute(@"DELETE FROM CustomMaps WHERE MapName = @mapName;
                                  INSERT INTO CustomMaps VALUES (@updateTime, @mapName, @mapImageData)",
                            new
                            {
                                mapName = targetMapName,
                                mapImageData,
                                updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            });
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Info("MainForm/SetMapImageData() Fail = " + ex.Message);
                }
            }
        }

        public static string ConvertImageToEncodedString(Image image)
        {
            lock (lockObj)
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static Image ConvertEncodedStringToImage(string mapEncodedString)
        {
            lock (lockObj)
            {
                byte[] mapDecodedBytes = Convert.FromBase64String(mapEncodedString);
                using (var ms = new MemoryStream(mapDecodedBytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }

    }
}
