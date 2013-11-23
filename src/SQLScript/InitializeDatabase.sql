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

GO
PRINT 'Start to set database properties'

ALTER DATABASE [FreightSystemDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FreightSystemDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [FreightSystemDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [FreightSystemDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [FreightSystemDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [FreightSystemDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [FreightSystemDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [FreightSystemDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [FreightSystemDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [FreightSystemDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [FreightSystemDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [FreightSystemDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [FreightSystemDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [FreightSystemDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [FreightSystemDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [FreightSystemDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [FreightSystemDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [FreightSystemDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [FreightSystemDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [FreightSystemDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [FreightSystemDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [FreightSystemDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [FreightSystemDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [FreightSystemDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [FreightSystemDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [FreightSystemDB] SET  READ_WRITE 
GO

ALTER DATABASE [FreightSystemDB] SET RECOVERY FULL 
GO

ALTER DATABASE [FreightSystemDB] SET  MULTI_USER 
GO

ALTER DATABASE [FreightSystemDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [FreightSystemDB] SET DB_CHAINING OFF 
GO

PRINT 'Set database properties successful'

USE FreightSystemDB

SET LANGUAGE 简体中文

PRINT 'Start create table ''Roles'''
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](255) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

PRINT 'Start create table ''Access'''
CREATE TABLE [dbo].[Access](
	[AccessID] [int] IDENTITY(1,1) NOT NULL,
	[CanCreate] [bit] NOT NULL,
	[CanDelete] [bit] NOT NULL,
	[CanUpdate] [bit] NOT NULL,
	[CanQuery] [bit] NOT NULL,
	[ModelName] [nvarchar](255) NOT NULL,
	[PropertyName] [nvarchar](255) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Access] PRIMARY KEY CLUSTERED 
(
	[AccessID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Access]  WITH CHECK ADD  CONSTRAINT [FK_Access_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
ALTER TABLE [dbo].[Access] CHECK CONSTRAINT [FK_Access_Roles]

PRINT 'Start create table ''Users'''
CREATE TABLE [dbo].[Users](
	[UserID] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[Location] [nvarchar](255) NOT NULL,
	[LastLoginTime] [datetime] NOT NULL,
	[LastLoginIP] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleID])

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]

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
	[TrayNo] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_TransportRecords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

PRINT 'Initalize the roles data'
INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('管理员')
INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('调度员')
INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('当地内勤')
INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('总部内勤')
INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('结算')

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
INSERT INTO [dbo].[Access] ([CanCreate] ,[CanDelete] ,[CanUpdate] ,[CanQuery] ,[ModelName] ,[PropertyName] ,[RoleID])
     VALUES (1 ,1 ,1 ,1 , 'ALL' ,'ALL' , @AdminRoleID)

PRINT 'Initalize the admin user data'
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[Location] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('admin' ,'asdasd' ,GetDate() ,@AdminRoleID ,'' ,'杭州' ,GetDate() ,'127.0.0.1' ,'赵一')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[Location] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('deliver' ,'asdasd' ,GetDate() ,@DeliverID ,'' ,'杭州' ,GetDate() ,'127.0.0.1' ,'钱二')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[Location] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('local' ,'asdasd' ,GetDate() ,@LocalID ,'' ,'绍兴' ,GetDate() ,'127.0.0.1' ,'孙三')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[Location] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('company' ,'asdasd' ,GetDate() ,@CompanyID ,'' ,'杭州' ,GetDate() ,'127.0.0.1' ,'李四')
INSERT INTO [dbo].[Users] ([UserID] ,[Password] ,[CreateDateTime] ,[RoleId] ,[Comment] ,[Location] ,[LastLoginTime] ,[LastLoginIP] ,[Name])
     VALUES ('casher' ,'asdasd' ,GetDate() ,@CasherID ,'' ,'杭州' ,GetDate() ,'127.0.0.1' ,'周五')

PRINT 'Create transport records option history table'

CREATE TABLE [dbo].[TransportRecordsOptionHistory](
	[UserID] [nvarchar](50) NOT NULL,
	[TransportRecordID] [int] NOT NULL,
	[LogDateTime] [date] NOT NULL,
	[Operation] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TransportRecordsOptionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransportRecordsOptionHistory_TransportRecords] FOREIGN KEY([TransportRecordID])
REFERENCES [dbo].[TransportRecords] ([ID])
GO

ALTER TABLE [dbo].[TransportRecordsOptionHistory] CHECK CONSTRAINT [FK_TransportRecordsOptionHistory_TransportRecords]
GO

ALTER TABLE [dbo].[TransportRecordsOptionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransportRecordsOptionHistory_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[TransportRecordsOptionHistory] CHECK CONSTRAINT [FK_TransportRecordsOptionHistory_Users]
GO


