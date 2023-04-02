BEGIN;

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
	'system',
	'',
	false,
	CURRENT_TIMESTAMP,
	1,
	CURRENT_TIMESTAMP,
	1
WHERE
    NOT EXISTS (
        SELECT user_id FROM users.user WHERE user_name = 'system'
    );
	
COMMIT;
	
	