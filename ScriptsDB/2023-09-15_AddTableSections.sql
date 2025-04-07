CREATE TABLE "Sections"
(
    "Id" SERIAL PRIMARY KEY,
    "ProjectId" INT,
    "Name" CHARACTER VARYING(250),
    FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id")
);

INSERT INTO "Sections" ("ProjectId", "Name")
VALUES (3, 'Тестовыё раздел 101'), 
(3, 'Тестовыё раздел 102'), 
(3, 'Тестовыё раздел 103')

CREATE TABLE test
(
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(30),
    Age INTEGER
);

INSERT INTO test ("name", age)
VALUES ('Сергей', 25), ('Виктор', 35)