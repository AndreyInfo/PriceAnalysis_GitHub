CREATE TABLE "Suppliers"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" CHARACTER VARYING(250),
    "Inn" CHARACTER VARYING(10),
    "Kpp" CHARACTER VARYING(9),
    "WebSite" CHARACTER VARYING(250)
);

INSERT INTO "Suppliers" ("Name", "Inn", "Kpp", "WebSite")
VALUES ('���������', '12121212', '41414141', 'stroi.ru'), 
('���������', '23232323', '85858585', 'vert.ru'), 
('��������', '56565656', '74747474', 'petrovich.ru')