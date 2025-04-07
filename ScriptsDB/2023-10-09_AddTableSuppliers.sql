CREATE TABLE "Suppliers"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" CHARACTER VARYING(250),
    "Inn" CHARACTER VARYING(10),
    "Kpp" CHARACTER VARYING(9),
    "WebSite" CHARACTER VARYING(250)
);

INSERT INTO "Suppliers" ("Name", "Inn", "Kpp", "WebSite")
VALUES ('СтройГрад', '12121212', '41414141', 'stroi.ru'), 
('Вертикаль', '23232323', '85858585', 'vert.ru'), 
('Петрович', '56565656', '74747474', 'petrovich.ru')