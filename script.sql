USE [master]
GO
/****** Object:  Database [0118520229]    Script Date: 8/4/2024 6:26:02 PM ******/
CREATE DATABASE [0118520229]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'0118520229', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\0118520229.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'0118520229_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\0118520229_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [0118520229] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [0118520229].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [0118520229] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [0118520229] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [0118520229] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [0118520229] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [0118520229] SET ARITHABORT OFF 
GO
ALTER DATABASE [0118520229] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [0118520229] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [0118520229] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [0118520229] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [0118520229] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [0118520229] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [0118520229] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [0118520229] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [0118520229] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [0118520229] SET  DISABLE_BROKER 
GO
ALTER DATABASE [0118520229] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [0118520229] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [0118520229] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [0118520229] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [0118520229] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [0118520229] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [0118520229] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [0118520229] SET RECOVERY FULL 
GO
ALTER DATABASE [0118520229] SET  MULTI_USER 
GO
ALTER DATABASE [0118520229] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [0118520229] SET DB_CHAINING OFF 
GO
ALTER DATABASE [0118520229] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [0118520229] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [0118520229] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [0118520229] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'0118520229', N'ON'
GO
ALTER DATABASE [0118520229] SET QUERY_STORE = OFF
GO
USE [0118520229]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 8/4/2024 6:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellidos] [nvarchar](50) NOT NULL,
	[DineroCompradoTotal] [int] NOT NULL,
	[DineroCompradoUltimoAño] [int] NOT NULL,
	[DineroCompradoUltimosSeisMeses] [int] NOT NULL,
	[Clave] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Direcciones]    Script Date: 8/4/2024 6:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Direcciones](
	[ID] [int] NOT NULL,
	[IDCliente] [int] NOT NULL,
	[Provincia] [nvarchar](50) NOT NULL,
	[Canton] [nvarchar](50) NOT NULL,
	[Distrito] [nvarchar](50) NOT NULL,
	[PuntoWaze] [nvarchar](50) NOT NULL,
	[URL] [nvarchar](50) NOT NULL,
	[EsCondominio] [bit] NOT NULL,
 CONSTRAINT [PK_Direccion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventarios]    Script Date: 8/4/2024 6:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventarios](
	[ID] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[IDBodega] [int] NOT NULL,
	[FechaIngreso] [date] NOT NULL,
	[FechaVencimiento] [date] NOT NULL,
	[Tipo] [nvarchar](50) NOT NULL,
	[Precio] [int] NOT NULL,
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 8/4/2024 6:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[ID] [int] NOT NULL,
	[IDCliente] [int] NOT NULL,
	[IDInventario] [int] NOT NULL,
	[IDDireccion] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[CostoSinIva] [int] NOT NULL,
	[Costo] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Estado] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Pedido] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reportes]    Script Date: 8/4/2024 6:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reportes](
	[ID] [int] NOT NULL,
	[IDCliente] [int] NOT NULL,
	[MontoTotalPedidos] [int] NOT NULL,
 CONSTRAINT [PK_Reporte] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Direcciones]  WITH CHECK ADD  CONSTRAINT [FK_Direcciones_Clientes] FOREIGN KEY([IDCliente])
REFERENCES [dbo].[Clientes] ([ID])
GO
ALTER TABLE [dbo].[Direcciones] CHECK CONSTRAINT [FK_Direcciones_Clientes]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Clientes] FOREIGN KEY([IDCliente])
REFERENCES [dbo].[Clientes] ([ID])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Clientes]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Direcciones] FOREIGN KEY([IDDireccion])
REFERENCES [dbo].[Direcciones] ([ID])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Direcciones]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Inventarios] FOREIGN KEY([IDInventario])
REFERENCES [dbo].[Inventarios] ([ID])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Inventarios]
GO
ALTER TABLE [dbo].[Reportes]  WITH CHECK ADD  CONSTRAINT [FK_Reporte_Clientes] FOREIGN KEY([IDCliente])
REFERENCES [dbo].[Clientes] ([ID])
GO
ALTER TABLE [dbo].[Reportes] CHECK CONSTRAINT [FK_Reporte_Clientes]
GO
USE [master]
GO
ALTER DATABASE [0118520229] SET  READ_WRITE 
GO

