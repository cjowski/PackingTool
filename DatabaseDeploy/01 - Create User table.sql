
SET NOCOUNT ON

DECLARE @TransactionName VARCHAR(MAX) = 'Create User table'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN @TransactionName

	
	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.tables WHERE table_schema = 'dbo' AND table_name = 'User')
	BEGIN
		CREATE TABLE dbo.[User](
            UserID					INT IDENTITY(1, 1)			NOT NULL,
            [UserName]				VARCHAR(20)					NOT NULL,
            PasswordHash			VARCHAR(100)				NOT NULL,
			Email					VARCHAR(40)					NOT NULL,
			CreatedDate				DATETIME					NOT NULL,
			CreatedUserID			INT							NOT NULL,
			ModifiedDate			DATETIME					NOT NULL,
			ModifiedUserID			INT							NOT NULL
		
			CONSTRAINT PK_User		PRIMARY KEY CLUSTERED(UserID)
		)
		PRINT 'Created table dbo.User'
	END
	ELSE PRINT '[ERROR] Table dbo.User already exists'


COMMIT TRAN @TransactionName
