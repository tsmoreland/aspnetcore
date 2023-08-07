BEGIN TRANSACTION;

INSERT INTO "Departments" ("Id", "Name")
VALUES (1, 'Finance');
INSERT INTO "Departments" ("Id", "Name")
VALUES (2, 'Sales');
INSERT INTO "Departments" ("Id", "Name")
VALUES (3, 'Marketing');
INSERT INTO "Departments" ("Id", "Name")
VALUES (4, 'HR');
INSERT INTO "Departments" ("Id", "Name")
VALUES (5, 'IT');
INSERT INTO "Departments" ("Id", "Name")
VALUES (6, 'Research and Development');

INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "LastName")
VALUES (1, 1, 'John', 'Smith');
INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "LastName")
VALUES (2, 2, 'Jessica', 'Jones');

INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "IsDeveloper", "LastName")
VALUES (3, 3, 'Tony', 1, 'Stark');

INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "LastName")
VALUES (4, 4, 'Edward', 'Enigma');
INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "LastName")
VALUES (5, 5, 'Brenda', 'Moore');

INSERT INTO "Employees" ("Id", "DepartmentId", "FirstName", "IsDeveloper", "LastName")
VALUES (6, 6, 'Bruce', 1, 'Wayne');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221128125150_SeedData', '7.0.0');

COMMIT;

