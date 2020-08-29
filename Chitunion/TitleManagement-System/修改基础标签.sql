


DELETE FROM dbo.TitleBasicInfo WHERE Name='系统已有'
UPDATE IPTitleInfo set PIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='幸福感') WHERE PIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='辛福感')
DELETE FROM TitleBasicInfo WHERE Name='辛福感'
UPDATE IPTitleInfo set PIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='逃离感') WHERE PIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='逃离')
DELETE FROM TitleBasicInfo WHERE Name='逃离'
UPDATE IPTitleInfo set SubIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='满足感' and Type=65005) WHERE SubIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='，满足感')
DELETE FROM TitleBasicInfo WHERE Name='，满足感'

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='自由' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='个性' AND type=65005) WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B1.Name='定制' AND B2.Name='安全感' AND B3.Name='个性化'
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='安全感' AND B3.Name='浅薄' AND B1.Name IN ('诱惑'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='安全感' AND B3.Name='轻松' AND B1.Name IN ('大数据分析','手机操作'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='安全感' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='保障' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='安全感' AND B3.Name='轻松' AND B1.Name='酒后代驾'
)


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='自由' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='热情' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='安全感' AND B3.Name='热情' 
)

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='安全感' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='保障' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='成就感' AND B3.Name='保障' 
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='成就感' AND B3.Name='可控感' AND B1.Name IN ('汽车装潢'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='安全感' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='确定感' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='成就感' AND B3.Name='确定感' 
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='个性化' AND B3.Name='粉丝量' AND B1.Name IN ('娱乐'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='个性化' AND B3.Name='好奇心' AND B1.Name IN ('优越感'))

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='安全感' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='保障' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='孤独' AND B3.Name='不信任'  AND B1.Name IN ('匿名'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='修复' AND B3.Name='成就感' AND B1.Name IN ('满足感'))


DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='修复' AND B3.Name='焦虑' AND B1.Name IN ('不满足'))


UPDATE IPTitleInfo set SubIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='进取' and Type=65005) WHERE SubIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='进去')
DELETE FROM TitleBasicInfo WHERE Name='进去'


DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='娱乐' AND B3.Name='愉悦' AND B1.Name IN ('糗事'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='自由' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='个性/魔性' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='自由' AND B3.Name='个性'  AND B1.Name IN ('人物摄影','特色设计'))

UPDATE  TitleBasicInfo SET  Name='复古/历史' WHERE Name='复古' AND Type=65005


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='文化艺术' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='复古/历史' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='文化艺术' AND B3.Name='历史')

DELETE FROM  TitleBasicInfo WHERE  Name='历史' AND Type=65005






































