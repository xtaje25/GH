USE [MH]
GO
/****** Object:  Table [dbo].[Tab_MHImg]    Script Date: 2017/08/04 17:24:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tab_MHImg](
	[F_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Name] [nvarchar](50) NOT NULL,
	[F_Img] [nvarchar](200) NOT NULL,
	[F_MHId] [int] NOT NULL,
	[F_Sort] [int] NOT NULL,
	[F_IsEnable] [bit] NOT NULL,
	[F_UserId] [int] NOT NULL,
	[F_CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Tab_MHImg] PRIMARY KEY CLUSTERED 
(
	[F_MHId] ASC,
	[F_Sort] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Tab_MHImg] ON 

INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (1, N'第一章', N'IMG/3/1/1501773272827.jpg', 1, 1, 1, 3, CAST(N'2017-08-03T17:46:27.333' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (2, N'第二集', N'IMG/3/1/1501766321460.jpg', 1, 2, 1, 3, CAST(N'2017-08-03T21:18:41.887' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (3, N'第三集', N'IMG/3/1/1501766801516.jpg', 1, 3, 1, 3, CAST(N'2017-08-03T21:26:41.807' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (4, N'第四章', N'IMG/3/1/1501770082134.jpg', 1, 4, 1, 3, CAST(N'2017-08-03T22:21:22.493' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (5, N'第五章', N'IMG/3/1/1501770183439.jpg', 1, 5, 1, 3, CAST(N'2017-08-03T22:23:03.710' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (6, N'哈哈', N'IMG/3/1/1501770336321.jpg', 1, 6, 1, 3, CAST(N'2017-08-03T22:25:44.570' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (7, N'嘎嘎嘎', N'IMG/3/1/1501770482764.jpg', 1, 7, 1, 3, CAST(N'2017-08-03T22:28:03.130' AS DateTime))
INSERT [dbo].[Tab_MHImg] ([F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]) VALUES (8, N'第八章', N'IMG/3/1/1501812089989.jpg', 1, 8, 1, 3, CAST(N'2017-08-04T10:01:30.987' AS DateTime))
SET IDENTITY_INSERT [dbo].[Tab_MHImg] OFF
ALTER TABLE [dbo].[Tab_MHImg] ADD  CONSTRAINT [DF_Tab_MHImg_F_IsEnable]  DEFAULT ((1)) FOR [F_IsEnable]
GO
ALTER TABLE [dbo].[Tab_MHImg] ADD  CONSTRAINT [DF_Tab_MHImg_F_CreateDate]  DEFAULT (getdate()) FOR [F_CreateDate]
GO
