-- DROP USER LAFT CASCADE;
CREATE USER LAFT IDENTIFIED BY LAFT DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE temp QUOTA UNLIMITED ON USERS;
GRANT CONNECT TO LAFT;
GRANT CREATE TABLE TO LAFT;
GRANT CREATE SEQUENCE TO LAFT;
GRANT CREATE PROCEDURE TO LAFT;
GRANT CREATE TRIGGER TO LAFT;

