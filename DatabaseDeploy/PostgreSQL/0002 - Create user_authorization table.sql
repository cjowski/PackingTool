BEGIN;

CREATE TABLE IF NOT EXISTS users.user_authorization(
	user_id INT PRIMARY KEY,
	password_hash VARCHAR(200) NOT NULL,
	authorized BOOLEAN NOT NULL,
	required_new_password BOOLEAN NOT NULL DEFAULT FALSE,
	last_login_date TIMESTAMP NOT NULL,
	login_attempts_left INT NOT NULL DEFAULT 10,
	last_login_attempt_date TIMESTAMP NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	modified_date TIMESTAMP NOT NULL,
	modified_user_id INT NOT NULL,
	FOREIGN KEY (user_id) REFERENCES users.user (user_id)
);

COMMIT;