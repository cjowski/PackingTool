BEGIN;

CREATE SCHEMA IF NOT EXISTS users;

CREATE TABLE IF NOT EXISTS users.user(
	user_id SERIAL PRIMARY KEY,
	user_name VARCHAR(50) UNIQUE NOT NULL,
	password_hash VARCHAR(200) NOT NULL,
	email VARCHAR(50) NOT NULL,
	authorized BOOLEAN NOT NULL,
	deleted BOOLEAN NOT NULL,
	last_login_date TIMESTAMP NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	modified_date TIMESTAMP NOT NULL,
	modified_user_id INT NOT NULL
);

COMMIT;