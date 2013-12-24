USE [master]
GO

DECLARE @DBName VARCHAR(50)
SET @DBName = 'FreightSystemDB'
-- Delete the database if it's existed --
IF EXISTS(SELECT * FROM sysdatabases where name=@DBName)
BEGIN
	PRINT 'Kill all connections for DB ' + @DBName
	
	DECLARE @d VARCHAR(8000) 
	SET @d = ' '	
	SELECT @d = @d + '   kill   ' + CAST(spid AS VARCHAR) + CHAR(13) FROM MASTER.sys.sysprocesses WHERE   dbid = DB_ID(@DBName)	
	EXEC (@d)
	
	PRINT 'Drop database ' + @DBName
	DROP DATABASE FreightSystemDB
	
END

PRINT 'Create database ' + @DBName
CREATE DATABASE [FreightSystemDB] COLLATE Chinese_Simplified_Pinyin_100_BIN

PRINT 'Start to set database properties'

ALTER DATABASE [FreightSystemDB] SET COMPATIBILITY_LEVEL = 100

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FreightSystemDB].[dbo].[sp_fulltext_database] @action = 'enable'
end

ALTER DATABASE [FreightSystemDB] SET ANSI_NULL_DEFAULT OFF 

ALTER DATABASE [FreightSystemDB] SET ANSI_NULLS OFF 

ALTER DATABASE [FreightSystemDB] SET ANSI_PADDING OFF 

ALTER DATABASE [FreightSystemDB] SET ANSI_WARNINGS OFF 

ALTER DATABASE [FreightSystemDB] SET ARITHABORT OFF 

ALTER DATABASE [FreightSystemDB] SET AUTO_CLOSE OFF 

ALTER DATABASE [FreightSystemDB] SET AUTO_CREATE_STATISTICS ON 

ALTER DATABASE [FreightSystemDB] SET AUTO_SHRINK OFF 

ALTER DATABASE [FreightSystemDB] SET AUTO_UPDATE_STATISTICS ON 

ALTER DATABASE [FreightSystemDB] SET CURSOR_CLOSE_ON_COMMIT OFF 

ALTER DATABASE [FreightSystemDB] SET CURSOR_DEFAULT  GLOBAL 

ALTER DATABASE [FreightSystemDB] SET CONCAT_NULL_YIELDS_NULL OFF 

ALTER DATABASE [FreightSystemDB] SET NUMERIC_ROUNDABORT OFF 

ALTER DATABASE [FreightSystemDB] SET QUOTED_IDENTIFIER OFF 

ALTER DATABASE [FreightSystemDB] SET RECURSIVE_TRIGGERS OFF 

ALTER DATABASE [FreightSystemDB] SET  DISABLE_BROKER 

