BEGIN;

CREATE TABLE IF NOT EXISTS users.user_role(
	user_role_id SERIAL PRIMARY KEY,
	user_id INT NOT NULL,
	role_id INT NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	FOREIGN KEY (user_id) REFERENCES users.user (user_id),
  	FOREIGN KEY (role_id) REFERENCES users.role (role_id),
	UNIQUE (user_id, role_id)
);

COMMIT;