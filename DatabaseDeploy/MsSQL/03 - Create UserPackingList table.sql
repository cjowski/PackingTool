
SET NOCOUNT ON

DECLARE @TransactionName VARCHAR(MAX) = 'Create UserPackingList table'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN @TransactionName

	
	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.tables WHERE table_schema = 'dbo' AND table_name = 'UserPackingList')
	BEGIN
		CREATE TABLE dbo.UserPackingList(
            UserPackingListID			INT IDENTITY(1, 1)			NOT NULL,
            UserID						INT							NOT NULL,
            PackingListID				INT							NOT NULL,
			CreatedDate					DATETIME					NOT NULL,
			CreatedUserID				INT							NOT NULL
		
			CONSTRAINT PK_UserPackingList				PRIMARY KEY CLUSTERED(UserPackingListID),
			CONSTRAINT FK_UserPackingList_User			FOREIGN KEY(UserID) REFERENCES dbo.[User](UserID),
			CONSTRAINT FK_UserPackingList_PackingList	FOREIGN KEY(PackingListID) REFERENCES dbo.[PackingList](PackingListID),
			CONSTRAINT UK_UserPackingList				UNIQUE(UserID, PackingListID)
		)
		PRINT 'Created table dbo.UserPackingList'
	END
	ELSE PRINT '[ERROR] Table dbo.UserPackingList already exists'


COMMIT TRAN @TransactionName
