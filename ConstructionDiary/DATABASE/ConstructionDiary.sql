USE [master]
GO
/****** Object:  Database [ConstructionDiary]    Script Date: 01/10/2019 10:35:17 PM ******/
CREATE DATABASE [ConstructionDiary]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ConstructionDiary', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVERNEW\MSSQL\DATA\ConstructionDiary.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ConstructionDiary_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVERNEW\MSSQL\DATA\ConstructionDiary_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ConstructionDiary] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConstructionDiary].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConstructionDiary] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ConstructionDiary] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ConstructionDiary] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ConstructionDiary] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ConstructionDiary] SET ARITHABORT OFF 
GO
ALTER DATABASE [ConstructionDiary] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ConstructionDiary] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ConstructionDiary] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ConstructionDiary] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ConstructionDiary] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ConstructionDiary] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ConstructionDiary] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ConstructionDiary] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ConstructionDiary] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ConstructionDiary] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ConstructionDiary] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ConstructionDiary] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ConstructionDiary] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ConstructionDiary] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ConstructionDiary] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ConstructionDiary] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ConstructionDiary] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ConstructionDiary] SET RECOVERY FULL 
GO
ALTER DATABASE [ConstructionDiary] SET  MULTI_USER 
GO
ALTER DATABASE [ConstructionDiary] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ConstructionDiary] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ConstructionDiary] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ConstructionDiary] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ConstructionDiary] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ConstructionDiary', N'ON'
GO
ALTER DATABASE [ConstructionDiary] SET QUERY_STORE = OFF
GO
USE [ConstructionDiary]
GO
/****** Object:  Schema [ConstructionDiary]    Script Date: 01/10/2019 10:35:17 PM ******/
CREATE SCHEMA [ConstructionDiary]
GO
/****** Object:  Table [dbo].[tbl_Clients]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Clients](
	[ClientId] [uniqueidentifier] NOT NULL,
	[ClientName] [nvarchar](365) NOT NULL,
	[FirmName] [nvarchar](100) NULL,
	[PackageTypeId] [int] NOT NULL,
	[ExpireDate] [datetime] NULL,
	[Remarks] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ContractorFinance]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContractorFinance](
	[ContractorFinanceId] [uniqueidentifier] NOT NULL,
	[SiteId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[SelectedDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[CreditOrDebit] [nvarchar](50) NULL,
	[ReasonFor] [nvarchar](365) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[ChequeNo] [nvarchar](50) NULL,
	[BankName] [nvarchar](50) NULL,
	[BankBranch] [nvarchar](50) NULL,
	[Remarks] [nvarchar](365) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_ContractorFinance] PRIMARY KEY CLUSTERED 
(
	[ContractorFinanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Finance]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Finance](
	[FinanceId] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NULL,
	[SelectedDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[CreditOrDebit] [nvarchar](50) NULL,
	[GivenAmountBy] [uniqueidentifier] NULL,
	[ReasonFor] [nvarchar](365) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[ChequeNo] [nvarchar](50) NULL,
	[BankName] [nvarchar](50) NULL,
	[BankBranch] [nvarchar](50) NULL,
	[Remarks] [nvarchar](365) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Finance] PRIMARY KEY CLUSTERED 
(
	[FinanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GeneralSetting]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GeneralSetting](
	[GeneralSettingId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tbl_GeneralSetting] PRIMARY KEY CLUSTERED 
(
	[GeneralSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PackageType]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PackageType](
	[PackageTypeId] [int] IDENTITY(1,1) NOT NULL,
	[PackageType] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tbl_PackageType] PRIMARY KEY CLUSTERED 
(
	[PackageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PaymentTransaction]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PaymentTransaction](
	[PaymentTransactionId] [uniqueidentifier] NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[PackageTypeId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](200) NULL,
	[IsPaid] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_PaymentTransaction] PRIMARY KEY CLUSTERED 
(
	[PaymentTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Persons]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Persons](
	[PersonId] [uniqueidentifier] NOT NULL,
	[PersonFirstName] [nvarchar](365) NULL,
	[PersonAddress] [nvarchar](365) NULL,
	[MobileNo] [nvarchar](50) NULL,
	[PersonPhoto] [nvarchar](365) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ClientId] [uniqueidentifier] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Persons] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Role]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NULL,
 CONSTRAINT [PK_tbl_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Sites]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Sites](
	[SiteId] [uniqueidentifier] NOT NULL,
	[SiteName] [nvarchar](365) NULL,
	[SiteDescription] [nvarchar](max) NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Users]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](367) NOT NULL,
	[Password] [nvarchar](67) NOT NULL,
	[ClientId] [uniqueidentifier] NULL,
	[RoleId] [int] NOT NULL,
	[FirstName] [nvarchar](367) NOT NULL,
	[EmailId] [nvarchar](67) NULL,
	[MobileNo] [nvarchar](15) NULL,
	[UserPhoto] [nvarchar](365) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_Clients] ([ClientId], [ClientName], [FirmName], [PackageTypeId], [ExpireDate], [Remarks], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'c6da8912-b1c7-4730-b7b3-795404366a68', N'Kamlesh Prajapati', N'RR Traders', 1, CAST(N'2019-10-15T00:00:00.000' AS DateTime), NULL, 1, 0, CAST(N'2019-09-28T19:02:41.223' AS DateTime), NULL)
INSERT [dbo].[tbl_Clients] ([ClientId], [ClientName], [FirmName], [PackageTypeId], [ExpireDate], [Remarks], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'3fcd0429-92e9-4079-85d8-aa8b9a7f180f', N'Arvind Prajapati', N'Dream Construction', 1, CAST(N'2019-10-05T00:00:00.000' AS DateTime), NULL, 1, 0, CAST(N'2019-09-22T18:54:47.153' AS DateTime), CAST(N'2019-09-29T08:52:17.410' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'25b81f7d-90a5-4ecc-953f-11cae4080040', N'33ee41ea-2e6b-4217-9f6b-71ab3d0acb3e', N'5210265d-340c-4a9f-b4d8-6d2565204612', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'Credit', N'test entry', N'Cash', NULL, NULL, NULL, N'fdfgdggd', 1, 0, N'5210265d-340c-4a9f-b4d8-6d2565204612', NULL, CAST(N'2019-09-29T18:19:07.800' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd03ac5d7-4026-43e7-816d-4004ab550372', N'33ee41ea-2e6b-4217-9f6b-71ab3d0acb3e', N'da71e191-9433-487d-b12b-ffb2c544ce5e', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(5000.00 AS Decimal(18, 2)), N'Debit', N'abc', N'Cheque', N'abc', N'HDFC', N'anand', N'fdfgdggd', 1, 0, N'5210265d-340c-4a9f-b4d8-6d2565204612', NULL, CAST(N'2019-09-29T18:21:11.130' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2ce6d3c0-9998-4a56-98ef-73cd1f3a52d7', N'b2a5427f-6778-477e-8584-09cbb5d1c8a9', N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'Debit', N'test', N'Cheque', NULL, NULL, NULL, N'aaaaa', 1, 0, N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', NULL, CAST(N'2019-09-29T18:24:02.903' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd5f992e8-16b7-40aa-a45a-ae5c8ff47dc0', N'b2a5427f-6778-477e-8584-09cbb5d1c8a9', N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'first', N'Cash', NULL, NULL, NULL, N'aaaaa', 1, 0, N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', NULL, CAST(N'2019-09-29T18:22:35.233' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a2d932af-0d38-414d-81f5-c8bb3f068318', N'33ee41ea-2e6b-4217-9f6b-71ab3d0acb3e', N'5210265d-340c-4a9f-b4d8-6d2565204612', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'Credit', N'first entry', N'Cash', NULL, NULL, NULL, N'aasas', 1, 1, N'5210265d-340c-4a9f-b4d8-6d2565204612', N'5210265d-340c-4a9f-b4d8-6d2565204612', CAST(N'2019-09-29T18:16:24.477' AS DateTime), CAST(N'2019-09-29T18:17:08.390' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'263c0ca6-718d-4bda-906b-e7a940ac46e8', N'b2a5427f-6778-477e-8584-09cbb5d1c8a9', N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', CAST(N'2019-09-28T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'Credit', N'test', N'Cheque', NULL, NULL, NULL, NULL, 1, 1, N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', CAST(N'2019-09-29T18:23:27.237' AS DateTime), CAST(N'2019-09-29T18:23:40.380' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [ReasonFor], [PaymentType], [ChequeNo], [BankName], [BankBranch], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'8b3f08be-36c8-443f-abc4-fbe42841b222', N'33ee41ea-2e6b-4217-9f6b-71ab3d0acb3e', N'da71e191-9433-487d-b12b-ffb2c544ce5e', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(5000.00 AS Decimal(18, 2)), N'Credit', N'abc', N'Cheque', NULL, NULL, NULL, N'fdfdfdfdf', 1, 1, N'5210265d-340c-4a9f-b4d8-6d2565204612', N'5210265d-340c-4a9f-b4d8-6d2565204612', CAST(N'2019-09-29T18:19:47.333' AS DateTime), CAST(N'2019-09-29T18:20:38.947' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_PackageType] ON 

INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (1, N'Free Trial Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (2, N'1 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (3, N'3 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (4, N'6 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (5, N'1 Year Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (6, N'One Time Package')
SET IDENTITY_INSERT [dbo].[tbl_PackageType] OFF
SET IDENTITY_INSERT [dbo].[tbl_Role] ON 

INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (1, N'Site Owner')
INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (2, N'Admin')
INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (3, N'Sub User')
SET IDENTITY_INSERT [dbo].[tbl_Role] OFF
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b2a5427f-6778-477e-8584-09cbb5d1c8a9', N'Vivah Party Plot', N'Sojitra road - Ashwinbhai Patel', N'c6da8912-b1c7-4730-b7b3-795404366a68', 1, 0, N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', CAST(N'2019-09-29T17:06:43.227' AS DateTime), CAST(N'2019-09-29T17:14:15.413' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'33ee41ea-2e6b-4217-9f6b-71ab3d0acb3e', N'Vinayak Bunglows', N'With Material - Bhavinbhai Patel', N'3fcd0429-92e9-4079-85d8-aa8b9a7f180f', 1, 0, N'5210265d-340c-4a9f-b4d8-6d2565204612', N'5210265d-340c-4a9f-b4d8-6d2565204612', CAST(N'2019-09-29T16:56:14.200' AS DateTime), CAST(N'2019-09-29T17:13:42.603' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'5210265d-340c-4a9f-b4d8-6d2565204612', N'arvindp', N'arvindp', N'3fcd0429-92e9-4079-85d8-aa8b9a7f180f', 2, N'Arvind Prajapati', N'arvindp689@gmail.com', N'9638640840', NULL, 1, 0, CAST(N'2019-09-28T14:53:26.750' AS DateTime), NULL)
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'1d09ec36-697b-4e5b-8612-a48dcacc2c47', N'kamleshp', N'kamleshp', N'c6da8912-b1c7-4730-b7b3-795404366a68', 2, N'Kamlesh Prajapati', NULL, NULL, NULL, 1, 0, CAST(N'2019-09-29T05:58:55.723' AS DateTime), NULL)
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'fc6cf0fd-2304-4016-a95d-b64083febee3', N'admin', N'admin', NULL, 1, N'Site Owner', N'prajapati.nileshbhai@gmail.com', N'9824936252', N'd9b69101-1d43-40e4-a0a0-9ffe8e6cc35buser8-128x128.jpg', 1, 0, CAST(N'2019-09-16T23:36:18.600' AS DateTime), CAST(N'2019-09-29T05:57:45.317' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'da71e191-9433-487d-b12b-ffb2c544ce5e', N'nileshp', N'nileshp', N'3fcd0429-92e9-4079-85d8-aa8b9a7f180f', 3, N'Nilesh Prajapati', N'nilesh007444@gmail.com', N'9824936252', NULL, 1, 0, CAST(N'2019-09-28T18:34:30.743' AS DateTime), CAST(N'2019-09-28T18:35:13.253' AS DateTime))
ALTER TABLE [dbo].[tbl_Clients]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Clients_tbl_PackageType] FOREIGN KEY([PackageTypeId])
REFERENCES [dbo].[tbl_PackageType] ([PackageTypeId])
GO
ALTER TABLE [dbo].[tbl_Clients] CHECK CONSTRAINT [FK_tbl_Clients_tbl_PackageType]
GO
ALTER TABLE [dbo].[tbl_ContractorFinance]  WITH CHECK ADD  CONSTRAINT [FK_tbl_ContractorFinance_tbl_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[tbl_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[tbl_ContractorFinance] CHECK CONSTRAINT [FK_tbl_ContractorFinance_tbl_Sites]
GO
ALTER TABLE [dbo].[tbl_Users]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Users_tbl_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[tbl_Clients] ([ClientId])
GO
ALTER TABLE [dbo].[tbl_Users] CHECK CONSTRAINT [FK_tbl_Users_tbl_Clients]
GO
ALTER TABLE [dbo].[tbl_Users]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Users_tbl_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tbl_Role] ([RoleId])
GO
ALTER TABLE [dbo].[tbl_Users] CHECK CONSTRAINT [FK_tbl_Users_tbl_Role]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPersonsList]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- [dbo].[SP_GetPersonsList] 

CREATE PROCEDURE [dbo].[SP_GetPersonsList]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET FMTONLY OFF

	--Temp Table: 
   CREATE TABLE [dbo].#TempPerons 
   ( 
	    PersonId	uniqueidentifier,
		PersonFirstName	nvarchar(365),
		PersonAddress	nvarchar(365),
		MobileNo	nvarchar(50),
		PersonPhoto	nvarchar(365),
		IsActive	bit,
		IsDeleted	bit,
		TotalCreditAmount decimal(18,2),
		TotalDebitAmount decimal(18,2),
		TotalRemainingAmount decimal(18,2)
   ) 

   INSERT INTO [dbo].#TempPerons
	SELECT 	
	PersonId
	,PersonFirstName
	,PersonAddress
	,MobileNo
	,PersonPhoto
	,IsActive
	,IsDeleted,
	(
		SELECT ISNULL(SUM(finance.Amount),0) FROM tbl_Finance finance, tbl_Users users
		WHERE finance.GivenAmountBy = users.UserId AND finance.PersonId = tbl_Persons.PersonId
		AND finance.CreditOrDebit = 'Credit' and finance.IsActive = 1 and finance.IsDeleted = 0
	) as TotalCreditAmount,
	(
		SELECT ISNULL(SUM(finance.Amount),0) FROM tbl_Finance finance, tbl_Users users
		WHERE finance.GivenAmountBy = users.UserId AND finance.PersonId = tbl_Persons.PersonId
		AND finance.CreditOrDebit = 'Debit' and finance.IsActive = 1 and finance.IsDeleted = 0
	) as TotalDebitAmount,
	null

	FROM tbl_Persons
	WHERE IsActive=1 and IsDeleted=0
	ORDER BY CreatedDate DESC

	SELECT 
	PersonId
	,PersonFirstName
	,PersonAddress
	,MobileNo
	,PersonPhoto
	,IsActive
	,IsDeleted
	,TotalCreditAmount
	,TotalDebitAmount
	,(TotalCreditAmount - TotalDebitAmount)  as TotalRemainingAmount 

	FROM [dbo].#TempPerons	

	DROP TABLE [dbo].#TempPerons
	
END


GO
/****** Object:  StoredProcedure [dbo].[SP_GetSiteDetailById]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- [dbo].[SP_GetSiteDetailById] '99F2026C-C0EF-441D-B1E7-41BAD4FDC1D6'

CREATE PROCEDURE [dbo].[SP_GetSiteDetailById] 
	-- Add the parameters for the stored procedure here
	@SiteId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET FMTONLY OFF

	--Temp Table: 
   CREATE TABLE [dbo].#TempFinance
   ( 
	    ContractorFinanceId uniqueidentifier,
		UserName nvarchar(365),
		SelectedDate datetime,
		Amount decimal(18,2),
		CreditOrDebit nvarchar(50),
		SiteId uniqueidentifier,
		ReasonFor nvarchar(365),
		PaymentType nvarchar(50),
		ChequeNo nvarchar(50),
		BankName nvarchar(50),
		BankBranch nvarchar(50),
		Remarks nvarchar(MAX)
   ) 

   INSERT INTO [dbo].#TempFinance
	SELECT 	
	ContractorFinanceId,
	FirstName as UserName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	SiteId,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks
	FROM tbl_ContractorFinance finance, tbl_Users users
	WHERE finance.UserId = users.UserId
	AND finance.IsActive=1 and finance.IsDeleted=0 AND SiteId = @SiteId
	ORDER BY finance.CreatedDate DESC

	SELECT 
	ContractorFinanceId,
	UserName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	SiteId,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks

	FROM [dbo].#TempFinance

	DROP TABLE [dbo].#TempFinance
	
END


GO
/****** Object:  StoredProcedure [dbo].[SP_GetSitesList]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- [dbo].[SP_GetSitesList] 

CREATE PROCEDURE [dbo].[SP_GetSitesList]
	-- Add the parameters for the stored procedure here
	 @ClientId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET FMTONLY OFF

	--Temp Table: 
   CREATE TABLE [dbo].#TempSites 
   ( 
	    SiteId uniqueidentifier,
		SiteName nvarchar(365),
		SiteDescription nvarchar(365),
		IsActive	bit,
		IsDeleted	bit,
		TotalCreditAmount decimal(18,2),
		TotalDebitAmount decimal(18,2),
		TotalRemainingAmount decimal(18,2)
   ) 

   INSERT INTO [dbo].#TempSites
	SELECT 	
	SiteId,
	SiteName,
	SiteDescription,
	IsActive,
	IsDeleted,
	(
		SELECT ISNULL(SUM(finance.Amount),0) FROM tbl_ContractorFinance finance
		WHERE finance.CreditOrDebit = 'Credit' and finance.IsActive = 1 and finance.IsDeleted = 0
		AND finance.SiteId = tbl_Sites.SiteId
	) as TotalCreditAmount,
	(
		SELECT ISNULL(SUM(finance.Amount),0) FROM tbl_ContractorFinance finance
		WHERE finance.CreditOrDebit = 'Debit' and finance.IsActive = 1 and finance.IsDeleted = 0
		AND finance.SiteId = tbl_Sites.SiteId
	) as TotalDebitAmount,
	null

	FROM tbl_Sites
	WHERE IsActive=1 and IsDeleted=0 AND ClientId = @ClientId
	ORDER BY CreatedDate DESC

	SELECT 
	SiteId,
	SiteName,
	SiteDescription,
	IsActive,
	IsDeleted,
	TotalCreditAmount,
	TotalDebitAmount,
	(TotalCreditAmount - TotalDebitAmount)  as TotalRemainingAmount 

	FROM [dbo].#TempSites	

	DROP TABLE [dbo].#TempSites
	
END


GO
/****** Object:  StoredProcedure [dbo].[SP_GetTodayPartyFinance]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- EXEC [dbo].[SP_GetTodayPartyFinance] 

CREATE PROCEDURE [dbo].[SP_GetTodayPartyFinance] 
	-- Add the parameters for the stored procedure here
	@ClientId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET FMTONLY OFF

	--Temp Table: 
   CREATE TABLE [dbo].#TempFinance
   ( 
	    ContractorFinanceId	uniqueidentifier,
		SiteId	uniqueidentifier,
		SiteName nvarchar(365),
		SelectedDate	datetime,
		Amount	decimal(18, 2),
		CreditOrDebit	nvarchar(50),
		UserId	uniqueidentifier,
		ReasonFor	nvarchar(365),
		PaymentType	nvarchar(50),
		ChequeNo	nvarchar(50),
		BankName	nvarchar(50),
		BankBranch	nvarchar(50),
		Remarks	nvarchar(365),
		UserName nvarchar(50),
   ) 

   INSERT INTO [dbo].#TempFinance
	SELECT 	
	ContractorFinanceId,
	finance.SiteId, 
	SiteName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	finance.UserId,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks,
	users.FirstName as UserName
	FROM tbl_ContractorFinance finance, tbl_Users users, tbl_Sites sites
	WHERE finance.UserId = users.UserId AND finance.SiteId = sites.SiteId
	AND finance.IsActive=1 and finance.IsDeleted=0 
	AND sites.ClientId = @ClientId 
	AND CAST(SelectedDate AS DATE) = CAST(GETDATE() AS DATE)
	ORDER BY finance.CreatedDate DESC

	SELECT 
	ContractorFinanceId,
	SiteId, 
	SiteName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	UserId,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks,
	UserName

	FROM [dbo].#TempFinance

	DROP TABLE [dbo].#TempFinance
	
END


GO
/****** Object:  StoredProcedure [dbo].[SP_GetTodayPersonFinance]    Script Date: 01/10/2019 10:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- EXEC [dbo].[SP_GetTodayPersonFinance] 

CREATE PROCEDURE [dbo].[SP_GetTodayPersonFinance] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET FMTONLY OFF

	--Temp Table: 
   CREATE TABLE [dbo].#TempFinance
   ( 
	    FinanceId	uniqueidentifier,
		PersonId	uniqueidentifier,
		PersonFirstName nvarchar(365),
		SelectedDate	datetime,
		Amount	decimal(18, 2),
		CreditOrDebit	nvarchar(50),
		GivenAmountBy	uniqueidentifier,
		ReasonFor	nvarchar(365),
		PaymentType	nvarchar(50),
		ChequeNo	nvarchar(50),
		BankName	nvarchar(50),
		BankBranch	nvarchar(50),
		Remarks	nvarchar(365),
		UserName nvarchar(50),
   ) 

   INSERT INTO [dbo].#TempFinance
	SELECT 	
	FinanceId,
	finance.PersonId,
	PersonFirstName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	GivenAmountBy,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks,
	users.FirstName as UserName
	FROM tbl_Finance finance, tbl_Users users, tbl_Persons person
	WHERE finance.GivenAmountBy = users.UserId AND finance.PersonId = person.PersonId
	AND finance.IsActive=1 and finance.IsDeleted=0 
	AND CAST(SelectedDate AS DATE) = CAST(GETDATE() AS DATE)
	ORDER BY finance.CreatedDate DESC

	SELECT 
	FinanceId,
	PersonId,
	PersonFirstName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	GivenAmountBy,
	ReasonFor,
	PaymentType,
	ChequeNo,
	BankName,
	BankBranch,
	Remarks,
	UserName

	FROM [dbo].#TempFinance

	DROP TABLE [dbo].#TempFinance
	
END


GO
USE [master]
GO
ALTER DATABASE [ConstructionDiary] SET  READ_WRITE 
GO
