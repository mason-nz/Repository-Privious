


DELETE FROM dbo.TitleBasicInfo WHERE Name='ϵͳ����'
UPDATE IPTitleInfo set PIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='�Ҹ���') WHERE PIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='������')
DELETE FROM TitleBasicInfo WHERE Name='������'
UPDATE IPTitleInfo set PIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='�����') WHERE PIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='����')
DELETE FROM TitleBasicInfo WHERE Name='����'
UPDATE IPTitleInfo set SubIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='�����' and Type=65005) WHERE SubIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='�������')
DELETE FROM TitleBasicInfo WHERE Name='�������'

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65005) WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B1.Name='����' AND B2.Name='��ȫ��' AND B3.Name='���Ի�'
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='��ȫ��' AND B3.Name='ǳ��' AND B1.Name IN ('�ջ�'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='��ȫ��' AND B3.Name='����' AND B1.Name IN ('�����ݷ���','�ֻ�����'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='��ȫ��' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='��ȫ��' AND B3.Name='����' AND B1.Name='�ƺ����'
)


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='��ȫ��' AND B3.Name='����' 
)

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='��ȫ��' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�ɾ͸�' AND B3.Name='����' 
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�ɾ͸�' AND B3.Name='�ɿظ�' AND B1.Name IN ('����װ��'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='��ȫ��' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='ȷ����' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�ɾ͸�' AND B3.Name='ȷ����' 
)

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='���Ի�' AND B3.Name='��˿��' AND B1.Name IN ('����'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='���Ի�' AND B3.Name='������' AND B1.Name IN ('��Խ��'))

UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='��ȫ��' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�¶�' AND B3.Name='������'  AND B1.Name IN ('����'))

DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�޸�' AND B3.Name='�ɾ͸�' AND B1.Name IN ('�����'))


DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�޸�' AND B3.Name='����' AND B1.Name IN ('������'))


UPDATE IPTitleInfo set SubIP=(SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='��ȡ' and Type=65005) WHERE SubIP in  (SELECT TitleID FROM  dbo.TitleBasicInfo WHERE Name='��ȥ')
DELETE FROM TitleBasicInfo WHERE Name='��ȥ'


DELETE FROM  IPTitleInfo WHERE RecID IN (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='����' AND B3.Name='����' AND B1.Name IN ('����'))


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����/ħ��' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='����' AND B3.Name='����'  AND B1.Name IN ('������Ӱ','��ɫ���'))

UPDATE  TitleBasicInfo SET  Name='����/��ʷ' WHERE Name='����' AND Type=65005


UPDATE  dbo.IPTitleInfo SET PIP=(SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='�Ļ�����' AND type=65003),SubIP= (SELECT TOP 1 TitleID FROM dbo.TitleBasicInfo WHERE Name='����/��ʷ' AND type=65005)  
WHERE  RecID IN  (SELECT I.RecID FROM   dbo.IPTitleInfo I INNER JOIN dbo.TitleBasicInfo B1 ON I.TitleID=B1.TitleID
LEFT JOIN dbo.TitleBasicInfo B2 ON I.PIP=B2.TitleID 
LEFT JOIN dbo.TitleBasicInfo B3 ON I.SubIP=B3.TitleID
WHERE  B2.Name='�Ļ�����' AND B3.Name='��ʷ')

DELETE FROM  TitleBasicInfo WHERE  Name='��ʷ' AND Type=65005






































