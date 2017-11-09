CREATE TABLE dbo.Release
(
	Id int NOT NULL IDENTITY(1, 1),
	Version nvarchar(30) NOT NULL,
	Description nvarchar(200) NULL,
	CONSTRAINT PK_Release PRIMARY KEY CLUSTERED (Id)
)
