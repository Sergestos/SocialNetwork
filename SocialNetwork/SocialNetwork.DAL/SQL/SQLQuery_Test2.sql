SELECT * FROM [dbo].[Users]

SELECT * FROM [dbo].[Contents]
SELECT * FROM [dbo].[Dialogs]
SELECT * FROM [dbo].[DialogMembers]

SELECT * FROM [dbo].[Followers]
SELECT * FROM [dbo].[BlackLists]

UPDATE Contents
SET Path = 'F:\Social Network\TestRecordsRepositoty\Content__hc20180508T003542'
WHERE ID = 2;

UPDATE Users
SET SurName = 'The Goel'
WHERE FirstName = 'Thrall'

UPDATE Users
SET IsOthersCanStartChat = 1
WHERE ID < 1005

UPDATE Dialogs
Set Name = 'NewDialog4'
WHERE ID = 24;

UPDATE Contents
Set Extension = '.jpg'
WHERE Extension = '.xml'

DELETE FROM Dialogs
WHERE ID = 36