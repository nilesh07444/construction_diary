USE [master]
GO
/****** Object:  Database [DB_A4E4E6_myconstruction]    Script Date: 24/11/2019 6:17:02 PM ******/
CREATE DATABASE [DB_A4E4E6_myconstruction]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_A4E4E6_myconstruction_Data', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DB_A4E4E6_myconstruction_DATA.mdf' , SIZE = 8192KB , MAXSIZE = 1024000KB , FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DB_A4E4E6_myconstruction_Log', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DB_A4E4E6_myconstruction_Log.LDF' , SIZE = 3072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_A4E4E6_myconstruction].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET  MULTI_USER 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET QUERY_STORE = OFF
GO
USE [DB_A4E4E6_myconstruction]
GO
/****** Object:  Schema [DB_A4E4E6_myconstruction]    Script Date: 24/11/2019 6:17:06 PM ******/
CREATE SCHEMA [DB_A4E4E6_myconstruction]
GO
/****** Object:  Table [dbo].[tbl_Clients]    Script Date: 24/11/2019 6:17:06 PM ******/
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
/****** Object:  Table [dbo].[tbl_ContractorFinance]    Script Date: 24/11/2019 6:17:07 PM ******/
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
	[PaymentType] [nvarchar](50) NULL,
	[ChequeNo] [nvarchar](50) NULL,
	[BankName] [nvarchar](50) NULL,
	[ChequeFor] [nvarchar](50) NULL,
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
/****** Object:  Table [dbo].[tbl_Finance]    Script Date: 24/11/2019 6:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Finance](
	[FinanceId] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[SelectedDate] [datetime] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[SiteId] [uniqueidentifier] NULL,
	[CreditOrDebit] [nvarchar](50) NOT NULL,
	[GivenAmountBy] [uniqueidentifier] NOT NULL,
	[PaymentType] [nvarchar](50) NOT NULL,
	[ChequeNo] [nvarchar](50) NULL,
	[BankName] [nvarchar](50) NULL,
	[ChequeFor] [nvarchar](50) NULL,
	[Remarks] [nvarchar](365) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tbl_Finance] PRIMARY KEY CLUSTERED 
(
	[FinanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GeneralSetting]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_PackageType]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_PaymentTransaction]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_Persons]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_Role]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_Sites]    Script Date: 24/11/2019 6:17:07 PM ******/
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
/****** Object:  Table [dbo].[tbl_Users]    Script Date: 24/11/2019 6:17:07 PM ******/
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
INSERT [dbo].[tbl_Clients] ([ClientId], [ClientName], [FirmName], [PackageTypeId], [ExpireDate], [Remarks], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'8727fb85-2667-48ca-a50e-16f1194305be', N'Nilesh Prajapati', N'Prajapati Infosoft', 3, CAST(N'2020-01-14T00:00:00.000' AS DateTime), NULL, 1, 0, CAST(N'2019-10-14T16:56:35.203' AS DateTime), NULL)
INSERT [dbo].[tbl_Clients] ([ClientId], [ClientName], [FirmName], [PackageTypeId], [ExpireDate], [Remarks], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'Kamlesh Prajapati', N'R R Traders', 1, CAST(N'2019-11-27T00:00:00.000' AS DateTime), NULL, 1, 0, CAST(N'2019-10-07T06:44:46.960' AS DateTime), CAST(N'2019-11-16T10:59:33.233' AS DateTime))
INSERT [dbo].[tbl_Clients] ([ClientId], [ClientName], [FirmName], [PackageTypeId], [ExpireDate], [Remarks], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', N'Arvind Prajapati', N'Maruti Construction', 1, CAST(N'2019-11-15T00:00:00.000' AS DateTime), NULL, 1, 0, CAST(N'2019-10-01T18:08:05.163' AS DateTime), CAST(N'2019-11-01T08:22:47.483' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'74094f3e-8822-4ec9-bed3-01c385a94123', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-18T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:07:54.177' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dd69a916-2b25-4c7d-997f-0664f3f39302', N'358d08eb-0718-45c8-ae85-1d68c1c8d89c', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', CAST(N'2019-10-14T00:00:00.000' AS DateTime), CAST(12000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'0006', N'HDFC Bank', N'Arvind Prajapati', N'reti', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-14T17:00:24.037' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3edd72a4-50e6-4295-8550-083d8f7f1ad6', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-18T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:30:10.947' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a98c0114-6ff8-4ce5-bc41-0b6f6b1d10e3', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-11T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'Debit', N'Cash', N'', N'', N'', N'Sardar', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-15T04:34:53.763' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'953d6576-5636-40cf-b40b-0e94fbc814df', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-06T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:56:34.410' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b6d2f54c-696d-4d29-abc4-10ffe694d5e6', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-13T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:05:40.897' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'069fdf4c-3838-47ca-a3b0-16b0a55e6188', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-04-01T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:01:52.497' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'340048bf-3c8b-431c-b2c0-16c41c0a8342', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-05T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-03T16:04:10.623' AS DateTime), CAST(N'2019-10-03T16:22:28.170' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1b66a672-3d94-4da4-8ce8-1952b2a7888c', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-07-30T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'bank of baroda', N'Gamdi', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:13:38.743' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1e799809-de32-4ca0-ae29-1a7d8d1dd659', N'358d08eb-0718-45c8-ae85-1d68c1c8d89c', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', CAST(N'2019-10-19T00:00:00.000' AS DateTime), CAST(1000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'fdfgdggd', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', CAST(N'2019-10-19T14:57:11.863' AS DateTime), CAST(N'2019-10-19T14:57:24.900' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'0c36fb9c-dc8f-4df7-bf2a-273380129684', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-17T00:00:00.000' AS DateTime), CAST(130000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'BERAR', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:27:20.370' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b8a0bc4e-adfc-4120-a224-29d96391b9c0', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-04-11T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:18:13.857' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'44d8ff01-f89d-475b-a8c8-2a430069966b', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-18T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-30T08:46:50.790' AS DateTime), CAST(N'2019-10-30T08:48:23.523' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a811081c-8938-4c82-9359-2a836506e3d7', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(900000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'State bank Of  India', N'Niruben Arvindbhai Prajapati', N'Dr. Chirag Patel NEFT', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T06:02:33.427' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3d8a738f-0ab3-4a75-b484-2cfaeac6876e', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-12T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T16:59:20.253' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a45d6663-fe6f-4891-9872-322578b0ea22', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-13T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:05:26.443' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2e2262b9-55fc-452f-aa28-32e5e11035bd', N'56dd90d5-c0d3-4ecd-8470-46b8f5c8f84b', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-09-01T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'25508', N'hdfc benk', N'anand', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T16:44:35.587' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ecb6548a-a00b-4e2d-b640-336cd7fc4205', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-13T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:05:09.493' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b4b5f024-a40e-40e6-ad94-33d9e4d82ad2', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-11T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'059874', N'State bank Of  India', N'Vijaybhai Prajapati', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:53:25.757' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'56d27e87-78f7-44aa-817d-37b4fc6b31b2', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-19T00:00:00.000' AS DateTime), CAST(500.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-19T15:05:11.313' AS DateTime), CAST(N'2019-10-19T15:05:37.583' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ff1b5a82-f858-4578-806a-3a3458d2f732', N'6b3893d4-249e-489e-afff-d861db175943', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-18T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-29T12:46:39.403' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'50a4f2be-b8a2-4c71-8104-3a9fba23f81b', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-08T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:05:05.817' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'233bbedb-faba-491a-8f14-3b9977e308bc', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-13T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:11:13.933' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5a88a2d8-f713-4e1b-bee5-40daf0a6ad81', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-31T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'RTGS', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:24:50.810' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'db9dc634-820c-4f13-b04a-42b55d40b2b6', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-26T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, N'Vijaybhai Prajapati', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:51:03.593' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'67530da0-ad06-4268-8e3d-46b216f5c235', N'6b3893d4-249e-489e-afff-d861db175943', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-08-13T00:00:00.000' AS DateTime), CAST(113000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-09T06:46:36.490' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'9afbf124-123a-48f4-961e-47348ce54a52', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-07T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:20:27.327' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1bfc7b60-fcad-46d2-a2dd-4909bcd979b1', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-08T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:06:53.243' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'793fd5ec-486b-474b-b868-4f4705a83713', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'Rameshbhai ', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:12:49.583' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dd42b186-3f35-49d9-8a24-521259b8c4f7', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-18T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:01:02.537' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'8e306cd5-9e92-43d8-93af-53051130222b', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(1500000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'Rajanbhai / Anilbhai', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T05:48:08.237' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'e627bfd4-af67-449c-8d52-53806d6670a9', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:13:56.617' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'77c8494c-fed7-4180-971a-576483fd7952', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-04T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:02:33.547' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f8338050-10f3-4ad5-9a88-57ab8769744e', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2018-12-07T00:00:00.000' AS DateTime), CAST(816111.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T16:51:14.023' AS DateTime), CAST(N'2019-10-08T14:53:31.003' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ab041cac-7c30-4bd6-9fed-58c1f926339a', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-23T00:00:00.000' AS DateTime), CAST(160000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:35:10.837' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd26081a0-11ba-4b7a-a844-5d4e32421938', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2018-12-07T00:00:00.000' AS DateTime), CAST(816111.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:03:16.603' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a1b768b1-fb97-41a5-a89f-600ef9f35a07', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-01T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:07:06.703' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd2fc1807-b22f-43c6-8a90-6472689a6443', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-12T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:08:22.267' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3a18a2e6-1e42-4db2-83b6-6689396f30ff', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-10T00:00:00.000' AS DateTime), CAST(36000.00 AS Decimal(18, 2)), N'Debit', N'Cash', N'', N'', N'', N'Khodan', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-30T05:14:17.250' AS DateTime), CAST(N'2019-10-30T05:15:04.347' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'08ebdbb0-25e6-4024-bdbf-726201caf5d7', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-22T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:07:04.970' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c7c4d051-8d88-4827-b38c-7349df0ab49b', N'6b3893d4-249e-489e-afff-d861db175943', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-09-20T00:00:00.000' AS DateTime), CAST(113000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-09T06:46:58.713' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5ebfcb8e-4511-4857-b0b4-751c5e1f2b8e', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-18T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:58:24.040' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'601ec2a8-e756-4f6f-a9bd-7659105f1cbb', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2018-12-24T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, N'Kuldip R Solanki', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:50:12.057' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5e00815d-919c-4c01-af3b-76dc6dd560c3', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-16T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'bank of baroda', N'Vijaybhai Prajapati', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T05:44:02.293' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5b25082b-7a53-45c7-afff-76e74a96210e', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-06-25T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'bank of baroda', N'Gamdi', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-08T17:09:19.960' AS DateTime), CAST(N'2019-10-08T17:10:26.167' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'196d30fd-8c6f-44ef-bd8c-77083732d4cf', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-17T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:25:27.163' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c47cbbfe-86f2-46ce-9d23-793d6c77e4fb', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-06T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:08:11.957' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c3a396c7-a01d-4542-8514-7c1998e63bce', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-31T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:23:14.067' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'89bbfb41-65dd-4e2e-9ed6-7f34a64d290b', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-19T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:59:22.713' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'85e33cac-7fb6-4111-8ae5-7f560650301f', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-05T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T16:52:52.987' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'cc4a3639-64fd-4a59-991e-865e0ed75714', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-15T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:21:01.813' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'34a50c71-9219-43d7-bc16-867a6a89db1c', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-08T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:09:07.960' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'aed29e44-c3b5-49c1-a4c5-879ab42459df', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-09-12T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:11:11.773' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b3982f7b-20c3-440a-86ba-8d0174c8be33', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-09-27T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'for salary', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-09T06:30:13.990' AS DateTime), CAST(N'2019-10-09T06:30:41.583' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'9f3fde70-0669-4033-af20-9182fb65896c', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-08-31T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:14:58.130' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a77d17f2-09e0-418e-89c4-94728d9da3cf', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-05T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:19:03.887' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'9e103035-3d94-4578-bd20-983844026dc0', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-01T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T17:38:53.250' AS DateTime), CAST(N'2019-10-22T04:45:25.457' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f8396311-f31b-483d-b0fb-997e5ad54cd8', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-14T00:00:00.000' AS DateTime), CAST(250000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:09:49.917' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'85c011bb-ee14-4c21-9239-9990f14f7fc6', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-27T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:09:58.820' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a78a6af9-0835-428b-a496-9a5f5233a00e', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-18T00:00:00.000' AS DateTime), CAST(1500.00 AS Decimal(18, 2)), N'Debit', N'Cash', N'', N'', N'', N'Vijay sadiya', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-18T13:28:52.033' AS DateTime), CAST(N'2019-10-30T14:45:59.437' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f2a130fb-b5d5-425a-ad0a-9ab5f5c3b1be', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-18T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-30T09:00:21.330' AS DateTime), CAST(N'2019-10-30T09:04:06.503' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5ef39a08-c1c5-499b-9ea1-9b5dc18dbf71', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-15T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:21:53.000' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1dc40e43-42ef-4b4b-b514-a190ef83c6b7', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-23T00:00:00.000' AS DateTime), CAST(140000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'BERAR', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:36:29.180' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f70e841f-bf15-49a5-9230-aa619b639bcc', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2018-12-22T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-30T08:46:20.430' AS DateTime), CAST(N'2019-10-30T08:48:29.117' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'56d8c901-d669-439c-810b-ade25e26d3e2', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-25T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'890683', NULL, N'Vijaybhai Prajapati', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:55:40.017' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'6994003a-e2c0-4094-a936-b21f80a19f72', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-07-14T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'bank of baroda', N'Gamdi', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:12:41.013' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c957fc8c-095d-49a9-91da-b3ee3e2ca817', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-23T00:00:00.000' AS DateTime), CAST(160000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T17:31:37.883' AS DateTime), CAST(N'2019-10-02T17:57:50.267' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'776670c9-1007-420f-81cc-b7665d6a578d', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'b88bdc97-ed8e-4699-86cd-fe72a7a2699f', CAST(N'2019-10-13T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-30T05:11:17.447' AS DateTime), CAST(N'2019-10-30T05:11:43.010' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f3288abe-0576-4c1c-84bb-b89a9d8b6271', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-04-11T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:09:52.903' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'deed2c2e-d124-4a23-8f7b-bcb0f03ed991', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'Bank Of India', N'Jitubhai Prajapati', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:15:31.777' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'cc0bba36-8209-4759-8770-bdf45db09d2d', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-18T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:08:42.363' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'72756a5b-0d73-4ebf-ba1c-bfb7e3909fef', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-17T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'BERAR', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:28:19.200' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'7344a464-2ab5-41d8-8437-bff47dc478a7', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-20T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:08:58.987' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'6f31c58a-b272-4093-8246-c0a88dce9e27', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(140000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'Momay', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-26T16:44:21.703' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'681bd7e7-5108-4a42-b1a6-c2a45312f524', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-17T00:00:00.000' AS DateTime), CAST(180000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:05:53.697' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'291d6d25-8e16-4266-9793-c2ae513ce53a', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-23T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'000196', N'bank of baroda', N'Arvind Prajapati', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T05:45:44.623' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f252c183-c34a-42d2-9df1-c45f75ee9823', N'05d4f2b8-1e22-456e-925f-b99d641fb826', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-11-10T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'બેન્ક ઓફ બરોડા', N'અરવિંદભાઈ બાબુભાઈ પ્રજાપતિ', N'બેરર ચેક', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-11-12T06:22:55.007' AS DateTime), CAST(N'2019-11-15T11:03:34.860' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5f9d24ff-cac1-43fe-b060-c5d31c0c5584', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-04-11T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T16:58:58.583' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'36b9322c-8f44-425d-b871-c74a296e0020', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'b88bdc97-ed8e-4699-86cd-fe72a7a2699f', CAST(N'2019-09-13T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'13-09-2019', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-09T06:31:21.130' AS DateTime), CAST(N'2019-10-10T00:57:51.607' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b506d0ab-3526-4214-a0a0-c74a37fb735a', N'0b466908-441f-48a0-ac10-844b1f2777e0', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(60000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'000014', N'HDFC BENK', N'Kamlesh', N'Momay', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-26T16:45:28.047' AS DateTime), CAST(N'2019-10-26T16:46:23.207' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd9b33851-6c82-4d32-a284-c820a0eeab31', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-09-12T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, N'bank of baroda', N'Gamdi', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-08T17:15:37.950' AS DateTime), CAST(N'2019-10-15T17:40:43.987' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ac22f4e6-05a7-4ed3-8fd7-cba729758bf8', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-06-18T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:06:25.557' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'53015522-f81a-4985-b7dd-ce4b1daae84c', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-20T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:06:03.523' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c769b949-d863-4939-8453-d19190caf4cb', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-02-28T00:00:00.000' AS DateTime), CAST(150000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T16:53:51.700' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd710c718-84ba-4e05-9b56-d4d22d6c85db', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-07-15T00:00:00.000' AS DateTime), CAST(1000000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:43:22.543' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ae273b9d-f923-494b-955e-d52dec202c99', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-07T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-03T16:04:52.220' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'99075081-04db-4db5-8da1-d6506ecf4d8d', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-19T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T16:56:01.410' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'fb9ad175-ef1d-42ee-97c7-d6e94418e48b', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-07T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T16:56:10.483' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'95d49069-6340-4461-b9b5-d731c7a8f834', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-09-05T00:00:00.000' AS DateTime), CAST(300000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, N'Rajanbhai Home', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:53:55.580' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'19690b18-0e2b-4cc9-a2d4-d8133bb2592c', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-17T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T08:57:51.493' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1e1d39e4-c813-42ac-ba7f-d8b0c4772c8c', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-05-09T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:04:33.413' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'95d44ca6-0fae-45ba-acab-de2b4df8cad4', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-04-11T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', NULL, NULL, NULL, N'Jatinbhai BERAR', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:02:09.980' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f95153d8-a870-4619-b89d-e17676f35834', N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-09-18T00:00:00.000' AS DateTime), CAST(400000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, N'Rajanbhai', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-02T17:55:32.507' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'774ecde3-9cda-43ab-9145-e5010183dc37', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-09-26T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:11:34.820' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'7d22348f-193b-45b2-8bf4-ec745b342155', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-16T00:00:00.000' AS DateTime), CAST(30000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:13:12.257' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'99d952e7-8e16-4606-9ac7-ef15db824f0e', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-01-07T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', NULL, NULL, NULL, NULL, 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T16:53:46.440' AS DateTime), CAST(N'2019-10-03T16:20:21.353' AS DateTime))
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd1f6c731-8051-46e6-b4db-f0638d299ed1', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-03-15T00:00:00.000' AS DateTime), CAST(400000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:00:17.673' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ebb9efae-a00c-4894-97d4-f1f04b766d37', N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'3f408403-a109-457e-9c68-36769cdff843', CAST(N'2019-05-05T00:00:00.000' AS DateTime), CAST(200000.00 AS Decimal(18, 2)), N'Credit', N'Cheque', N'000045', N'bank of baroda', N'gamdi', N'account pay', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-08T17:03:48.927' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5ec25804-ab75-491b-8f4d-f7abf6d7e280', N'664dc604-6d0c-4389-9e01-6c5366beda59', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-08-23T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:10:43.820' AS DateTime), NULL)
INSERT [dbo].[tbl_ContractorFinance] ([ContractorFinanceId], [SiteId], [UserId], [SelectedDate], [Amount], [CreditOrDebit], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'78bbfa01-73cf-4acb-86fd-fbc33d679d07', N'a983f4e7-11b4-417f-ae41-b6968c10762e', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', CAST(N'2019-10-14T00:00:00.000' AS DateTime), CAST(1000.00 AS Decimal(18, 2)), N'Credit', N'Cash', N'', N'', N'', N'material', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-14T16:59:28.833' AS DateTime), NULL)
GO
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1e723998-d3db-4c49-a374-06125542143d', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-09-28T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:32:22.077' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f3bea6ed-dc8c-47b6-b63c-0be8f63e0fba', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-07-22T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:29:06.613' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'4e6359cb-f8bc-49d4-b4ed-1f9dae77d31e', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-09-25T00:00:00.000' AS DateTime), CAST(1400.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'Helmet', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:38:26.227' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ce386cd8-9aef-4b86-9eb1-25681e493af1', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-27T00:00:00.000' AS DateTime), CAST(31000.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Choras foot', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T04:42:14.660' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'651539ec-82e6-433b-8b00-29d46e1b91f4', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-13T00:00:00.000' AS DateTime), CAST(39000.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Dipat', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T04:41:27.927' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'303065e9-7079-4c70-959b-2a435edbdaaa', N'85349ec7-4999-4ca9-9a9a-ad9506c7ec92', CAST(N'2019-10-27T00:00:00.000' AS DateTime), CAST(9785.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Bill chukte', 1, 0, N'b88bdc97-ed8e-4699-86cd-fe72a7a2699f', NULL, CAST(N'2019-10-29T13:07:07.247' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'49b6fd8c-b03b-4d72-9269-2f29be370730', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-13T00:00:00.000' AS DateTime), CAST(39000.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Pagar', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T00:36:22.163' AS DateTime), CAST(N'2019-10-30T04:40:36.843' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'720397c4-3afc-46c7-b35e-3686248870f1', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(17510.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'1030 sft', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-26T16:30:03.600' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'43e1fca6-0c57-4b06-9163-3f9d7f44d6f4', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-06-04T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'Jetho', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:26:17.877' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ec802a7b-1acb-41f7-9d3a-55bb6b92152c', N'7c406589-2aba-46c9-84c4-d722e3173700', CAST(N'2019-10-21T00:00:00.000' AS DateTime), CAST(1000.00 AS Decimal(18, 2)), NULL, N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'bjjhbjhbj', 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-21T17:10:36.303' AS DateTime), CAST(N'2019-10-21T17:12:27.410' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2b2afa68-6daf-47c5-af34-5b959a444b2e', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-14T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, NULL, 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T16:03:58.737' AS DateTime), CAST(N'2019-10-26T16:28:41.597' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'afee445d-1a48-459e-a332-5c8c29738a87', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2018-12-22T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:19:49.673' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'24b167fd-2afb-42e1-951d-6167825cc059', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-11-12T00:00:00.000' AS DateTime), CAST(5000.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-11-12T06:14:52.530' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd8ef9a51-9d1d-42c8-9f17-64ccf54ad122', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-09-29T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:39:26.303' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'acd6e83d-5a52-4b85-9d7a-66e6e846e666', N'2af9f543-533f-4c52-bb49-95f47a4ab201', CAST(N'2019-10-22T00:00:00.000' AS DateTime), CAST(7975.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Pagar 29 roj', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-22T06:20:02.227' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dcec4dea-2d47-4607-923a-674b923ec0f6', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-25T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Dipat', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T04:39:35.087' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'fe512a41-5b74-4872-bae5-679ee6672c30', N'98d9d07c-959c-4ae6-8f06-37b9910102f2', CAST(N'2019-10-20T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), NULL, N'Debit', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', N'Cash', NULL, NULL, NULL, N'watertank project', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-20T18:07:12.763' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'9c4fbe32-70e7-4958-9dd1-6ca85c675453', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-13T00:00:00.000' AS DateTime), CAST(210000.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Credit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Dipat ane choras fit', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T04:35:24.433' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c05f93cc-fe28-4c9e-a981-6f61d233ca91', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(21600.00 AS Decimal(18, 2)), N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'1200 sft', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-26T16:30:41.617' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'cbe33192-83e1-49da-b92f-7e9119ea9a83', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-03-19T00:00:00.000' AS DateTime), CAST(50000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:23:32.423' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd4eb994d-9f28-423b-a40a-7f371a9bc515', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-06-18T00:00:00.000' AS DateTime), CAST(30000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:27:05.813' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'243b01c0-fab6-4b73-82de-80584c1158a0', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-11-11T00:00:00.000' AS DateTime), CAST(30000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cheque', NULL, N'Hdfc', NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-11-12T06:16:33.490' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'18860d98-295a-4335-b9cc-8747f7424dd4', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-23T00:00:00.000' AS DateTime), CAST(15500.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Alkesh ne', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T16:05:07.463' AS DateTime), CAST(N'2019-10-26T16:28:25.363' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3fb1d3ba-f27f-4b03-b853-8fd30362eef1', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-10-10T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:40:12.197' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'eca6039e-de25-4126-bbd5-972fc0772a1e', N'5b3be80e-f133-4693-8f0a-ddac028f2491', CAST(N'2019-10-20T00:00:00.000' AS DateTime), CAST(100000.00 AS Decimal(18, 2)), NULL, N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'test', 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-20T18:11:19.520' AS DateTime), CAST(N'2019-10-20T18:11:50.387' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dd21583b-b02c-4c23-8bdd-9bec0a75c8be', N'98d9d07c-959c-4ae6-8f06-37b9910102f2', CAST(N'2019-10-25T00:00:00.000' AS DateTime), CAST(50.00 AS Decimal(18, 2)), N'a983f4e7-11b4-417f-ae41-b6968c10762e', N'Debit', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', N'Cash', NULL, NULL, NULL, N'aaaaa', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-25T18:31:12.090' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'6b490970-959f-450a-b990-b01ab09e0e84', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-07-16T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'3f408403-a109-457e-9c68-36769cdff843', N'Cash', NULL, NULL, NULL, N'Jetho', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:28:20.677' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'f2a2024b-6b0e-4a00-87d5-b54da50b2a09', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-23T00:00:00.000' AS DateTime), CAST(2000.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Upad', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T16:04:26.537' AS DateTime), CAST(N'2019-10-26T16:28:35.050' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2404941e-33ac-4230-a8cf-c11fa5b8174e', N'ba3b1e15-3809-454e-bf68-06e383a10005', CAST(N'2019-10-27T00:00:00.000' AS DateTime), CAST(36000.00 AS Decimal(18, 2)), N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Paya khodan', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T05:19:22.193' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1527eec6-0bd5-4aed-9138-c33c60afdf28', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-08-30T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'Jetho', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:30:27.120' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'485e63dd-e6b9-45a6-bb1d-c3d6becd81a6', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(60000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:33:42.737' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a25dfd16-5466-4ce2-b879-c402d1b551f6', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-08-14T00:00:00.000' AS DateTime), CAST(15000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'Raju', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:29:46.273' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'50bc97d1-855f-462e-ba0a-c42ab5508aef', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(21600.00 AS Decimal(18, 2)), N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'1200 sft', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-26T16:30:38.023' AS DateTime), CAST(N'2019-10-26T16:30:59.117' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'511bb5bc-a72b-4340-ac23-c7e861733e95', N'a940c053-2e74-49a6-9b34-f55a537a4c55', CAST(N'2019-10-26T00:00:00.000' AS DateTime), CAST(7000.00 AS Decimal(18, 2)), N'6b3893d4-249e-489e-afff-d861db175943', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Upad pete', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-26T16:31:57.870' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3cc4d996-5040-48de-bf9e-cabc041dfac7', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-10-24T00:00:00.000' AS DateTime), CAST(40000.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, N'net banking', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:41:11.400' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'667d15ce-41de-4a15-89a1-caea4ae5bd30', N'85349ec7-4999-4ca9-9a9a-ad9506c7ec92', CAST(N'2019-10-18T00:00:00.000' AS DateTime), CAST(9785.00 AS Decimal(18, 2)), N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Credit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Choras foit', 1, 0, N'b88bdc97-ed8e-4699-86cd-fe72a7a2699f', NULL, CAST(N'2019-10-29T13:06:21.200' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'89fe3319-ae47-4632-8838-ce21cc2db870', N'961894b0-b41a-4c17-9fe5-1c6a8199c9b0', CAST(N'2019-11-27T00:00:00.000' AS DateTime), CAST(18000.00 AS Decimal(18, 2)), N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Pagar pete', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-11-02T06:13:24.840' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dab03c66-be76-48c3-b82e-cea38cb3e82b', N'4e3a7ad9-432e-4573-ab30-47780245ec41', CAST(N'2019-10-13T00:00:00.000' AS DateTime), CAST(18000.00 AS Decimal(18, 2)), N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'8 makan nu', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-31T05:06:48.133' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'854d240b-024e-4268-937c-d0dd8360c8cf', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-02-18T00:00:00.000' AS DateTime), CAST(25000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:22:48.513' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'1a8e6769-f891-4a8b-8027-e16f1abfac41', N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', CAST(N'2019-10-23T00:00:00.000' AS DateTime), CAST(36000.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Pgar', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T00:36:57.980' AS DateTime), CAST(N'2019-10-30T04:40:28.570' AS DateTime))
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2b28fdf1-9f9d-40d8-95b2-e454ca0aa4e8', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-10-02T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:32:53.873' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd247916c-212a-49bc-838b-eda9a1918131', N'99fd6b9e-8010-4131-8fcf-de57e679faac', CAST(N'2019-10-22T00:00:00.000' AS DateTime), CAST(13800.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Kanjri pagar', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-22T06:18:17.427' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'c2006930-5e06-4494-ba84-f3413c2e5ecb', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-04-19T00:00:00.000' AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'3f408403-a109-457e-9c68-36769cdff843', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:24:33.797' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'8ed467dc-8759-435f-9aa1-f447379459a6', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-05-14T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:24:58.050' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'7d7ad499-6104-4d47-8873-f9cbc3038b0f', N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', CAST(N'2019-09-13T00:00:00.000' AS DateTime), CAST(30000.00 AS Decimal(18, 2)), N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:37:26.053' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a2983bb8-d1bf-41d2-8213-fa51ef7bebe3', N'dd84c328-468c-44e5-be18-36e5f7695109', CAST(N'2019-09-14T00:00:00.000' AS DateTime), CAST(20000.00 AS Decimal(18, 2)), N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Debit', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'Cash', NULL, NULL, NULL, NULL, 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:31:04.667' AS DateTime), NULL)
INSERT [dbo].[tbl_Finance] ([FinanceId], [PersonId], [SelectedDate], [Amount], [SiteId], [CreditOrDebit], [GivenAmountBy], [PaymentType], [ChequeNo], [BankName], [ChequeFor], [Remarks], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'd4360fd7-6450-4fe7-9e72-ffdd6be8fbb0', N'bd5c7f80-5e6d-48cd-9cde-328f313c5b56', CAST(N'2019-10-20T00:00:00.000' AS DateTime), CAST(2000.00 AS Decimal(18, 2)), NULL, N'Debit', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'Cash', NULL, NULL, NULL, N'Kanjri ', 1, 1, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-21T03:35:34.167' AS DateTime), CAST(N'2019-10-26T13:00:59.860' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_PackageType] ON 

INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (1, N'Free Trial Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (2, N'1 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (3, N'3 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (4, N'6 Month Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (5, N'1 Year Package')
INSERT [dbo].[tbl_PackageType] ([PackageTypeId], [PackageType]) VALUES (6, N'One Time Package')
SET IDENTITY_INSERT [dbo].[tbl_PackageType] OFF
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'8942c1f5-2c35-4b3e-9e15-056a47cafd81', N'Kundan Sentingvada', N'Lambhavel Anand', N'9328085004', N'e4f9d149-c4c0-4428-838a-adcef82fa7307476_XL-G-WH-69.jpg', 1, 0, N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T09:36:54.050' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'ba3b1e15-3809-454e-bf68-06e383a10005', N'Khodan ', NULL, NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-30T05:18:40.940' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'961894b0-b41a-4c17-9fe5-1c6a8199c9b0', N'Rajesh karigar', NULL, NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-11-02T06:12:08.683' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'bd5c7f80-5e6d-48cd-9cde-328f313c5b56', N'Rajesh sabur', N'Limdi
', NULL, N'', 1, 1, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-21T03:25:51.663' AS DateTime), CAST(N'2019-10-26T13:01:09.337' AS DateTime))
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'dd84c328-468c-44e5-be18-36e5f7695109', N'Tinabhai Sariyavada', N'anand', N'9913640561', N'', 1, 0, N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', NULL, CAST(N'2019-10-30T06:08:34.213' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'98d9d07c-959c-4ae6-8f06-37b9910102f2', N'Krutik shah', N'anand', N'9824534914', N'ff5e04e1-3a02-4d5a-8aa5-7fca2d9d335aavatar04.png', 1, 0, N'8727fb85-2667-48ca-a50e-16f1194305be', N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-20T18:06:26.120' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'4e3a7ad9-432e-4573-ab30-47780245ec41', N'P C C FLORING', N'
', NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-31T05:05:55.883' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'2af9f543-533f-4c52-bb49-95f47a4ab201', N'Bachu majur', NULL, NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-22T06:18:52.037' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'85349ec7-4999-4ca9-9a9a-ad9506c7ec92', N'Vijay ', N'Samarkha', N'9586917620', N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-21T03:25:20.617' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'7c406589-2aba-46c9-84c4-d722e3173700', N'Vijay Prajapati', NULL, NULL, N'', 1, 1, N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-21T17:09:51.727' AS DateTime), CAST(N'2019-10-21T17:12:57.083' AS DateTime))
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'5b3be80e-f133-4693-8f0a-ddac028f2491', N'Vijay Prajapati', N'100 foot road, Anand', NULL, NULL, 1, 1, N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-20T18:10:22.283' AS DateTime), CAST(N'2019-10-20T18:12:07.387' AS DateTime))
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'99fd6b9e-8010-4131-8fcf-de57e679faac', N'Raju karigar plaster ', N'Anand', NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-21T03:27:05.163' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a940c053-2e74-49a6-9b34-f55a537a4c55', N'Sardar sentring', N'Godra
', NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-21T03:24:46.257' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'49f3491a-7ec7-454b-be54-f5aefffa34d5', N'Ganga Alkesh ', NULL, NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-21T03:26:29.990' AS DateTime), NULL)
INSERT [dbo].[tbl_Persons] ([PersonId], [PersonFirstName], [PersonAddress], [MobileNo], [PersonPhoto], [IsActive], [IsDeleted], [ClientId], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'3f5aef2d-4a93-41f7-9f5a-fe627a03bb17', N'Dilip karigar', NULL, NULL, N'', 1, 0, N'c1655804-2eec-456b-99cc-699b5a97e5b1', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', NULL, CAST(N'2019-10-21T03:26:10.710' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[tbl_Role] ON 

INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (1, N'Site Owner')
INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (2, N'Admin')
INSERT [dbo].[tbl_Role] ([RoleId], [RoleName]) VALUES (3, N'Staff')
SET IDENTITY_INSERT [dbo].[tbl_Role] OFF
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'358d08eb-0718-45c8-ae85-1d68c1c8d89c', N'Site B', N'This is details of Site B.', N'8727fb85-2667-48ca-a50e-16f1194305be', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-14T16:58:33.737' AS DateTime), NULL)
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'8504e44f-e2d1-43c2-8a87-2af58e770ea1', N'Ratikala society', N'Ruchitbhai Patel', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T16:42:49.560' AS DateTime), CAST(N'2019-10-02T17:11:14.653' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'56dd90d5-c0d3-4ecd-8470-46b8f5c8f84b', N'om arced', N'bharatbhai maheshwari', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 1, 1, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-08T16:41:46.700' AS DateTime), CAST(N'2019-10-15T17:35:50.820' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'664dc604-6d0c-4389-9e01-6c5366beda59', N'Prabotham flat', N'Jatinbhai patel', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T08:47:15.633' AS DateTime), CAST(N'2019-10-02T17:11:46.993' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'0b466908-441f-48a0-ac10-844b1f2777e0', N'Gokuldham - Kanjari', N'Only Labour', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-09T06:27:57.690' AS DateTime), CAST(N'2019-10-26T16:40:52.933' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a134ccc4-3a2c-4fd7-860c-a726587bf8f6', N'Haridham jor vadtal ', N'Only Labour', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-23T06:09:04.453' AS DateTime), CAST(N'2019-10-26T16:41:17.887' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'b6bd986f-dfb8-4926-b7a0-aafb316fc831', N'Vinayak banglow', N'Rajan bhai Talati', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-10-02T08:45:32.280' AS DateTime), CAST(N'2019-10-02T17:12:13.293' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'a983f4e7-11b4-417f-ae41-b6968c10762e', N'Site A', N'This is details of Site A.', N'8727fb85-2667-48ca-a50e-16f1194305be', 1, 0, N'705a3367-ac6c-464a-9b3e-79d0957cd7db', NULL, CAST(N'2019-10-14T16:58:14.517' AS DateTime), NULL)
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'05d4f2b8-1e22-456e-925f-b99d641fb826', N'Ekata complex', N'Rakesh patel', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 1, 0, N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', CAST(N'2019-11-12T06:19:23.413' AS DateTime), CAST(N'2019-11-12T06:20:42.843' AS DateTime))
INSERT [dbo].[tbl_Sites] ([SiteId], [SiteName], [SiteDescription], [ClientId], [IsActive], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [ModifiedDate]) VALUES (N'6b3893d4-249e-489e-afff-d861db175943', N'Kunjanbhai-V V Nagar', N'With Materials', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 1, 0, N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', CAST(N'2019-10-09T06:43:15.200' AS DateTime), CAST(N'2019-10-09T06:43:52.013' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'3f408403-a109-457e-9c68-36769cdff843', N'mehul', N'mehul', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 3, N'Mehul Vandra', NULL, N'7990608491', NULL, 1, 0, CAST(N'2019-10-02T18:04:24.437' AS DateTime), CAST(N'2019-10-07T18:05:10.657' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'99ded87c-ea63-458c-ae0e-4cdabe5ac077', N'kamlesh', N'kamlesh', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 2, N'Kamlesh Prajapati', NULL, N'9824534914', NULL, 1, 0, CAST(N'2019-10-07T17:16:57.630' AS DateTime), NULL)
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'93d8ae08-8ebe-4af0-98f5-5f7ee1239ef2', N'arvind', N'arvind2640', N'41bbb5cf-20ff-4e89-b0d1-9cd6289e7307', 2, N'Arvind Prajapati', N'arvind689@gmail.com', N'9638640840', N'132eebbd-ee12-427f-91bf-72747e7ce43b2625e096-47f6-42e6-964f-409a6263543f.jpg', 1, 0, CAST(N'2019-10-01T18:09:50.067' AS DateTime), CAST(N'2019-10-08T17:53:05.900' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'705a3367-ac6c-464a-9b3e-79d0957cd7db', N'nileshp', N'nileshp', N'8727fb85-2667-48ca-a50e-16f1194305be', 2, N'Nilesh Prajapati', N'nilesh007444@gmail.com', N'9824936252', NULL, 1, 0, CAST(N'2019-10-14T16:57:06.327' AS DateTime), NULL)
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'fc6cf0fd-2304-4016-a95d-b64083febee3', N'admin', N'admin', NULL, 1, N'Site Owner', N'prajapati.nileshbhai@gmail.com', N'9824936252', NULL, 1, 0, CAST(N'2019-09-16T23:36:18.600' AS DateTime), CAST(N'2019-10-01T17:49:53.127' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'2c26c5e4-72f4-4a43-bfcd-e345eff696c3', N'sunil', N'sunil@123', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 2, N'Sunilbhai Prajapati', NULL, N'7779078790', NULL, 1, 0, CAST(N'2019-10-07T16:59:24.067' AS DateTime), CAST(N'2019-10-09T06:24:01.647' AS DateTime))
INSERT [dbo].[tbl_Users] ([UserId], [UserName], [Password], [ClientId], [RoleId], [FirstName], [EmailId], [MobileNo], [UserPhoto], [IsActive], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (N'b88bdc97-ed8e-4699-86cd-fe72a7a2699f', N'ajay', N'ajay@123', N'c1655804-2eec-456b-99cc-699b5a97e5b1', 3, N'Ajay Gohel', NULL, N'8320345509', NULL, 1, 0, CAST(N'2019-10-09T06:22:54.190' AS DateTime), NULL)
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
/****** Object:  StoredProcedure [dbo].[SP_GetPersonsList]    Script Date: 24/11/2019 6:17:21 PM ******/
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
	@ClientId uniqueidentifier
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
		SELECT ISNULL(SUM(finance.Amount),0) 
		FROM tbl_Finance finance
		WHERE finance.PersonId = tbl_Persons.PersonId
		AND finance.CreditOrDebit = 'Credit' and finance.IsActive = 1 and finance.IsDeleted = 0
	) as TotalCreditAmount,
	(
		SELECT ISNULL(SUM(finance.Amount),0) 
		FROM tbl_Finance finance
		WHERE finance.PersonId = tbl_Persons.PersonId
		AND finance.CreditOrDebit = 'Debit' and finance.IsActive = 1 and finance.IsDeleted = 0
	) as TotalDebitAmount,
	null

	FROM tbl_Persons
	WHERE IsActive=1 AND IsDeleted=0 AND ClientId = @ClientId
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
/****** Object:  StoredProcedure [dbo].[SP_GetSiteDetailById]    Script Date: 24/11/2019 6:17:21 PM ******/
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
		PaymentType nvarchar(50),
		ChequeNo nvarchar(50),
		BankName nvarchar(50),
		ChequeFor nvarchar(50),
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
	PaymentType,
	ChequeNo,
	BankName,
	ChequeFor,
	Remarks
	FROM tbl_ContractorFinance finance, tbl_Users users
	WHERE finance.UserId = users.UserId
	AND finance.IsActive=1 and finance.IsDeleted=0 AND SiteId = @SiteId
	ORDER BY finance.SelectedDate DESC

	SELECT 
	ContractorFinanceId,
	UserName,
	SelectedDate,
	Amount,
	CreditOrDebit,
	SiteId,
	PaymentType,
	ChequeNo,
	BankName,
	ChequeFor,
	Remarks

	FROM [dbo].#TempFinance
	ORDER BY SelectedDate DESC

	DROP TABLE [dbo].#TempFinance
	
END


GO
/****** Object:  StoredProcedure [dbo].[SP_GetSitesList]    Script Date: 24/11/2019 6:17:21 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetTodayPartyFinance]    Script Date: 24/11/2019 6:17:21 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetTodayPersonFinance]    Script Date: 24/11/2019 6:17:21 PM ******/
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
		PaymentType	nvarchar(50),
		ChequeNo	nvarchar(50),
		BankName	nvarchar(50),
		ChequeFor	nvarchar(50),
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
	PaymentType,
	ChequeNo,
	BankName,
	ChequeFor,
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
	PaymentType,
	ChequeNo,
	BankName,
	ChequeFor,
	Remarks,
	UserName

	FROM [dbo].#TempFinance

	DROP TABLE [dbo].#TempFinance
	
END


GO
USE [master]
GO
ALTER DATABASE [DB_A4E4E6_myconstruction] SET  READ_WRITE 
GO
