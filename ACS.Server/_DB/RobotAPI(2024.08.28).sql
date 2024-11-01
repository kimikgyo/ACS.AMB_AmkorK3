USE [master]
GO
/****** Object:  Database [RobotAPI]    Script Date: 2024-08-28 오전 11:56:57 ******/
CREATE DATABASE [RobotAPI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RobotApiTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RobotApiTest.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RobotApiTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RobotApiTest_log.ldf' , SIZE = 466944KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE [RobotAPI] SET QUERY_STORE = OFF
GO
USE [RobotAPI]
GO
/****** Object:  Table [dbo].[Robots]    Script Date: 2024-08-28 오전 11:56:57 ******/
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
/****** Object:  View [dbo].[RobotInfo]    Script Date: 2024-08-28 오전 11:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[RobotInfo]
as 
select JobId,RobotID,RobotName [Name],ACSRobotGroup [Group],ACSRobotActive [Active],Fleet_State_Text [FleetState],StateText [RobotState],Position_X [X],Position_Y [Y],Product 
from Robots
GO
/****** Object:  Table [dbo].[ACSChargerCountConfig]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  Table [dbo].[ACSGroupConfigs]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChargeMissionConfigs]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorCodeList]    Script Date: 2024-08-28 오전 11:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorCodeList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorCode] [int] NOT NULL,
	[ErrorMessage] [varchar](500) NULL,
	[ErrorType] [varchar](50) NULL,
	[Explanation] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorHistory]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  Table [dbo].[FloorMapIDConfigs]    Script Date: 2024-08-28 오전 11:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FloorMapIDConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FloorName] [varchar](500) NULL,
	[MapID] [varchar](500) NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobConfigs]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	[ProductValue] [int] NULL,
	[ProductActive] [int] NULL,
	[ElevatorModeValue] [varchar](50) NULL,
	[ElevatorModeActive] [int] NULL,
	[TransportCountActive] [int] NULL,
	[ErrorMissionName] [varchar](50) NULL,
	[MissionName] [varchar](50) NULL,
	[JobPriority] [int] NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_JobConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobHistory]    Script Date: 2024-08-28 오전 11:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CallName] [varchar](50) NULL,
	[LineName] [varchar](50) NULL,
	[PostName] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Missions]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PositionAreaConfig]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_TrafficPositionAreaConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RobotRegisterSync]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  Table [dbo].[WaitMissionConfigs]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ACSChargerCountConfig] ON 

INSERT [dbo].[ACSChargerCountConfig] ([Id], [ChargerCountUse], [RobotGroupName], [FloorName], [FloorMapId], [ChargerGroupName], [ChargerCount], [ChargerCountStatus], [DisplayFlag]) VALUES (1, N'Unuse', NULL, NULL, NULL, N'None', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[ACSChargerCountConfig] OFF
SET IDENTITY_INSERT [dbo].[ACSGroupConfigs] ON 

INSERT [dbo].[ACSGroupConfigs] ([Id], [GroupUse], [GroupName], [DisplayFlag]) VALUES (1, N'Unuse', N'None', 1)
INSERT [dbo].[ACSGroupConfigs] ([Id], [GroupUse], [GroupName], [DisplayFlag]) VALUES (2, N'Unuse', N'None', 0)
SET IDENTITY_INSERT [dbo].[ACSGroupConfigs] OFF
SET IDENTITY_INSERT [dbo].[ChargeMissionConfigs] ON 

INSERT [dbo].[ChargeMissionConfigs] ([Id], [ChargerGroupName], [PositionZone], [ChargeMissionUse], [ChargeMissionName], [StartBattery], [SwitchaingBattery], [EndBattery], [ProductValue], [ProductActive], [RobotName], [DisplayFlag]) VALUES (1, N'None', N'None', N'Unuse', N'None', 0, 0, 0, 0, 1, N'None', 1)
SET IDENTITY_INSERT [dbo].[ChargeMissionConfigs] OFF
SET IDENTITY_INSERT [dbo].[ErrorCodeList] ON 

INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (1, 101, N'CPU load is too HIGH - Above 90% for more than 3 minutes.', N'Heavy Alarm', N'로봇 소프트웨어에 문제가 발생하여 비정상적인 양의 처리 능력이 필요합니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (2, 201, N'Free Memory is VERY low', N'Heavy Alarm', N'로봇 소프트웨어에 문제가 발생했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (3, 301, N'No ethernet adapter found', N'Heavy Alarm', N'로봇 컴퓨터에 문제가 발생했습니다')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (4, 302, N'Ethernet is not connected.', N'Heavy Alarm', N'로봇 컴퓨터와 라우터 사이의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (5, 401, N'Very low free space on hard drive.', N'Heavy Alarm', N'하드 드라이브 메모리가 거의 꽉 찼습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (6, 501, N'Battery MIR 100/200 :Battery is TOO low.', N'Heavy Alarm', N'로봇의 전력이 5%만 남아 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (7, 503, N'Battery MIR 100/200: Missing charging status message.', N'Heavy Alarm', N'배터리 관리 시스템의 CAN 버스 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (8, 504, N'Battery MIR 100/200: Unable to detect hardware configuration.', N'Heavy Alarm', N'로봇 소프트웨어에 오류가 있어 하드웨어 구성을 확인할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (9, 601, N'Teensy: Gyroscope data timed out - Last Gyroscope message received %(seconds)d seconds ago', N'Heavy Alarm', N'MiR 보드가 자이로스코프로부터 데이터를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (10, 602, N'Teensy: Gyroscope data is jumping ALOT!', N'Heavy Alarm', N'MiR 보드가 자이로스코프로부터 데이터를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (11, 650, N'Unable to find Light Controller MCU.', N'Heavy Alarm', N'로봇 컴퓨터에서 MiR 보드로의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (12, 650, N'Unable to get version of Light Controller MCU.', N'Heavy Alarm', N'로봇 컴퓨터에서 MiR 보드로의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (13, 650, N'Connecting to Light controller MCU failed.', N'Heavy Alarm', N'로봇 컴퓨터에서 MiR 보드로의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (14, 650, N'No firmware available for this Light controller MCU.', N'Heavy Alarm', N'MiR 보드에 소프트웨어가 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (15, 650, N'Upgrading firmware failed.', N'Heavy Alarm', N'이 오류는 소프트웨어 업데이트 중에 MiR 보드를 업데이트하지 못한 경우에 발생할 수 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (16, 650, N'Multiple teensies found -this is not supported!', N'Heavy Alarm', N'이 오류는 다른 컨트롤러 보드를 로봇 컴퓨터에 연결하려는 경우에만 발생합니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (17, 651, N'Teensy: Programming MiR controller MCU…', N'Heavy Alarm', N'이 오류는 소프트웨어를 MiR 보드에 업로드할 때 소프트웨어 업데이트 중에 발생할 수 있습니다..')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (18, 661, N'Battery firmware does not match recommended version: Current installed: x.x. Recommen ded: 3.2.', N'Heavy Alarm', N'배터리 펌웨어가 권장 버전이 아닙니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (19, 671, N'Battery firmware does not match recommended version: Current installed: x.x. Recommen ded: 3.2.', N'Heavy Alarm', N'배터리 펌웨어가 권장 버전이 아닙니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (20, 663, N'Firmware upgrade of the battery failed in bootloader state. Reboot the robot to retry the upgrade.', N'Heavy Alarm', N'배터리 펌웨어를 업데이트하는 동안 배터리가 부트로더 상태에서 실패했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (21, 665, N'Firmware upgrade of the battery failed in uploading parameters state. Reboot the robot to retry the upgrade.', N'Heavy Alarm', N'배터리 펌웨어를 업데이트하는 동안 배터리가 업로드 매개 변수 상태에서 실패했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (22, 666, N'Skipped firmware upgrade since the battery level is too low. Charge the robot and reboot to retry the upgrade', N'Heavy Alarm', N'배터리 비율이 50% 미만이므로 배터리 펌웨어 업데이트에 실패했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (23, 667, N'Firmware upgrade of the battery failed in file corrupted state. Reboot the robot to retry the upgrade.', N'Heavy Alarm', N'배터리 펌웨어 업데이트를 시도하는 동안 배터리가 손상된 상태로 실패했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (24, 668, N'Skipped firmware upgrade due to excessive current drawn. Please disconnect peripherals, make sure the robot is not charging and reboot.', N'Heavy Alarm', N'배터리가 너무 많은 전류를 소모하기 때문에 배터리 펌웨어 업데이트에 실패했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (25, 701, N'MIR100/200 Motor Controller: Motor power usage above limit!', N'Heavy Alarm', N'모터가 예상보다 많은 전력을 사용하고 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (26, 710, N'MIR100/200 Motor Controller: Initialization failed!', N'Heavy Alarm', N'모터 컨트롤러와 로봇 컴퓨터의 연결이 잘못되었습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (27, 711, N'MIR100/200 Motor Controller: Unable to connect to motor controller!', N'Heavy Alarm', N'모터 컨트롤러와 로봇 컴퓨터의 연결이 잘못되었습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (28, 712, N'MIR100/200 Motor Controller: Lost connection to motor controller!', N'Heavy Alarm', N'모터 컨트롤러와 로봇 컴퓨터의 연결이 잘못되었습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (29, 801, N'SICK S300: Lost connection to scanner.', N'Heavy Alarm', N'로봇 컴퓨터가 레이저 스캐너에서 스캐너 정보를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (30, 802, N'SICK S300: No data for >15 sec.', N'Heavy Alarm', N'로봇 컴퓨터가 레이저 스캐너에서 스캐너 정보를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (31, 803, N'SICK S300: Got xx failed transmissions in the last xx seconds.', N'Heavy Alarm', N'로봇 컴퓨터가 레이저 스캐너에서 스캐너 정보를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (32, 901, N'Camera (OpenNI): Waiting for connection is taking longer than expected!', N'Heavy Alarm', N'적어도 하나의 카메라와 로봇 컴퓨터 사이의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (33, 902, N'Camera (OpenNI): No Data for xx seconds.', N'Heavy Alarm', N'적어도 하나의 카메라와 로봇 컴퓨터 사이의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (34, 910, N'Camera (realsense <R200/D435>): Lost connection to camera. No data for xx seconds.', N'Heavy Alarm', N'로봇 컴퓨터가 3D 카메라 중 하나와 통신할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (35, 911, N'Camera (realsense <R200/D435>): Unable to connect to camera, however it seems to be available.', N'Heavy Alarm', N'이 오류는 새 카메라나 로봇 컴퓨터를 설치한 후에 발생할 수 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (36, 911, N'Camera (realsense <R200/D435>): Unable to detect camera with serial: xxxx. yyyy. zzzz, however xx other cameras are connected with serial: xxxx. yyyy. zzzz.', N'Heavy Alarm', N'두 카메라 모두 로봇 컴퓨터가 검색하는 것과 다른 일련 번호를 가지고 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (37, 911, N'Camera (realsense <R200/D435>): Unable to detect camera with serial: xxxx. yyyy. zzzz, however 1 other camera are connected with serial: xxxx.', N'Heavy Alarm', N'카메라 중 하나에 로봇 컴퓨터가 검색하는 번호와 다른 일련 번호가 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (38, 911, N'Camera (realsense <R200/D435>): Unable to detect camera with serial: xxxx. yyyy. zzzz and no other cameras available.', N'Heavy Alarm', N'로봇 컴퓨터가 3D 카메라 중 하나와 통신할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (39, 912, N'Camera (realsense <R200/D435>): The serial number is not configured for this camera!', N'Heavy Alarm', N'로봇 컴퓨터에는 검색할 일련 번호가 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (40, 1001, N'SICK PLC (MIR100/200): Receiving SICK Safety PLC data timed out.', N'Heavy Alarm', N'안전 PLC와 로봇 컴퓨터 간의 통신이 잘못되었습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (41, 1004, N'Unsupported SICK program version.', N'Heavy Alarm', N'안전 PLC에서 SICK 프로그램을 수정했습니다. 안전 시스템에서 오류가 발생하는 경우 사용자 정의 수정으로 프로그램 문제를 해결할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (42, 1005, N'Gear ratio configuration mismatch! Sick is configured with: xxx and Robot software is configured with: yyy.', N'Heavy Alarm', N'안전 PLC 또는 로봇 컴퓨터가 잘못된 기어비 구성을 사용하고 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (43, 5101, N'Unable to connect to camera.', N'Heavy Alarm', N'로봇 컴퓨터가 3D 카메라 중 하나와 통신할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (44, 5102, N'Missing data from camera: Time since last frame [s].', N'Heavy Alarm', N'로봇 컴퓨터가 3D 카메라 중 하나와 통신할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (45, 5103, N'Lost connection to camera: Time since last frame [s].', N'Heavy Alarm', N'로봇 컴퓨터가 3D 카메라 중 하나와 통신할 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (46, 9000, N'Missing - Last Message: Firmware upgrade of the battery failed in application state. Reboot the robot to retry the upgrade.', N'Heavy Alarm', N'배터리와 MiR 보드 간의 연결이 끊어져 배터리 펌웨어를 업데이트하지 못했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (47, 9000, N'Diagnostics monitoring: Stale error (Expected diagnostics messages missing).', N'Heavy Alarm', N'로봇 컴퓨터는 제 시간에 도착하지 않은 진단 메시지를 기다리고 있습니다. 오류는 누락된 진단 메시지를 설명해야 합니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (48, 9030, N'Missing - Last Message: Camera is OK', N'Heavy Alarm', N'적어도 하나의 카메라와 로봇 컴퓨터 사이의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (49, 9030, N'Missing - Last Message: {"message": "Lost connection to camera. No data for <duration> seconds."', N'Heavy Alarm', N'적어도 하나의 카메라와 로봇 컴퓨터 사이의 연결에 결함이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (50, 10101, N'<action name> aborted - <current action name> failed to start (<status>).', N'', N'미션의 작업을 완료할 수 없습니다. 실패 이유는 오류 코드 텍스트에 설명되어 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (51, 10110, N'Goal position ''<position name>'' is in forbidden area.', N'', N'로봇을 이동시키려는 위치가 금지 구역 안에 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (52, 10111, N'Goal position ''<position name>'' is in obstacle.', N'', N'로봇을 이동하려는 위치에 장애물이 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (53, 10120, N'Failed to reach goal position ''<position name>''.', N'', N'로봇이 이동하고자 하는 위치가 장애물에 의해 막혀 있습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (54, 10198, N'Invalid action parameters. Update mission list before running again.', N'', N'미션에서 설정한 매개변수 중 하나가 잘못되었습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (55, 10201, N'E_LOCALIZATION_FAILED.', N'', N'로봇이 스스로를 올바르게 위치시킬 수 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (56, 10202, N'E_LOCALIZATION_FAILED_ NO_SCANNER_DATA.', N'Heavy Alarm', N'로봇 컴퓨터가 레이저 스캐너에서 스캐너 정보를 수신하지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (57, 10401, N'Robot detected skid condition!', N'Heavy Alarm', N'로봇의 바퀴는 돌아가지만 로봇은 움직이지 않습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (58, 10702, N'motor stall detected!', N'Heavy Alarm', N'3페이지의 오류 오류 코드 및 해결 방법을 참조하십시오.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (59, 10701, N'Left motor encoder signal missing!', N'Heavy Alarm', N'모터 컨트롤러에 왼쪽 모터 인코더에 대한 연결이 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (60, 10711, N'Right motor encoder signal missing!', N'Heavy Alarm', N'모터 컨트롤러에 오른쪽 모터 인코더에 대한 연결이 없습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (61, 11001, N'The local planner failed repeatedly to follow new global plans!', N'', N'로봇이 로컬 경로를 따를 수 없어 새로운 경로를 생성했습니다.')
INSERT [dbo].[ErrorCodeList] ([Id], [ErrorCode], [ErrorMessage], [ErrorType], [Explanation]) VALUES (62, 13000, N'Precision docking disconnected: There is no communication with the Raspberry Pi of the precision docking.', N'Heavy Alarm', N'로봇 컴퓨터가 정밀 도킹 잠금 장치용 컨트롤러에 대한 연결이 끊어졌습니다.')
SET IDENTITY_INSERT [dbo].[ErrorCodeList] OFF
SET IDENTITY_INSERT [dbo].[FloorMapIDConfigs] ON 

INSERT [dbo].[FloorMapIDConfigs] ([Id], [FloorName], [MapID], [DisplayFlag]) VALUES (1, N'Test', N'24b3bfa0-440e-11ef-a36c-94c6911e7a70', 1)
SET IDENTITY_INSERT [dbo].[FloorMapIDConfigs] OFF
SET IDENTITY_INSERT [dbo].[JobConfigs] ON 

INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (1, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 1)
INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (2, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 0)
INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (3, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 0)
INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (4, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 0)
INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (5, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 0)
INSERT [dbo].[JobConfigs] ([Id], [JobConfigUse], [ACSMissionGroup], [CallName], [JobMissionName1], [JobMissionName2], [JobMissionName3], [JobMissionName4], [JobMissionName5], [ExecuteBattery], [jobCallSignal], [jobCancelSignal], [POSjobCallSignal_Reg32], [POSjobCallSignal_Reg33], [ProductValue], [ProductActive], [ElevatorModeValue], [ElevatorModeActive], [TransportCountActive], [ErrorMissionName], [MissionName], [JobPriority], [DisplayFlag]) VALUES (6, N'Unuse', N'None', N'None_None', N'None', N'None', N'None', N'None', N'None', 0, N'None', N'None', N'None', N'None', 0, 0, N'None', 0, 0, N'', N'', 0, 0)
SET IDENTITY_INSERT [dbo].[JobConfigs] OFF
SET IDENTITY_INSERT [dbo].[PositionAreaConfig] ON 

INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (1, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 1)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (2, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (3, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (4, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (5, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (6, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (7, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (8, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (9, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
INSERT [dbo].[PositionAreaConfig] ([Id], [ACSRobotGroup], [PositionAreaUse], [PositionAreaFloorName], [PositionAreaFloorMapId], [PositionAreaName], [PositionAreaX1], [PositionAreaX2], [PositionAreaX3], [PositionAreaX4], [PositionAreaY1], [PositionAreaY2], [PositionAreaY3], [PositionAreaY4], [DisplayFlag]) VALUES (10, N'None', N'Unuse', N'None', N'None', N'None', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', 0)
SET IDENTITY_INSERT [dbo].[PositionAreaConfig] OFF
SET IDENTITY_INSERT [dbo].[RobotRegisterSync] ON 

INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (1, N'Unuse', N'None', N'None', N'None', 0, 0, 1)
INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (2, N'Unuse', N'None', N'None', N'None', 0, 0, 0)
INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (3, N'Unuse', N'None', N'None', N'None', 0, 0, 0)
INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (4, N'Unuse', N'None', N'None', N'None', 0, 0, 0)
INSERT [dbo].[RobotRegisterSync] ([Id], [RegisterSyncUse], [PositionGroup], [PositionName], [ACSRobotGroup], [RegisterNo], [RegisterValue], [DisplayFlag]) VALUES (5, N'Unuse', N'None', N'None', N'None', 0, 0, 0)
SET IDENTITY_INSERT [dbo].[RobotRegisterSync] OFF
SET IDENTITY_INSERT [dbo].[Robots] ON 

INSERT [dbo].[Robots] ([Id], [JobId], [ACSRobotGroup], [ACSRobotActive], [Fleet_State], [Fleet_State_Text], [RobotID], [RobotIp], [RobotName], [StateID], [StateText], [MissionText], [MissionQueueID], [MapID], [BatteryPercent], [DistanceToNextTarget], [Position_Orientation], [Position_X], [Position_Y], [Product], [Door], [PositionZoneName], [ErrorsJson], [RobotModel], [RobotAlias]) VALUES (1, 0, N'', 1, NULL, NULL, 1, N'192.168.1.250', N'MiR_S488', N'3', N'Ready', N'Waiting for new missions ...', 0, N'24b3bfa0-440e-11ef-a36c-94c6911e7a70', 44.4, 0, -95.62, 18.48, 11.88, N'', N'', N'', N'[]', N'', N'')
SET IDENTITY_INSERT [dbo].[Robots] OFF
SET IDENTITY_INSERT [dbo].[WaitMissionConfigs] ON 

INSERT [dbo].[WaitMissionConfigs] ([Id], [PositionZone], [WaitMissionUse], [WaitMissionName], [EnableBattery], [ProductValue], [ProductActive], [RobotName], [DisplayFlag]) VALUES (1, N'None', N'Unuse', N'None', 0, 0, 1, N'None', 1)
SET IDENTITY_INSERT [dbo].[WaitMissionConfigs] OFF
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr1]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetErrorHistoryAggr2]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr1]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	--RobotName, SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr1_JobHistoryTransportValueColumnAdd]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr2]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	--SELECT LineName [출발지], SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr2_JobHistoryTransportValueColumnAdd]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr3]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	--SELECT PostName [목적지], SUM(TransportCountValue) [반송량], AVG(datediff(SECOND,JobCreateTime,JobFinishTime)) [평균반송시간]
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr3_JobHistoryTransportValueColumnAdd]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr4]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	--SELECT DATEPART(HOUR, JobFinishTime) [Hour], SUM(TransportCountValue) [반송량]
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
/****** Object:  StoredProcedure [dbo].[spGetJobHistoryAggr4_JobHistoryTransportValueColumnAdd]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetSummary1_총반송량_평균반송시간]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
		--COUNT(*) [총반송량],
		SUM(TransportCountValue) [총반송량],
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
/****** Object:  StoredProcedure [dbo].[spGetSummary2_시간평균반송량]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
			--COUNT(*) [반송량]
			SUM(TransportCountValue)[반송량]
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
/****** Object:  StoredProcedure [dbo].[spGetSummary3_월평균반송량]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
			--COUNT(*) [반송량]
			SUM(TransportCountValue)[반송량]
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
/****** Object:  StoredProcedure [dbo].[spGetSummary4_총에러수_평균반송시간]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetSummary5_시간평균에러수]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetSummary6_월별반송량]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
	SELECT concat(DATEPART(MONTH, JobFinishTime), '월') [Month],  SUM(TransportCountValue)[반송량] --COUNT(*) [반송량]
			
	FROM [JobHistory]
	WHERE 
		(JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
		AND ResultCD = 11  
	GROUP BY DATEPART(MONTH, JobFinishTime)
	ORDER BY DATEPART(MONTH, JobFinishTime)

END
GO
/****** Object:  StoredProcedure [dbo].[spSub1]    Script Date: 2024-08-28 오전 11:56:58 ******/
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
