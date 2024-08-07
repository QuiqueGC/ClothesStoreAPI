USE [ClothesStore]
GO
SET IDENTITY_INSERT [dbo].[Colors] ON 

INSERT [dbo].[Colors] ([id], [name]) VALUES (1, N'azul')
INSERT [dbo].[Colors] ([id], [name]) VALUES (2, N'verde')
INSERT [dbo].[Colors] ([id], [name]) VALUES (3, N'amarillo')
INSERT [dbo].[Colors] ([id], [name]) VALUES (4, N'negro')
INSERT [dbo].[Colors] ([id], [name]) VALUES (5, N'blanco')
INSERT [dbo].[Colors] ([id], [name]) VALUES (8, N'verde oscuro')
INSERT [dbo].[Colors] ([id], [name]) VALUES (9, N'azul marino')
INSERT [dbo].[Colors] ([id], [name]) VALUES (10, N'gris')
INSERT [dbo].[Colors] ([id], [name]) VALUES (11, N'gris Gandalf')
INSERT [dbo].[Colors] ([id], [name]) VALUES (12, N'ámbar')
INSERT [dbo].[Colors] ([id], [name]) VALUES (13, N'marrón')
INSERT [dbo].[Colors] ([id], [name]) VALUES (15, N'verde esmeralda')
SET IDENTITY_INSERT [dbo].[Colors] OFF
GO
SET IDENTITY_INSERT [dbo].[Size] ON 

INSERT [dbo].[Size] ([id], [value]) VALUES (1, N'M')
INSERT [dbo].[Size] ([id], [value]) VALUES (2, N'XL')
INSERT [dbo].[Size] ([id], [value]) VALUES (3, N'S')
INSERT [dbo].[Size] ([id], [value]) VALUES (4, N'36')
INSERT [dbo].[Size] ([id], [value]) VALUES (5, N'38')
INSERT [dbo].[Size] ([id], [value]) VALUES (6, N'40')
INSERT [dbo].[Size] ([id], [value]) VALUES (10, N'XS')
INSERT [dbo].[Size] ([id], [value]) VALUES (11, N'42')
INSERT [dbo].[Size] ([id], [value]) VALUES (12, N'44')
SET IDENTITY_INSERT [dbo].[Size] OFF
GO
SET IDENTITY_INSERT [dbo].[Clothes] ON 

INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (5, N'Capa', 3, 3, 10, N'Todo un galán')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (9, N'Abrigo de plumas', 1, 1, 50.89, N'The winter is coming')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (10, N'traje de neopreno', 4, 2, 15.99, N'Que tiemble neptuno!')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (11, N'zapatos', 1, 2, 23.32, N'unos hermosos zapatos')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (12, N'sombrero de copa', 3, 1, 40, N'Y que empiece la magia!')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (14, N'Abrigo de plumas', 5, 4, 50.89, N'The winter is coming')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (18, N'Chaqueta vaquera', 5, 4, 50.89, N'CowBoy')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (19, N'Gabardina', 2, 1, 59.66, N'Perfecto para investigaciones')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (20, N'Chuvasquero', 3, 4, 50.89, N'Ya sabes: en abril...')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (1002, N'Chuvasquero', 3, 1, 50.89, N'Hasta abril...')
INSERT [dbo].[Clothes] ([id], [name], [idColor], [idSize], [price], [description]) VALUES (1003, N'Chuvasquero', 3, 2, 50.89, N'Hasta abril...')
SET IDENTITY_INSERT [dbo].[Clothes] OFF
GO
INSERT [dbo].[ClothesDeleted] ([id], [name], [idColor], [idSize], [price], [description], [dateDeleted]) VALUES (7, N'Abrigo de plumas', 2, 1, 50.89, N'The winter is coming', CAST(N'2024-06-09' AS Date))
GO
