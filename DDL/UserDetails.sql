USE [lms]
GO

/****** Object:  View [dbo].[UserDetails]    Script Date: 25-03-2019 18:53:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




ALTER VIEW [dbo].[UserDetails]
AS
SELECT        Id AS User_Id, UserName2 AS Alternate_User_Name, UserName AS User_Name, IsEnabled, Password, DateCreated AS Date_Created, FName + ISNULL(' ' + MName, '') 
                         + ' ' + LName AS Full_Name, LName + ', ' + FName + ISNULL(' ' + MName, '') AS Full_Name_Format1, IsEnabled AS Is_Enabled, PrefAudio AS Preferred_Audio, 
                         PrefSpeed AS Preferred_Speed, PrefText AS Preferred_Text, UserPerms AS User_Permissions, SettingsPerms AS Settings_Permissions, 
                         CoursePerms AS Course_Permissions, TranscriptPerms AS Transcript_Permissions, CompanyPerms AS Company_Permissions, 
                         ForumPerms AS Forum_Permissions, ComPerms AS Communication_Permissions, ReportsPerms AS Report_Permissions, 
                         AnnouncementPerms AS Announcement_Permission, SystemPerms AS System_Permissions, Email, FName AS First_Name, LName AS Last_Name, 
                         MName AS Middle_Name, Preference, RemoteId AS Remote_Id, ISNetworldId AS ISNetworld_Id, PIIFields AS PII_Fields,
						 City AS City, State, Zip, Phone, Photo, BarcodeHash
FROM            dbo.[User] WITH (NOLOCK)
GO


