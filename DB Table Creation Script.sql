--Create script for the only table is below. Geography is provided by the File repository which uses the CSV files.

CREATE TABLE [dbo].[UserVisits] (
    [VisitId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]  INT              NOT NULL,
    [Created] DATETIME         NOT NULL,
    [CityId]  INT              NOT NULL,
    [StateId] TINYINT          NOT NULL,
    PRIMARY KEY CLUSTERED ([VisitId] ASC)
);

