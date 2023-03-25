
SET NOCOUNT ON

DECLARE @TransactionName VARCHAR(MAX) = 'Add admin user'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN @TransactionName

	DECLARE
		@UserName VARCHAR(20),
		@Password VARCHAR(100)

	SET @UserName = 'admin'
	SET @Password = '123'
	

	IF NOT EXISTS(SELECT 1 FROM dbo.[User] WHERE [Name] = @UserName)
	BEGIN
		
		INSERT INTO dbo.[User]
		(
			[Name],
			[Password],
			CreatedDate,
			CreatedUserID,
			ModifiedDate,
			ModifiedUserID
		)
		VALUES
		(
			@UserName,
			@Password,
			GETDATE(),
			1,
			GETDATE(),
			1
		)

		PRINT 'Added user: ' + @UserName
	END


COMMIT TRAN @TransactionName
