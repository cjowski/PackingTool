BEGIN;

DROP TABLE IF EXISTS packing.user_packing_list;
DROP TABLE IF EXISTS packing.packing_list;
DROP TABLE IF EXISTS users.user_authorization;
DROP TABLE IF EXISTS users.user_role;
DROP TABLE IF EXISTS users.role;
DROP TABLE IF EXISTS users.user;

COMMIT;