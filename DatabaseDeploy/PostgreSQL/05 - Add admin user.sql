BEGIN;

INSERT INTO users.user(
	user_name,
	password_hash,
	email,
	authorized,
	deleted,
	last_login_date,
	created_date,
	created_user_id,
	modified_date,
	modified_user_id
)
SELECT
	'admin',
	'$2a$11$po8Fve/pEheUp5xv4sFb5uV8ircokBARICNZedT/O1JthtFL9Esjm',
	'',
	true,
	false,
	CURRENT_TIMESTAMP,
	CURRENT_TIMESTAMP,
	1,
	CURRENT_TIMESTAMP,
	1
WHERE
    NOT EXISTS (
        SELECT user_id FROM users.user WHERE user_name = 'admin'
    );
	
COMMIT;
	
	