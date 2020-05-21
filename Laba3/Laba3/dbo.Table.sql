CREATE TABLE [dbo].[Users] (
    [Id]       INT        IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR(50) NOT NULL,
    [PassHash] NVARCHAR(50) NOT NULL,
    [Name]     NVARCHAR(50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

