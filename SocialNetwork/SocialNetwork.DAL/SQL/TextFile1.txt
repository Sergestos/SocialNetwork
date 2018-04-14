CREATE TABLE [dbo].[User]
(
	[ID] INT IDENTITY(1, 1) PRIMARY KEY,
	[Name] VARCHAR(32) NOT NULL,
	[Surname] VARCHAR(32) NOT NULL,
	[Email] VARCHAR(32),
	[Password] VARCHAR(32),
	[RegistrationDate] DATE,
)

CREATE TABLE [dbo].[Dialog]
(
	[ID] INT IDENTITY(1, 1) PRIMARY KEY,
	[Name] VARCHAR(32) NOT NULL,
	[CreationDate] DATE,
	[MasterID] INT FOREIGN KEY REFERENCES [dbo].[User]([ID])
)

CREATE TABLE [dbo].[UserToDialog]
(
	[ID] INT IDENTITY(1, 1) PRIMARY KEY,
	[UserID] INT FOREIGN KEY REFERENCES [dbo].[User]([ID]),
	[DialogID] INT FOREIGN KEY REFERENCES [dbo].[Dialog]([ID]),
)