CREATE TABLE "Projects"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" CHARACTER VARYING(250),
    "FullName" CHARACTER VARYING(250),
    "Number" CHARACTER VARYING(100)
);

select * from "Projects" ;

INSERT INTO "Projects" ("Name", "FullName", "Number")
VALUES ('�������� ������ 100', '������ ��������� ������� 100', '100-�'), 
('�������� ������ 200', '������ ��������� ������� 200', '200-�'),
('�������� ������ 300', '������ ��������� ������� 300', '300-�')