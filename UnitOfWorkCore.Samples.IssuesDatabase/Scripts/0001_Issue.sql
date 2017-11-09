CREATE TABLE dbo.Issue
(
	Id int NOT NULL IDENTITY(1, 1),
	Title nvarchar(200) NOT NULL,
	Description nvarchar(1000) NULL,
	PlannedReleaseId int NULL,
	CONSTRAINT PK_Issue PRIMARY KEY CLUSTERED (Id)
)
