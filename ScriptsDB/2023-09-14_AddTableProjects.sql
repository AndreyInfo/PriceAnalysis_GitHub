CREATE TABLE "Projects"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" CHARACTER VARYING(250),
    "FullName" CHARACTER VARYING(250),
    "Number" CHARACTER VARYING(100)
);

select * from "Projects" ;

INSERT INTO "Projects" ("Name", "FullName", "Number")
VALUES ('Тестовый проект 100', 'Пример тестового проекта 100', '100-п'), 
('Тестовый проект 200', 'Пример тестового проекта 200', '200-п'),
('Тестовый проект 300', 'Пример тестового проекта 300', '300-п')