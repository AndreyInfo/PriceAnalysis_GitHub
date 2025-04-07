CREATE TABLE "KPFiles"
(
    "Id" SERIAL PRIMARY KEY,
    "Notice" CHARACTER VARYING(250),
    "SupplierId" int4 NOT NULL,
    "SectionId" int4 NOT NULL,
    "DocFilesIdExcel" int4 NULL,
    "DocFilesIdPdf" int4 NULL,
    FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id"),
    FOREIGN KEY ("SectionId") REFERENCES "Sections" ("Id"),
    FOREIGN KEY ("DocFilesIdExcel") REFERENCES "DocFiles" ("Id"),
    FOREIGN KEY ("DocFilesIdPdf") REFERENCES "DocFiles" ("Id")
);