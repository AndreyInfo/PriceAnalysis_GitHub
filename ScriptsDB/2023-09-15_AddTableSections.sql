CREATE TABLE "Sections"
(
    "Id" SERIAL PRIMARY KEY,
    "ProjectId" INT,
    "Name" CHARACTER VARYING(250),
    FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id")
);

INSERT INTO "Sections" ("ProjectId", "Name")
VALUES (3, '�������� ������ 101'), 
(3, '�������� ������ 102'), 
(3, '�������� ������ 103')

CREATE TABLE test
(
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(30),
    Age INTEGER
);

INSERT INTO test ("name", age)
VALUES ('������', 25), ('������', 35)