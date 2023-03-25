
SET NOCOUNT ON

DECLARE @TransactionName VARCHAR(MAX) = 'Drop tables'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN @TransactionName

	
	DROP TABLE dbo.UserPackingList
	DROP TABLE dbo.[User]
	DROP TABLE dbo.PackingList


COMMIT TRAN @TransactionName
