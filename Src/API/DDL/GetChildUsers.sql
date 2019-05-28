-------------****Scripts to create function to get the child users****-------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[getChildUsers] (@userId INT)
RETURNS TABLE
AS
RETURN


WITH tableR (SupervisorId, UserId)
AS
(
	 

    SELECT e.SupervisorId, e.UserId
    FROM dbo.Supervisor AS e   
    WHERE e.SupervisorId =@userId

    UNION ALL

    SELECT e.SupervisorId, e.UserId
    FROM dbo.Supervisor AS e
    INNER JOIN tableR AS d
        ON e.SupervisorId = d.UserId
)

SELECT distinct tableR.UserId AS totalEmployee FROM tableR 
GO


