

SELECT * INTO #tempLabel 
FROM (

SELECT B2.Name AS PIP,B3.Name AS SUBIP ,B1.Name AS Label FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID) A

	SELECT B.PIP,B.SUBIP,LEFT(StuList,LEN(StuList)-1) as hobby FROM (
SELECT PIP,SUBIP,
(SELECT Label+'	' FROM #tempLabel 
  WHERE PIP=A.PIP AND SUBIP=A.SUBIP
  FOR XML PATH('')) AS StuList
FROM #tempLabel A 
GROUP BY PIP,SUBIP
) B ORDER  BY B.PIP,B.SUBIP