ALTER DATABASE [FreightSystemDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 

ALTER DATABASE [FreightSystemDB] SET DATE_CORRELATION_OPTIMIZATION OFF 

ALTER DATABASE [FreightSystemDB] SET TRUSTWORTHY OFF 

ALTER DATABASE [FreightSystemDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 

ALTER DATABASE [FreightSystemDB] SET PARAMETERIZATION SIMPLE 

ALTER DATABASE [FreightSystemDB] SET READ_COMMITTED_SNAPSHOT OFF 

ALTER DATABASE [FreightSystemDB] SET HONOR_BROKER_PRIORITY OFF 

ALTER DATABASE [FreightSystemDB] SET  READ_WRITE 

ALTER DATABASE [FreightSystemDB] SET RECOVERY FULL 

ALTER DATABASE [FreightSystemDB] SET  MULTI_USER 

ALTER DATABASE [FreightSystemDB] SET PAGE_VERIFY CHECKSUM  

ALTER DATABASE [FreightSystemDB] SET DB_CHAINING OFF 

PRINT 'Set database properties successful'

USE FreightSystemDB
GO

SET LANGUAGE 简体中文

PRINT 'Start create table ''Roles'''
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](255) NULL,
	[AccessList] [nvarchar](500) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

Print 'Start create table ''BusinessArea''' 
USE [FreightSystemDB]
GO

/****** Object:  Table [dbo].[BusinessArea]    Script Date: 11/26/2013 13:19:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[BusinessArea](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AreaName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_BusinessArea] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
Insert into [dbo].[BusinessArea]([AreaName]) values('杭州')
Insert into [dbo].[BusinessArea]([AreaName]) values('下沙')
Insert into [dbo].[BusinessArea]([AreaName]) values('绍兴')

Declare @HangzhouID int
Declare @XiashaID int
Declare @ShaoxingID int
select @HangzhouID = [ID] from [dbo].[BusinessArea] where [AreaName]='杭州'
select @XiashaID = [ID] from [dbo].[BusinessArea] where [AreaName]='下沙'
select @ShaoxingID = [ID] from [dbo].[BusinessArea] where [AreaName]='绍兴'

PRINT 'Start create table ''Users'''
CREATE TABLE [dbo].[Users](
	[UserID] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[AreaID] [int] NOT NULL,
	[LastLoginTime] [datetime] NOT NULL,
	[LastLoginIP] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([RoleID])
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_BusinessArea] FOREIGN KEY([AreaID]) REFERENCES [dbo].[BusinessArea] ([ID])
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_BusinessArea]

PRINT 'Start create table ''TransportRecords'''
CREATE TABLE [dbo].[TransportRecords](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarLicense] [nvarchar](255) NOT NULL,
	[ClientName] [nvarchar](255) NOT NULL,
	[DeliverDate] [datetime] NOT NULL,
	[Driver] [nvarchar](255) NOT NULL,
	[FromLocation] [nvarchar](255) NOT NULL,
	[PackageName] [nvarchar](255) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ToLocation] [nvarchar](255) NOT NULL,
	[Volume] [float] NOT NULL,
	[AccountPayble] [float] NULL,
	[Comment] [nvarchar](255) NULL,
	[Deductions] [float] NULL,
	[DeliverPrice] [float] NULL,
	[DeliverType] [nvarchar](255) NULL,
	[HandlingFee] [float] NULL,
	[PayDate] [datetime] NULL,
	[PrePay] [float] NULL,
	[OilCard] [float] NULL,
	[Reparations] [float] NULL,
	[ShortBargeFee] [float] NULL,
	[Status] [nvarchar](255) NULL,
	[TrayNo] [nvarchar](255) NULL,
	[BusinessArea] [nvarchar](255) NOT NULL,
	[Error] [bit] NOT NULL,
	[Received] [bit] NOT NULL,
	[ReceivedDate] [DateTime] NULL,
	[Paid] [bit] NOT NULL
 CONSTRAINT [PK_TransportRecords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

PRINT 'Initalize the roles data'
INSERT INTO [dbo].[Roles] ([RoleName],[AccessList]) VALUES ('管理员','ALL')
INSERT INTO [dbo].[Roles] ([RoleName],[AccessList]) VALUES ('调度员','')
INSERT INTO [dbo].[Roles] ([RoleName],[AccessList]) VALUES ('当地内勤','LOCALRDLIST|NEWRD|FILLRD|EXPORT')
INSERT INTO [dbo].[Roles] ([RoleName],[AccessList]) VALUES ('总部内勤','TOTALRDLIST|NEWRD|PRINT|EXPORT')
INSERT INTO [dbo].[Roles] ([RoleName],[AccessList]) VALUES ('结算','TOTALRDLIST|EXPORT|UPDTPAID')

PRINT 'Initalize the access data'
DECLARE @AdminRoleID int
select @AdminRoleID = RoleID from [dbo].[Roles] where RoleName='管理员'
DECLARE @DeliverID int
select @DeliverID = RoleID from [dbo].[Roles] where RoleName='调度员'
DECLARE @LocalID int
select @LocalID = RoleID from [dbo].[Roles] where RoleName='当地内勤'
DECLARE @CompanyID int
select @CompanyID = RoleID from [dbo].[Roles] where RoleName='总部内勤'
DECLARE @CasherID int
select @CasherID = RoleID from [dbo].[Roles] where RoleName='结算'

PRINT 'Initalize the admin user data'
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[AreaID] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('admin' ,'asdasd' ,GetDate() ,@AdminRoleID ,'' ,@HangzhouID ,GetDate() ,'127.0.0.1' ,'赵一')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[AreaID] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('deliver' ,'asdasd' ,GetDate() ,@DeliverID ,'' ,@HangzhouID ,GetDate() ,'127.0.0.1' ,'钱二')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[AreaID] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('local' ,'asdasd' ,GetDate() ,@LocalID ,'' ,@XiashaID ,GetDate() ,'127.0.0.1' ,'孙三')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[AreaID] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('company' ,'asdasd' ,GetDate() ,@CompanyID ,'' ,@HangzhouID ,GetDate() ,'127.0.0.1' ,'李四')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[AreaID] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('casher' ,'asdasd' ,GetDate() ,@CasherID ,'' ,@HangzhouID ,GetDate() ,'127.0.0.1' ,'周五')

PRINT 'Create transport records option history table'

CREATE TABLE [dbo].[TransportRecordsOptionHistory](
	[UserID] [nvarchar](50) NOT NULL,
	[TransportRecordID] [int] NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[Operation] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_TransportRecordsOptionHistory] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[TransportRecordID] ASC,
	[LogDateTime] ASC,
	[Operation] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TransportRecordsOptionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransportRecordsOptionHistory_TransportRecords] FOREIGN KEY([TransportRecordID])
REFERENCES [dbo].[TransportRecords] ([ID])

ALTER TABLE [dbo].[TransportRecordsOptionHistory] CHECK CONSTRAINT [FK_TransportRecordsOptionHistory_TransportRecords]

ALTER TABLE [dbo].[TransportRecordsOptionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransportRecordsOptionHistory_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])

ALTER TABLE [dbo].[TransportRecordsOptionHistory] CHECK CONSTRAINT [FK_TransportRecordsOptionHistory_Users]

PRINT 'Create Menu item table'
CREATE TABLE [dbo].[MenuItem](
	[MenuCode] [varchar](50) NOT NULL,
	[MenuText] [varchar](50) NOT NULL,
	[Link] [varchar](500) NOT NULL,
	[OrderIndex] [int] NOT NULL,
 CONSTRAINT [PK_MenuItem] PRIMARY KEY CLUSTERED 
(
	[MenuCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

Print 'Insert menu item records'
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('TOTALDELIVER','全部动态','/Business/Index',1)
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('LOCALDELIVER','当地动态','/Business/LocalTransportRecordList',2)
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('ROLEMGR','角色管理','/Security/RoleList',3)
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('USERMGR','用户管理','/Security/UserList',4)
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('AREAMGR','业务地域管理','/Business/AreaList',5)
Insert Into [dbo].[MenuItem](MenuCode,MenuText,Link,OrderIndex) Values('MONTHRPT','月报表','/Business/ExcelMonthlyReport',6)


PRINT 'Create menu access table'

CREATE TABLE [dbo].[MenuAccess](
	[MenuCode] [varchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_MenuAccess] PRIMARY KEY CLUSTERED 
(
	[MenuCode] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[MenuAccess]  WITH CHECK ADD  CONSTRAINT [FK_MenuAccess_MenuAccess] FOREIGN KEY([MenuCode])
REFERENCES [dbo].[MenuItem] ([MenuCode])

ALTER TABLE [dbo].[MenuAccess] CHECK CONSTRAINT [FK_MenuAccess_MenuAccess]

ALTER TABLE [dbo].[MenuAccess]  WITH CHECK ADD  CONSTRAINT [FK_MenuAccess_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])

ALTER TABLE [dbo].[MenuAccess] CHECK CONSTRAINT [FK_MenuAccess_Roles]

Print 'Insert menu access for Admin role'
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('TOTALDELIVER',@AdminRoleID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('TOTALDELIVER',@CasherID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('TOTALDELIVER',@CompanyID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('ROLEMGR',@AdminRoleID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('USERMGR',@AdminRoleID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('AREAMGR',@AdminRoleID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('MONTHRPT',@AdminRoleID)
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('MONTHRPT',@CasherID)

Print 'Insert menu access for local deliver role'
Insert Into [dbo].[MenuAccess]([MenuCode], [RoleID]) Values('LOCALDELIVER',@LocalID)

Print 'Create function item table'

CREATE TABLE [dbo].[FuncItem](
	[FuncCode] [varchar](50) NOT NULL,
	[FuncText] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FuncItem] PRIMARY KEY CLUSTERED 
(
	[FuncCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('ALL','所有权限')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('NEWRD','新建动态')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('TOTALRDLIST','全部动态')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('LOCALRDLIST','当地动态')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('FILLRD','补充动态记录信息')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('EXPORT','导出报表')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('PRINT','打印动态表')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('UPDTREV','修改到货')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('UPDTERR','修改异常')
Insert Into [dbo].[FuncItem]([FuncCode],[FuncText]) Values('UPDTPAID','修改结算')