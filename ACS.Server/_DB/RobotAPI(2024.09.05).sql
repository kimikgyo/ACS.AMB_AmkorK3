USE [master]
GO
/****** Object:  Database [RobotAPI]    Script Date: 2024-09-05 오후 2:27:33 ******/
CREATE DATABASE [RobotAPI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RobotAPI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\RobotAPI.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RobotAPI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\RobotAPI_log.ldf' , SIZE = 466944KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RobotAPI] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RobotAPI].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RobotAPI] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RobotAPI] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RobotAPI] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RobotAPI] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RobotAPI] SET ARITHABORT OFF 
GO
ALTER DATABASE [RobotAPI] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RobotAPI] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RobotAPI] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RobotAPI] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RobotAPI] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RobotAPI] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RobotAPI] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RobotAPI] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RobotAPI] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RobotAPI] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RobotAPI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RobotAPI] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RobotAPI] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RobotAPI] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RobotAPI] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RobotAPI] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RobotAPI] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RobotAPI] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RobotAPI] SET  MULTI_USER 
GO
ALTER DATABASE [RobotAPI] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RobotAPI] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RobotAPI] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RobotAPI] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RobotAPI] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RobotAPI] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RobotAPI] SET QUERY_STORE = OFF
GO
USE [RobotAPI]
GO
/****** Object:  Table [dbo].[Robots]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Robots](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[ACSRobotGroup] [varchar](50) NULL,
	[ACSRobotActive] [int] NULL,
	[Fleet_State] [int] NULL,
	[Fleet_State_Text] [varchar](50) NULL,
	[RobotID] [int] NULL,
	[RobotIp] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[StateID] [varchar](50) NULL,
	[StateText] [varchar](50) NULL,
	[MissionText] [varchar](500) NULL,
	[MissionQueueID] [int] NULL,
	[MapID] [varchar](50) NULL,
	[BatteryPercent] [float] NULL,
	[DistanceToNextTarget] [float] NULL,
	[Position_Orientation] [float] NULL,
	[Position_X] [float] NULL,
	[Position_Y] [float] NULL,
	[Product] [varchar](50) NULL,
	[Door] [varchar](50) NULL,
	[PositionZoneName] [varchar](50) NULL,
	[ErrorsJson] [varchar](5000) NULL,
	[RobotModel] [varchar](500) NULL,
	[RobotAlias] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[RobotInfo]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[RobotInfo]
as 
select JobId,RobotID,RobotName [Name],ACSRobotGroup [Group],ACSRobotActive [Active],Fleet_State_Text [FleetState],StateText [RobotState],Position_X [X],Position_Y [Y],Product 
from Robots
GO
/****** Object:  Table [dbo].[ACSChargerCountConfig]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACSChargerCountConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChargerCountUse] [varchar](50) NULL,
	[RobotGroupName] [varchar](50) NULL,
	[FloorName] [varchar](50) NULL,
	[FloorMapId] [varchar](50) NULL,
	[ChargerGroupName] [varchar](50) NULL,
	[ChargerCount] [int] NULL,
	[ChargerCountStatus] [int] NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ACSGroupConfigs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACSGroupConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupUse] [varchar](50) NULL,
	[GroupName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_ACSRobotGroupModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChargeMissionConfigs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChargeMissionConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChargerGroupName] [varchar](50) NULL,
	[PositionZone] [varchar](50) NULL,
	[ChargeMissionUse] [varchar](50) NULL,
	[ChargeMissionName] [varchar](50) NULL,
	[StartBattery] [float] NULL,
	[SwitchaingBattery] [float] NULL,
	[EndBattery] [float] NULL,
	[ProductValue] [int] NULL,
	[ProductActive] [int] NULL,
	[RobotName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_ChargeMissionConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomMaps]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomMaps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UpdateTime] [datetime] NULL,
	[MapName] [varchar](50) NULL,
	[MapImageData] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorCodeList]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorCodeList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorCode] [int] NOT NULL,
	[ErrorMessage] [varchar](500) NULL,
	[ErrorType] [varchar](50) NULL,
	[Explanation] [varchar](500) NULL,
	[MailSend] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorHistory]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorCreateTime] [datetime] NULL,
	[RobotName] [varchar](50) NULL,
	[ErrorCode] [int] NULL,
	[ErrorDescription] [varchar](5000) NULL,
	[ErrorModule] [varchar](500) NULL,
	[ErrorMessage] [varchar](500) NULL,
	[ErrorType] [varchar](50) NULL,
	[Explanation] [varchar](500) NULL,
	[POSMessage] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FleetPosition]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FleetPosition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Guid] [nvarchar](50) NULL,
	[TypeID] [nvarchar](50) NULL,
	[MapID] [nvarchar](50) NULL,
	[PosX] [float] NULL,
	[PosY] [float] NULL,
	[Orientation] [float] NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FloorMapIDConfigs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FloorMapIDConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FloorIndex] [varchar](500) NULL,
	[FloorName] [varchar](500) NULL,
	[MapID] [varchar](500) NULL,
	[MapImageData] [varchar](max) NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobConfigs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobConfigUse] [varchar](50) NULL,
	[ACSMissionGroup] [varchar](50) NULL,
	[CallName] [varchar](50) NULL,
	[JobMissionName1] [varchar](50) NULL,
	[JobMissionName2] [varchar](50) NULL,
	[JobMissionName3] [varchar](50) NULL,
	[JobMissionName4] [varchar](50) NULL,
	[JobMissionName5] [varchar](50) NULL,
	[ExecuteBattery] [float] NULL,
	[jobCallSignal] [varchar](50) NULL,
	[jobCancelSignal] [varchar](50) NULL,
	[POSjobCallSignal_Reg32] [varchar](50) NULL,
	[POSjobCallSignal_Reg33] [varchar](50) NULL,
	[JobPriority] [int] NULL,
	[ProductValue] [int] NULL,
	[ProductActive] [int] NULL,
	[ElevatorModeValue] [varchar](50) NULL,
	[ElevatorModeActive] [int] NULL,
	[TransportCountActive] [int] NULL,
	[ErrorMissionName] [varchar](50) NULL,
	[MissionName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_JobConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobHistory]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CallName] [varchar](50) NULL,
	[RobotAlias] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[LineName] [varchar](50) NULL,
	[PostName] [varchar](50) NULL,
	[JobState] [varchar](50) NULL,
	[CallTime] [datetime] NULL,
	[JobCreateTime] [datetime] NULL,
	[JobFinishTime] [datetime] NULL,
	[JobElapsedTime] [varchar](50) NULL,
	[MissionNames] [varchar](500) NULL,
	[MissionStates] [varchar](500) NULL,
	[TransportCountValue] [int] NULL,
	[ResultCD] [int] NULL,
 CONSTRAINT [PK_JobHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[CallName] [varchar](500) NULL,
	[PopServerId] [int] NULL,
	[PopCallReturnType] [varchar](50) NULL,
	[PopCallAngle] [int] NULL,
	[PopCallState] [int] NULL,
	[WmsId] [int] NULL,
	[ACSJobGroup] [varchar](50) NULL,
	[JobCreateRobotName] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[JobCreateTime] [datetime] NULL,
	[ExecuteBattery] [float] NULL,
	[JobState] [int] NULL,
	[JobStateText] [varchar](50) NULL,
	[MissionTotalCount] [int] NULL,
	[MissionSentCount] [int] NULL,
	[MissionId1] [varchar](50) NULL,
	[MissionId2] [varchar](50) NULL,
	[MissionId3] [varchar](50) NULL,
	[MissionId4] [varchar](50) NULL,
	[MissionId5] [varchar](50) NULL,
	[TransportCountValue] [int] NULL,
	[JobPriority] [int] NULL,
 CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Missions]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Missions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[ACSMissionGroup] [varchar](50) NULL,
	[CallName] [varchar](500) NULL,
	[CallButtonIndex] [int] NULL,
	[CallButtonName] [varchar](50) NULL,
	[MissionName] [varchar](50) NULL,
	[ErrorMissionName] [varchar](50) NULL,
	[MissionOrderTime] [datetime] NULL,
	[JobCreateRobotName] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[RobotID] [int] NULL,
	[ReturnID] [int] NULL,
	[MissionState] [varchar](50) NULL,
 CONSTRAINT [PK_Missions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PositionAreaConfig]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionAreaConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ACSRobotGroup] [varchar](50) NULL,
	[PositionAreaUse] [varchar](50) NULL,
	[PositionAreaFloorName] [varchar](50) NULL,
	[PositionAreaFloorMapId] [varchar](50) NULL,
	[PositionAreaName] [varchar](50) NULL,
	[PositionAreaX1] [varchar](50) NULL,
	[PositionAreaX2] [varchar](50) NULL,
	[PositionAreaX3] [varchar](50) NULL,
	[PositionAreaX4] [varchar](50) NULL,
	[PositionAreaY1] [varchar](50) NULL,
	[PositionAreaY2] [varchar](50) NULL,
	[PositionAreaY3] [varchar](50) NULL,
	[PositionAreaY4] [varchar](50) NULL,
	[PositionWaitTimeLog] [int] NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_TrafficPositionAreaConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PositionWaitTime]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionWaitTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RobotName] [varchar](50) NULL,
	[RobotAlias] [varchar](50) NULL,
	[PositionName] [varchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[FinishTime] [datetime] NULL,
	[ElapsedTime] [varchar](50) NULL,
	[Mailsend] [int] NULL,
 CONSTRAINT [PK_PositionWaitTime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PositionWaitTimeHistory]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionWaitTimeHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RobotName] [varchar](50) NULL,
	[RobotAlias] [varchar](50) NULL,
	[PositionName] [varchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[FinishTime] [datetime] NULL,
	[ElapsedTime] [varchar](50) NULL,
 CONSTRAINT [PK_PositionWaitTimeHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RobotRegisterSync]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RobotRegisterSync](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegisterSyncUse] [varchar](50) NULL,
	[PositionGroup] [varchar](50) NULL,
	[PositionName] [varchar](50) NULL,
	[ACSRobotGroup] [varchar](50) NULL,
	[RegisterNo] [int] NULL,
	[RegisterValue] [int] NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WaitMissionConfigs]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WaitMissionConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PositionZone] [varchar](50) NULL,
	[WaitMissionUse] [varchar](50) NULL,
	[WaitMissionName] [varchar](50) NULL,
	[EnableBattery] [float] NULL,
	[ProductValue] [int] NULL,
	[ProductActive] [int] NULL,
	[RobotName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_WaitMissionConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ACSChargerCountConfig] ON 

INSERT [dbo].[ACSChargerCountConfig] ([Id], [ChargerCountUse], [RobotGroupName], [FloorName], [FloorMapId], [ChargerGroupName], [ChargerCount], [ChargerCountStatus], [DisplayFlag]) VALUES (1, N'Unuse', NULL, NULL, NULL, N'None', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[ACSChargerCountConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[ACSGroupConfigs] ON 

INSERT [dbo].[ACSGroupConfigs] ([Id], [GroupUse], [GroupName], [DisplayFlag]) VALUES (1, N'Unuse', N'None', 1)
SET IDENTITY_INSERT [dbo].[ACSGroupConfigs] OFF
GO
SET IDENTITY_INSERT [dbo].[ChargeMissionConfigs] ON 

INSERT [dbo].[ChargeMissionConfigs] ([Id], [ChargerGroupName], [PositionZone], [ChargeMissionUse], [ChargeMissionName], [StartBattery], [SwitchaingBattery], [EndBattery], [ProductValue], [ProductActive], [RobotName], [DisplayFlag]) VALUES (1, N'None', N'None', N'Unuse', N'None', 0, 0, 0, 0, 0, N'None', 1)
SET IDENTITY_INSERT [dbo].[ChargeMissionConfigs] OFF
GO
SET IDENTITY_INSERT [dbo].[FleetPosition] ON 

INSERT [dbo].[FleetPosition] ([Id], [Name], [Guid], [TypeID], [MapID], [PosX], [PosY], [Orientation]) VALUES (4, N'1', N'36556d8c-6b28-11ef-bcab-000e8eb6a25b', N'0', N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', 11.067, 21.072, 0)
INSERT [dbo].[FleetPosition] ([Id], [Name], [Guid], [TypeID], [MapID], [PosX], [PosY], [Orientation]) VALUES (5, N'staging', N'5a601b88-6b3f-11ef-9355-000e8eb6a25b', N'42', N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', 9.128, 19.745, 89)
INSERT [dbo].[FleetPosition] ([Id], [Name], [Guid], [TypeID], [MapID], [PosX], [PosY], [Orientation]) VALUES (6, N'로봇 위치', N'ffe14275-6b3f-11ef-9355-000e8eb6a25b', N'0', N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', 9.34, 15.725, 86)
SET IDENTITY_INSERT [dbo].[FleetPosition] OFF
GO
SET IDENTITY_INSERT [dbo].[FloorMapIDConfigs] ON 

INSERT [dbo].[FloorMapIDConfigs] ([Id], [FloorIndex], [FloorName], [MapID], [MapImageData], [DisplayFlag]) VALUES (1, N'None', N'B1', N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', N'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', 1)
SET IDENTITY_INSERT [dbo].[FloorMapIDConfigs] OFF
GO
SET IDENTITY_INSERT [dbo].[JobConfigs] ON 

INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [JobPriority], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [DisplayFlag]) VALUES (1, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, 0, N'None', 0, 0, N'', N'', 1)
SET IDENTITY_INSERT [dbo].[JobConfigs] OFF
GO
SET IDENTITY_INSERT [dbo].[PositionAreaConfig] ON 

INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [PositionWaitTimeLog], [DisplayFlag]) VALUES (1, N'None', N'Unuse', N'None', N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0, 1)
SET IDENTITY_INSERT [dbo].[PositionAreaConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[RobotRegisterSync] ON 

INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (1, N'Unuse', N'None', N'None', N'None', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[RobotRegisterSync] OFF
GO
SET IDENTITY_INSERT [dbo].[Robots] ON 

INSERT [dbo].[Robots] ([Id], [JobId], [ACSRobotGroup], [ACSRobotActive], [Fleet_State], [Fleet_State_Text], [RobotID], [RobotIp], [RobotName], [StateID], [StateText], [MissionText], [MissionQueueID], [MapID], [BatteryPercent], [DistanceToNextTarget], [Position_Orientation], [Position_X], [Position_Y], [Product], [Door], [PositionZoneName], [ErrorsJson], [RobotModel], [RobotAlias]) VALUES (1, 0, N'', 0, 14, N'independent mission', 3, N'192.168.8.63', N'MiR_206000891', N'5', N'Executing', N'Moving to ''로봇 위치''. Waiting to be assigned  a necessary resource by MiR Fleet.', 23, N'f02944f7-6b27-11ef-bcab-000e8eb6a25b', 45.32, 2.47, -83.24, 9.55, 18.16, N'', N'', N'', N'[]', N'MiR250', N'')
INSERT [dbo].[Robots] ([Id], [JobId], [ACSRobotGroup], [ACSRobotActive], [Fleet_State], [Fleet_State_Text], [RobotID], [RobotIp], [RobotName], [StateID], [StateText], [MissionText], [MissionQueueID], [MapID], [BatteryPercent], [DistanceToNextTarget], [Position_Orientation], [Position_X], [Position_Y], [Product], [Door], [PositionZoneName], [ErrorsJson], [RobotModel], [RobotAlias]) VALUES (2, 0, N'', 0, 0, NULL, 2, N'192.168.1.11', N'', N'0', N'', N'', 0, N'', 0, 0, 0, 0, 0, N'', N'', NULL, N'', N'', N'')
INSERT [dbo].[Robots] ([Id], [JobId], [ACSRobotGroup], [ACSRobotActive], [Fleet_State], [Fleet_State_Text], [RobotID], [RobotIp], [RobotName], [StateID], [StateText], [MissionText], [MissionQueueID], [MapID], [BatteryPercent], [DistanceToNextTarget], [Position_Orientation], [Position_X], [Position_Y], [Product], [Door], [PositionZoneName], [ErrorsJson], [RobotModel], [RobotAlias]) VALUES (3, 0, N'', 0, 0, NULL, 3, N'192.168.1.20', N'', N'0', N'', N'', 0, N'', 0, 0, 0, 0, 0, N'', N'', NULL, N'', N'', N'')
SET IDENTITY_INSERT [dbo].[Robots] OFF
GO
SET IDENTITY_INSERT [dbo].[WaitMissionConfigs] ON 

INSERT [dbo].[WaitMissionConfigs] ([Id], [PositionZone], [WaitMissionUse], [WaitMissionName], [EnableBattery], [ProductValue], [ProductActive], [RobotName], [DisplayFlag]) VALUES (1, N'None', N'Unuse', N'None', 0, 0, 0, N'None', 1)
SET IDENTITY_INSERT [dbo].[WaitMissionConfigs] OFF
GO
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr1]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetErrorHistoryAggr1]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--date 테이블 생성
	create table #d_date (d_type datetime)
	declare @var datetime = @searchDate1
	while(@var < @searchDate2)
	begin
		insert into #d_date values(@var)
		set @var = DATEADD(DAY, +1, @var)
	end


	-- 날짜별 에러발생건수
	SELECT FORMAT(A.d_type, 'yyyy-MM-dd') [DATE], COUNT(B.에러발생일자) [TOTAL], COUNT(IIF(B.SHIFT=1,1,NULL)) [OFFICE], COUNT(IIF(B.SHIFT=0,1,NULL)) [NIGHT]
	FROM
	(
		SELECT d_type FROM #d_date
	) A
	LEFT JOIN
	(
		SELECT 
			FORMAT(ErrorCreateTime, 'yyyy-MM-dd') [에러발생일자],
			IIF(FORMAT(ErrorCreateTime, 'HH:mm:ss') BETWEEN '08:30:00' AND '17:30:00', 1, 0) [SHIFT]
		FROM [ErrorHistory] WITH(NOLOCK)
		WHERE 
			(ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2)
			AND ErrorCode >= 0
			AND (
				(@robotNamesEmpty = 1)
				OR
				(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
	) B
	ON A.d_type = B.에러발생일자
	GROUP BY A.d_type
	ORDER BY A.d_type


	--날짜별 반송량
	SELECT FORMAT(A.d_type, 'yyyy-MM-dd') [날짜], COUNT(B.JOB완료일자) [반송량]
	FROM
	(
		SELECT d_type FROM #d_date
	) A
	LEFT JOIN
	(
		SELECT 
			FORMAT(JobFinishTime, 'yyyy-MM-dd') [JOB완료일자]
		FROM [JobHistory] WITH(NOLOCK)
		WHERE 
			(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
			AND ResultCD = 11 
			AND LEN(RobotName) > 0
			AND 
			(
				(@robotNamesEmpty = 1)
				OR
				(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
	) B
	ON A.d_type = B.JOB완료일자
	GROUP BY A.d_type
	ORDER BY A.d_type

END
GO
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spGetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--date 테이블 생성
	create table #d_date (d_type datetime)
	declare @var datetime = @searchDate1
	while(@var < @searchDate2)
	begin
		insert into #d_date values(@var)
		set @var = DATEADD(DAY, +1, @var)
	end


	-- 날짜별 에러발생건수
	SELECT FORMAT(A.d_type, 'yyyy-MM-dd') [DATE], COUNT(B.에러발생일자) [TOTAL], COUNT(IIF(B.SHIFT=1,1,NULL)) [OFFICE], COUNT(IIF(B.SHIFT=0,1,NULL)) [NIGHT]
	FROM
	(
		SELECT d_type FROM #d_date
	) A
	LEFT JOIN
	(
		SELECT 
			FORMAT(ErrorCreateTime, 'yyyy-MM-dd') [에러발생일자],
			IIF(FORMAT(ErrorCreateTime, 'HH:mm:ss') BETWEEN '08:30:00' AND '17:30:00', 1, 0) [SHIFT]
		FROM [ErrorHistory] WITH(NOLOCK)
		WHERE 
			(ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2)
			AND ErrorCode >= 0
			AND (
				(@robotNamesEmpty = 1)
				OR
				(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
	) B
	ON A.d_type = B.에러발생일자
	GROUP BY A.d_type
	ORDER BY A.d_type


	--날짜별 반송량

	--SELECT 
	--     --FORMAT(JobFinishTime, 'yyyy-MM-dd')[날짜]
	--	--, TransportCountValue [반송량]
	--	--,Sum(case when TransportCountValue = 0 then TransportCountValue else 0 end)[Test]
	--	,Sum(case when TransportCountValue = 0 then TransportCountValue else 0 end)[Test]
	--
	--FROM [JobHistory] WITH(NOLOCK)
	--WHERE 
	--	(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
	--	AND ResultCD = 11 
	--	AND LEN(RobotName) > 0
	--	AND 
	--	(
	--		(@robotNamesEmpty = 1)
	--		OR
	--		(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
	--	)
	--GROUP BY All FORMAT(JobFinishTime, 'yyyy-MM-dd')


	--SELECT FORMAT(A.d_type, 'yyyy-MM-dd') [날짜], COUNT(B.JOB완료일자) [반송량]
	--SELECT FORMAT(A.d_type, 'yyyy-MM-dd') [날짜], SUM(B.JOB완료일자) [반송량]
	--SELECT FORMAT(A.d_type,'yyyy-MM-dd') [날짜]


	SELECT FORMAT(A.d_type,'yyyy-MM-dd') [날짜], 
		(CASE
			WHEN TotalTransportCnt IS NULL THEN 0 
			WHEN TotalTransportCnt = '' THEN 0 
			ELSE TotalTransportCnt 
		END)AS[반송량]
	FROM
	(
		SELECT d_type FROM #d_date
	) A
	LEFT JOIN
	(
		SELECT DATES AS JOB완료일자, 
		SUM(TransportCountValue) AS TotalTransportCnt
		FROM(
			SELECT FORMAT(JobFinishTime, 'yyyy-MM-dd') AS DATES, 
			TransportCountValue
			FROM JobHistory WITH(NOLOCK)
			WHERE ResultCD ='11' 
			AND JobFinishTime BETWEEN @searchDate1
			AND @searchDate2
			AND LEN(RobotName) > 0
			AND 
			(
				(@robotNamesEmpty = 1)
				OR
				(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
			GROUP BY JobFinishTime, TransportCountValue) C
		GROUP BY DATES
	) B
	ON A.d_type = B.JOB완료일자
	GROUP BY A.d_type, TotalTransportCnt
	ORDER BY A.d_type








	--SELECT FORMAT(A.d_type,'yyyy-MM-dd') [날짜] 
	--FROM
	--(
	--	SELECT d_type FROM #d_date
	--) A
	--LEFT JOIN
	--(
		--SELECT 
		--	FORMAT(JobFinishTime, 'yyyy-MM-dd') [JOB완료일자]
		--	,SUM(TransportCountValue) [반송량]
		--FROM [JobHistory] WITH(NOLOCK)
		--WHERE 
		--	(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		--	AND ResultCD = 11 
		--	AND LEN(RobotName) > 0
		--	AND 
		--	(
		--		(@robotNamesEmpty = 1)
		--		OR
		--		(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		--	)
	--) B
	--ON A.d_type = B.JOB완료일자
	--GROUP BY A.d_type
	--ORDER BY A.d_type




END
GO
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr2]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetErrorHistoryAggr2]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--date 테이블 생성
	create table #d_date (d_type datetime)
	declare @var datetime = @searchDate1
	while(@var < @searchDate2)
	begin
		insert into #d_date values(@var)
		set @var = DATEADD(DAY, +1, @var)
	end


	--에러별 발생건수
	SELECT COUNT(*) [에러개수], ErrorCode, ErrorMessage [ErrorText]
	FROM [ErrorHistory] WITH(NOLOCK)
	WHERE 
		(ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2)
		AND ErrorCode >= 0
		AND LEN(RobotName) > 0
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
	GROUP BY ErrorCode, ErrorMessage
	ORDER BY [에러개수] DESC, ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr1]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetJobHistoryAggr1]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--로봇별 반송량&반송시간
	SELECT 
	--  ROW_NUMBER() OVER(ORDER BY RobotName ASC) [No], 
		RobotName, COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11
		AND LEN(RobotName) > 0
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
	GROUP BY RobotName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr1_JobHistoryTransportValueColumnAdd]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spGetJobHistoryAggr1_JobHistoryTransportValueColumnAdd]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--로봇별 반송량&반송시간
	SELECT 
	--  ROW_NUMBER() OVER(ORDER BY RobotName ASC) [No], 
	--RobotName, COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
		RobotName, SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11
		AND LEN(RobotName) > 0
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
	GROUP BY RobotName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr2]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetJobHistoryAggr2]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max),	--로봇목록(콤마구분)
	@lineNames   varchar(max)	--출발지목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)
	DECLARE @lineNamesEmpty  INT = IIF(@lineNames  IS NULL OR LEN(@lineNames)  = 0, 1, 0)


	--출발지별 반송량&반송시간
	SELECT LineName [출발지], COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
		AND LEN(LineName) > 0 
		AND 
		(
			(@lineNamesEmpty = 1)
			OR
			(@lineNamesEmpty = 0 AND LineName IN (SELECT value FROM string_split(@lineNames, ',')))
		)
	GROUP BY LineName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr2_JobHistoryTransportValueColumnAdd]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spGetJobHistoryAggr2_JobHistoryTransportValueColumnAdd]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max),	--로봇목록(콤마구분)
	@lineNames   varchar(max)	--출발지목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)
	DECLARE @lineNamesEmpty  INT = IIF(@lineNames  IS NULL OR LEN(@lineNames)  = 0, 1, 0)


	--출발지별 반송량&반송시간
	SELECT LineName [출발지], SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	--SELECT LineName [출발지], COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
		AND LEN(LineName) > 0 
		AND 
		(
			(@lineNamesEmpty = 1)
			OR
			(@lineNamesEmpty = 0 AND LineName IN (SELECT value FROM string_split(@lineNames, ',')))
		)
	GROUP BY LineName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr3]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetJobHistoryAggr3]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max),	--로봇목록(콤마구분)
	@postNames   varchar(max)	--목적지목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)
	DECLARE @postNamesEmpty  INT = IIF(@postNames  IS NULL OR LEN(@postNames)  = 0, 1, 0)


	--목적지별 반송량&반송시간
	SELECT PostName [목적지], COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
		AND LEN(PostName) > 0 
		AND 
		(
			(@postNamesEmpty = 1)
			OR
			(@postNamesEmpty = 0 AND PostName IN (SELECT value FROM string_split(@postNames, ',')))
		)
	GROUP BY PostName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr3_JobHistoryTransportValueColumnAdd]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spGetJobHistoryAggr3_JobHistoryTransportValueColumnAdd]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max),	--로봇목록(콤마구분)
	@postNames   varchar(max)	--목적지목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)
	DECLARE @postNamesEmpty  INT = IIF(@postNames  IS NULL OR LEN(@postNames)  = 0, 1, 0)


	--목적지별 반송량&반송시간
	SELECT PostName [목적지], SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	--SELECT PostName [목적지], COUNT(*) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
		AND LEN(PostName) > 0 
		AND 
		(
			(@postNamesEmpty = 1)
			OR
			(@postNamesEmpty = 0 AND PostName IN (SELECT value FROM string_split(@postNames, ',')))
		)
	GROUP BY PostName

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr4]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetJobHistoryAggr4]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--시간별 반송량
	SELECT DATEPART(HOUR, JobFinishTime) [Hour], COUNT(*) [반송량]
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
	GROUP BY ALL DATEPART(HOUR, JobFinishTime)
	ORDER BY DATEPART(HOUR, JobFinishTime)

END
GO
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr4_JobHistoryTransportValueColumnAdd]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spGetJobHistoryAggr4_JobHistoryTransportValueColumnAdd]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames  varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotNamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--시간별 반송량
	SELECT DATEPART(HOUR, JobFinishTime) [Hour], SUM(TransportCountValue) [반송량]
	--SELECT DATEPART(HOUR, JobFinishTime) [Hour], COUNT(*) [반송량]
	 
	FROM [JobHistory] WITH(NOLOCK)
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11 
		AND LEN(RobotName) > 0 
		AND 
		(
			(@robotNamesEmpty = 1)
			OR
			(@robotNamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)
	GROUP BY ALL DATEPART(HOUR, JobFinishTime)
	ORDER BY DATEPART(HOUR, JobFinishTime)

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary1_총반송량_평균반송시간]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary1_총반송량_평균반송시간]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotnamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--총반송량, 반송시간(최대/최소/평균/편차)
	SELECT 
		COUNT(*) [총반송량],
		MAX(datediff(SECOND,JobCreateTime,JobFinishTime)) [반송시간MAX],
		MIN(datediff(SECOND,JobCreateTime,JobFinishTime)) [반송시간MIN],
		AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [반송시간AVG],
		STDEVP(datediff(SECOND,JobCreateTime,JobFinishTime)) [반송시간STDEVP]
	FROM [JobHistory]
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11
		AND LEN(RobotName) > 0
		AND 
		(
			(@robotnamesEmpty = 1 AND LEN(RobotName) > 0)
			OR
			(@robotnamesEmpty = 0 AND LEN(RobotName) > 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary2_시간평균반송량]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary2_시간평균반송량]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotnamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--시간평균반송량
	SELECT 
		COUNT(*) [시간갯수], 
		SUM(A.[반송량]) [총반송량], 
		ROUND(AVG(Cast(A.[반송량] AS Float)),2) [시간평균반송량]
	FROM
	(
		--시간대별 반송량
		SELECT 
			DATEPART(HOUR, JobFinishTime) [Hour], 
			COUNT(*) [반송량]
		FROM [JobHistory]
		WHERE 
			(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
			AND ResultCD = 11 
			AND LEN(RobotName) > 0 
			AND
			(
				(@robotnamesEmpty = 1 AND LEN(RobotName) > 0)
				OR
				(@robotnamesEmpty = 0 AND LEN(RobotName) > 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
		GROUP BY ALL DATEPART(HOUR, JobFinishTime)
	) A

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary3_월평균반송량]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary3_월평균반송량]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotnamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--월평균반송량
	SELECT 
		COUNT(*) [월수], 
		SUM(A.[반송량]) [총반송량], 
		AVG(Cast(A.[반송량] AS Float)) [월평균반송량]
	FROM
	(
		--월별반송량
		SELECT 
			DATEPART(MONTH, JobFinishTime) [Month], 
			COUNT(*) [반송량]
		FROM [JobHistory]
		WHERE 
			(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
			AND ResultCD = 11 
			AND
			(
				(@robotnamesEmpty = 1)
				OR
				(@robotnamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
		GROUP BY ALL DATEPART(MONTH, JobFinishTime)
	) A

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary4_총에러수_평균반송시간]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary4_총에러수_평균반송시간]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotnamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--총에러수, 반송시간(최대/최소/평균/편차)
	SELECT 
		COUNT(*) [총에러수]
	FROM [ErrorHistory]
	WHERE 
		(ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2)
		AND LEN(RobotName) > 0
		AND
		(
			(@robotnamesEmpty = 1)
			OR
			(@robotnamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
		)

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary5_시간평균에러수]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary5_시간평균에러수]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime,		--종료날짜
	@robotNames varchar(max)	--로봇목록(콤마구분)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @robotnamesEmpty INT = IIF(@robotNames IS NULL OR LEN(@robotNames) = 0, 1, 0)


	--시간평균에러수
	SELECT 
		COUNT(*) [시간갯수], 
		SUM(A.[에러수]) [총에러수], 
		ROUND(AVG(Cast(A.[에러수] AS Float)),2) [시간평균에러수]
	FROM
	(
		--시간대별 에러수
		SELECT 
			DATEPART(HOUR, ErrorCreateTime) [Hour], 
			COUNT(*) [에러수]
		FROM [ErrorHistory]
		WHERE 
			(ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2)
			AND LEN(RobotName) > 0
			AND
			(
				(@robotnamesEmpty = 1)
				OR
				(@robotnamesEmpty = 0 AND RobotName IN (SELECT value FROM string_split(@robotNames, ',')))
			)
		GROUP BY ALL DATEPART(HOUR, ErrorCreateTime)
	) A

END
GO
/****** Object:  StoredProcedure [dbo].[spGetSummary6_월별반송량]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSummary6_월별반송량]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime		--종료날짜
AS
BEGIN
	SET NOCOUNT ON;


	--월별 반송량
	SELECT concat(DATEPART(MONTH, JobFinishTime), '월') [Month], COUNT(*) [반송량]
	FROM [JobHistory]
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11  
	GROUP BY DATEPART(MONTH, JobFinishTime)
	ORDER BY DATEPART(MONTH, JobFinishTime)

END
GO
/****** Object:  StoredProcedure [dbo].[spSub1]    Script Date: 2024-09-05 오후 2:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spSub1]
	@searchDate1 datetime,		--시작날짜
	@searchDate2 datetime		--종료날짜
AS
BEGIN
	SET NOCOUNT ON;

	----month 테이블 생성
	--drop table #months
	--create table #months (dates datetime)
	--declare @var datetime = @searchDate1
	--while @var < dateadd(MONTH, +1, @searchDate2)
	--begin
	--	insert into #months Values(@var)
	--	set @var = Dateadd(MONTH, +1, @var)
	--end

	----date 테이블 생성
	--create table #d_date (d_type datetime)
	--declare @var datetime = @searchDate1
	--while(@var < @searchDate2)
	--begin
	--	insert into #d_date values(@var)
	--	set @var = DATEADD(DAY, +1, @var)
	--end

END
GO
USE [master]
GO
ALTER DATABASE [RobotAPI] SET  READ_WRITE 
GO
