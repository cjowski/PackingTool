BEGIN;

CREATE SCHEMA IF NOT EXISTS packing;

CREATE TABLE IF NOT EXISTS packing.list(
	list_id SERIAL PRIMARY KEY,
	list_name VARCHAR(50) NOT NULL,
	list_content JSON NOT NULL,
	deleted BOOLEAN NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	modified_date TIMESTAMP NOT NULL,
	modified_user_id INT NOT NULL
);

COMMIT;