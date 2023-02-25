
SET NOCOUNT ON

DECLARE @TransactionName VARCHAR(MAX) = 'Create PackingList table'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN @TransactionName


	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.tables WHERE table_schema = 'dbo' AND table_name = 'PackingList')
	BEGIN
		CREATE TABLE dbo.PackingList(
            PackingListID				INT IDENTITY(1, 1)			NOT NULL,
			[Name]						VARCHAR(50)					NOT NULL,
			[JSON]						VARCHAR(MAX)				NOT NULL,
			Deleted						BIT							NOT NULL,
			CreatedDate					DATETIME					NOT NULL,
			CreatedUserID				INT							NOT NULL,
			ModifiedDate				DATETIME					NOT NULL,
			ModifiedUserID				INT							NOT NULL
		
			CONSTRAINT PK_PackingList		PRIMARY KEY CLUSTERED(PackingListID)
		)
		PRINT 'Created table dbo.PackingList'
	END
	ELSE PRINT '[ERROR] Table dbo.PackingList already exists'


COMMIT TRAN @TransactionName
