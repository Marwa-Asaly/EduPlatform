USE [master]
GO
/****** Object:  Database [EduPlatform]    Script Date: 1/17/2025 9:44:42 PM ******/
CREATE DATABASE [EduPlatform]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EduPlatform', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EduPlatform.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EduPlatform_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EduPlatform_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EduPlatform] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EduPlatform].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EduPlatform] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EduPlatform] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EduPlatform] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EduPlatform] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EduPlatform] SET ARITHABORT OFF 
GO
ALTER DATABASE [EduPlatform] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EduPlatform] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EduPlatform] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EduPlatform] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EduPlatform] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EduPlatform] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EduPlatform] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EduPlatform] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EduPlatform] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EduPlatform] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EduPlatform] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EduPlatform] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EduPlatform] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EduPlatform] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EduPlatform] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EduPlatform] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EduPlatform] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EduPlatform] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EduPlatform] SET  MULTI_USER 
GO
ALTER DATABASE [EduPlatform] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EduPlatform] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EduPlatform] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EduPlatform] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EduPlatform] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EduPlatform] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EduPlatform] SET QUERY_STORE = ON
GO
ALTER DATABASE [EduPlatform] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EduPlatform]
GO
/****** Object:  Table [dbo].[assignment_submittions]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assignment_submittions](
	[id] [uniqueidentifier] NOT NULL,
	[file] [nvarchar](max) NOT NULL,
	[submitted_at] [datetime] NOT NULL,
	[submitted_by] [uniqueidentifier] NULL,
	[assignment_id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_assignment_submittions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[assignments]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assignments](
	[id] [uniqueidentifier] NOT NULL,
	[title_ar] [nvarchar](200) NOT NULL,
	[title_tr] [nvarchar](200) NOT NULL,
	[content_ar] [nvarchar](max) NOT NULL,
	[content_tr] [nvarchar](max) NOT NULL,
	[deadline] [datetime] NOT NULL,
 CONSTRAINT [PK_assignments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [uniqueidentifier] NOT NULL,
	[title_ar] [nvarchar](200) NOT NULL,
	[title_tr] [nvarchar](200) NOT NULL,
	[photo] [nvarchar](max) NULL,
 CONSTRAINT [PK_categories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[courses]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[courses](
	[id] [uniqueidentifier] NOT NULL,
	[title_ar] [nvarchar](200) NOT NULL,
	[title_tr] [nvarchar](200) NOT NULL,
	[content_ar] [nvarchar](max) NULL,
	[content_tr] [nvarchar](max) NULL,
	[category_id] [uniqueidentifier] NOT NULL,
	[in_home] [bit] NULL,
 CONSTRAINT [PK_courses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[exam_questions]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[exam_questions](
	[id] [uniqueidentifier] NOT NULL,
	[is_multiple_choice] [bit] NOT NULL,
	[question_ar] [nvarchar](max) NOT NULL,
	[question_tr] [nvarchar](max) NOT NULL,
	[exam_id] [uniqueidentifier] NOT NULL,
	[answer_1_ar] [nvarchar](200) NULL,
	[answer_2_ar] [nvarchar](200) NULL,
	[answer_3_ar] [nvarchar](200) NULL,
	[answer_1_tr] [nvarchar](200) NULL,
	[answer_2_tr] [nvarchar](200) NULL,
	[answer_3_tr] [nvarchar](200) NULL,
 CONSTRAINT [PK_exam_questions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[exam_submittions]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[exam_submittions](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[exam_id] [uniqueidentifier] NOT NULL,
	[grade] [int] NOT NULL,
 CONSTRAINT [PK_exam_submittions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[exams]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[exams](
	[id] [uniqueidentifier] NOT NULL,
	[title_ar] [nvarchar](200) NOT NULL,
	[title_tr] [nvarchar](200) NOT NULL,
	[content_ar] [nvarchar](max) NOT NULL,
	[content_tr] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[favorites]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[favorites](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[course_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_favorites] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 1/17/2025 9:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [uniqueidentifier] NOT NULL,
	[first_name] [varchar](200) NOT NULL,
	[last_name] [varchar](200) NOT NULL,
	[email] [varchar](200) NOT NULL,
	[password] [varchar](200) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [EduPlatform] SET  READ_WRITE 
GO
