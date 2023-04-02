BEGIN;

INSERT INTO users.role(
	role_name,
	created_date,
	created_user_id,
	modified_date,
	modified_user_id
)
SELECT
	'admin',
	CURRENT_TIMESTAMP,
	1,
	CURRENT_TIMESTAMP,
	1
WHERE
    NOT EXISTS (
        SELECT user_id FROM users.user WHERE user_name = 'admin'
    );
	
COMMIT;