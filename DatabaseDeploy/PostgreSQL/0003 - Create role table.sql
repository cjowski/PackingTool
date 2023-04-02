BEGIN;

CREATE TABLE IF NOT EXISTS users.role(
	role_id SERIAL PRIMARY KEY,
	role_name VARCHAR(50) UNIQUE NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	modified_date TIMESTAMP NOT NULL,
	modified_user_id INT NOT NULL
);

COMMIT;