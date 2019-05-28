USE [lms]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 26-03-2019 18:07:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[Split]
(
@String NVARCHAR(4000)
)
RETURNS TABLE 
AS
RETURN 
(
WITH Split(stpos,endpos) 
AS(
SELECT 0 AS stpos, CHARINDEX(',',@String) AS endpos
UNION ALL
SELECT endpos+1, CHARINDEX(',',@String,endpos+1)
FROM Split
WHERE endpos > 0
)
SELECT 
'Values' = SUBSTRING(@String,stpos,COALESCE(NULLIF(endpos,0),LEN(@String)+1)-stpos)
FROM Split
)
GO


