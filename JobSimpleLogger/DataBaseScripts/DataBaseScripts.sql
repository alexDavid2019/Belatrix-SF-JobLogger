CREATE TABLE [dbo].[Log] (
    [Message]			NVARCHAR (MAX) NOT NULL,
    [SeverityLevelCode]	VARCHAR(10)    NOT NULL,
    [CreationDate] 		DATETIME       NOT NULL
);