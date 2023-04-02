DO $$
DECLARE
   _user_id INTEGER;
   _role_id INTEGER;
   _user_name VARCHAR(50) := 'admin';
   _role_name VARCHAR(50) := 'admin';
   
BEGIN
   
_user_id = (SELECT user_ID FROM users.user WHERE user_name = _user_name LIMIT 1);
_role_id = (SELECT role_id FROM users.role WHERE role_name = _role_name LIMIT 1);

IF _user_id IS NULL THEN

	INSERT INTO users.user(
		user_name,
		email,
		deleted,
		created_date,
		created_user_id,
		modified_date,
		modified_user_id
	)
	SELECT
		_user_name,
		'',
		false,
		CURRENT_TIMESTAMP,
		1,
		CURRENT_TIMESTAMP,
		1
	RETURNING
		user_ID INTO _user_id;
		
END IF;


INSERT INTO users.user_authorization(
	user_id,
	password_hash,
	authorized,
	last_login_date,
	login_attempts_left,
	last_login_attempt_date,
	created_date,
	created_user_id,
	modified_date,
	modified_user_id
)
SELECT
	_user_id,
	'$2a$11$po8Fve/pEheUp5xv4sFb5uV8ircokBARICNZedT/O1JthtFL9Esjm',
	true,
	CURRENT_TIMESTAMP,
	1,
	CURRENT_TIMESTAMP,
	CURRENT_TIMESTAMP,
	1,
	CURRENT_TIMESTAMP,
	1
WHERE
    NOT EXISTS (
        SELECT user_id FROM users.user_authorization WHERE user_id = _user_id
    );
	
	
INSERT INTO users.user_role(
	user_id,
	role_id,
	created_date,
	created_user_id
)
SELECT
	_user_id,
	_role_id,
	CURRENT_TIMESTAMP,
	1
WHERE
    NOT EXISTS (
        SELECT user_id FROM users.user_role WHERE user_id = _user_id
    );
	
COMMIT;
END $$;
	