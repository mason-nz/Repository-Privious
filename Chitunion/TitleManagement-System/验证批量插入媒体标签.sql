
SELECT * FROM dbo.DictInfo WHERE DictType=65
SELECT * FROM  Chitunion_IP2017.dbo.MediaLabelResult WHERE MediaType=14002 AND MediaName=' ¿ºÕº—‘µ'
SELECT * FROM Chitunion_IP2017.dbo.ResultLabel WHERE MediaResultID=2 AND TYPE=65001
SELECT * FROM Chitunion_IP2017.dbo.ResultLabel WHERE MediaResultID=2 AND TYPE=65002
SELECT * FROM Chitunion_IP2017.dbo.ResultLabel WHERE MediaResultID=2 AND TYPE=65003
SELECT * FROM Chitunion_IP2017.dbo.ResultLabel WHERE MediaResultID=2 AND TYPE=65006

SELECT R.TitleID, TB1.Name AS IP,M.TitleID, TB2.Name AS SonIp FROM  Chitunion_IP2017.dbo.MediaResultIPSubLabe M LEFT JOIN ResultLabel R ON M.ResultLabelID=R.ResultLabelID  LEFT JOIN TitleBasicInfo TB1 ON  R.TitleID =TB1.TitleID LEFT JOIN TitleBasicInfo TB2 ON M.TitleID=TB2.TitleID   WHERE M.MediaResultID=2

SELECT R.TitleID, TB1.Name AS IP,M.TitleID, TB2.Name AS SonIp,L.TitleID AS labelID,L.Name AS labelName FROM MediaResultSonIPLabel L 
LEFT JOIN   Chitunion_IP2017.dbo.MediaResultIPSubLabe M  ON L.ResultSubIPID=M.ResultSubIPID 
LEFT JOIN ResultLabel R ON M.ResultLabelID=R.ResultLabelID  LEFT JOIN TitleBasicInfo TB1 ON  R.TitleID =TB1.TitleID 
LEFT JOIN TitleBasicInfo TB2 ON M.TitleID=TB2.TitleID 
 WHERE M.MediaResultID=2


SELECT * FROM Chitunion2017.dbo.Media_BasePCAPP --317
select * FROM  dbo.TitleBasicInfo  --3745
select * FROM  dbo.IPTitleInfo  --6729
SELECT * FROM  Chitunion_IP2017.dbo.MediaLabelResult
SELECT * FROM  Chitunion_IP2017.dbo.MediaResultIPSubLabe
SELECT * FROM  Chitunion_IP2017.dbo.MediaResultSonIPLabel
