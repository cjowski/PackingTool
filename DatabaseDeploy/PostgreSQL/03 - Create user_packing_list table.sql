BEGIN;

CREATE TABLE IF NOT EXISTS packing.user_packing_list(
	user_packing_list_id SERIAL PRIMARY KEY,
	user_id INT NOT NULL,
	packing_list_id INT NOT NULL,
	deleted BOOLEAN NOT NULL,
	created_date TIMESTAMP NOT NULL,
	created_user_id INT NOT NULL,
	modified_date TIMESTAMP NOT NULL,
	modified_user_id INT NOT NULL,
	FOREIGN KEY (user_id) REFERENCES users.user (user_id),
  	FOREIGN KEY (packing_list_id) REFERENCES packing.packing_list (packing_list_id),
	UNIQUE (user_id, packing_list_id)
);

COMMIT;