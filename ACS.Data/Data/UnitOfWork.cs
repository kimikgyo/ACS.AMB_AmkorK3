using INA_ACS_Server;
using System;
using System.Data;

namespace INA_ACS_Server
{
    public class ConnectionStrings
    {
       

#if LOCALDB

        public static readonly string DB2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=AmkorK3_User_Data; User ID = sa; Password=acsserver;Connect Timeout=30;"; // My(Kimikgyo) PC
        public static readonly string DB3 = @"Data Source=.\SQLEXPRESS;Initial Catalog=RobotAPI; User ID = sa; Password=acsserver;Connect Timeout=30;"; // My(Kimikgyo) PC
        public static readonly string DB1 = @"Data Source=.\SQLEXPRESS;Initial Catalog=AmkorK3_AMB; User ID = sa; Password=acsserver;Connect Timeout=30;"; // My(Kimikgyo) PC

#else
        // ACS PC만 연결
        public static readonly string DB1 = @"Data Source=192.168.100.230;Initial Catalog=KGC_Buyeo;User ID = sa; Password=acsserver;Connect Timeout=30;";
        public static readonly string DB2 = @"Data Source=192.168.100.230;Initial Catalog=KGC_Buyeo;User ID = sa; Password=acsserver;Connect Timeout=30;";
        public static readonly string DB3 = @"Data Source=192.168.100.230;Initial Catalog=KGC_Buyeo;User ID = sa; Password=acsserver;Connect Timeout=30;";
#endif
    }


    public interface IUnitOfWork : IDisposable
    {
        ACSChargerCountConfigRepository ACSChargerCountConfigs { get; }
        ACSRobotGroupRepository ACSRobotGroups { get; }
        ChargeMissionConfigRepository ChargeMissionConfigs { get; }
        ErrorCodeListRepository ErrorCodeLists { get; }
        ErrorHistoryRepository ErrorHistorys { get; }
        FloorMapIDConfigRepository FloorMapIDConfigs { get; }
        JobConfigRepository JobConfigs { get; }
        JobLogRepository JobLogs { get; }
        JobRepository Jobs { get; }
        MissionRepository Missions { get; }
        PositionAreaConfigRepository PositionAreaConfigs { get; }
        PositionWaitTimeHistoryRepository PositionWaitTimeHistorys { get; }
        PositionWaitTimeRepository PositionWaitTimes { get; }
        RobotRegisterSyncRepository RobotRegisterSyncs { get; }
        RobotRepository Robots { get; }
        WaitMissionConfigRepository WaitMissionConfigs { get; }
        FleetPositionRepository FleetPositions { get; }
        ElevatorStateRepository ElevatorState { get; }
        ElevatorInfoRepository ElevatorInfo { get; }
        MissionsSpecificRepository MissionsSpecific { get; }

        UserEmailAddressRepository UserEmailAddress { get; }

        UserNumberRepositoy UserNumber { get; }
        void SaveChanges();
        //void BeginTransaction();
        //bool Commit();
        //void Rollback();
        //IRepository<TEntity> Repository<TEntity>();
    }


    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _db;

        private static readonly string connectionString = ConnectionStrings.DB1;
        private static readonly string connectionString2 = ConnectionStrings.DB2;
        public ACSChargerCountConfigRepository ACSChargerCountConfigs { get; private set; }
        public ACSRobotGroupRepository ACSRobotGroups { get; private set; }
        public ChargeMissionConfigRepository ChargeMissionConfigs { get; private set; }
        public ErrorCodeListRepository ErrorCodeLists { get; private set; }
        public ErrorHistoryRepository ErrorHistorys { get; private set; }
        public FloorMapIDConfigRepository FloorMapIDConfigs { get; private set; }
        public JobConfigRepository JobConfigs { get; private set; }
        public JobLogRepository JobLogs { get; private set; }
        public JobRepository Jobs { get; private set; }
        public MissionRepository Missions { get; private set; }
        public PositionAreaConfigRepository PositionAreaConfigs { get; private set; }
        public PositionWaitTimeHistoryRepository PositionWaitTimeHistorys { get; private set; }
        public PositionWaitTimeRepository PositionWaitTimes { get; private set; }
        public RobotRegisterSyncRepository RobotRegisterSyncs { get; private set; }
        public RobotRepository Robots { get; private set; }
        public WaitMissionConfigRepository WaitMissionConfigs { get; private set; }
        public FleetPositionRepository FleetPositions { get; private set; }
        public ElevatorStateRepository ElevatorState { get; private set; }
        public ElevatorInfoRepository ElevatorInfo { get; private set; }
        public MissionsSpecificRepository MissionsSpecific { get; private set; }
        public UserEmailAddressRepository UserEmailAddress { get; private set; }
        public UserNumberRepositoy UserNumber { get; private set; }

        public UnitOfWork()
        {
            ACSChargerCountConfigs = new ACSChargerCountConfigRepository(connectionString);
            ACSRobotGroups = new ACSRobotGroupRepository(connectionString);
            ChargeMissionConfigs = new ChargeMissionConfigRepository(connectionString);
            ErrorCodeLists = new ErrorCodeListRepository(connectionString);
            ErrorHistorys = new ErrorHistoryRepository(connectionString);
            FloorMapIDConfigs = new FloorMapIDConfigRepository(connectionString);
            JobConfigs = new JobConfigRepository(connectionString);
            JobLogs = new JobLogRepository(connectionString);
            PositionAreaConfigs = new PositionAreaConfigRepository(connectionString);
            PositionWaitTimeHistorys = new PositionWaitTimeHistoryRepository(connectionString);
            PositionWaitTimes = new PositionWaitTimeRepository(connectionString);
            RobotRegisterSyncs = new RobotRegisterSyncRepository(connectionString);
            WaitMissionConfigs = new WaitMissionConfigRepository(connectionString);
            FleetPositions = new FleetPositionRepository(connectionString);
            Robots = new RobotRepository(connectionString);
            Missions = new MissionRepository(connectionString, Robots);
            Jobs = new JobRepository(connectionString, Missions);
            ElevatorState = new ElevatorStateRepository(connectionString);
            ElevatorInfo = new ElevatorInfoRepository(connectionString);
            MissionsSpecific = new MissionsSpecificRepository(connectionString);
            UserEmailAddress = new UserEmailAddressRepository(connectionString2);
            UserNumber = new UserNumberRepositoy(connectionString2);
        }

        public void SaveChanges()
        {
            //begin transaction
            //...update robots
            //...update missions
            //commit/rollback
        }

        public void Dispose()
        {
            //_db.Dispose();
        }
    }
}
