CREATE TABLE "SynchSpecification"
(
    "Id" SERIAL PRIMARY KEY,
    "SectionId" int4 NOT NULL,
    "CodeProduct" CHARACTER VARYING(250),
    "TypeProduct" CHARACTER VARYING(250),
    "Name" CHARACTER VARYING(250),
    "Manufacturer" CHARACTER VARYING(250),
    "UnitOfMeasurementId" int4 NOT NULL,
	FOREIGN KEY ("SectionId") REFERENCES "Sections" ("Id"),
	FOREIGN KEY ("UnitOfMeasurementId") REFERENCES "UnitOfMeasurement" ("Id")
);

CREATE TABLE "SynchPrice"
(
    "Id" SERIAL PRIMARY KEY,
    "SynchSpecificationId" int4 NOT NULL,
    "SupplierId" int4 NOT NULL,
    "Price" float4 NULL,
	FOREIGN KEY ("SynchSpecificationId") REFERENCES "SynchSpecification" ("Id"),
	FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id")
);
