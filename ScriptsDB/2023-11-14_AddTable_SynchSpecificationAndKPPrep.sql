CREATE TABLE "SynchSpecificationAndKPPrep"
(
	"Id" SERIAL PRIMARY KEY,
	"SectionId" int4 NOT NULL,
	"IsSynch" bool null,
	FOREIGN KEY ("SectionId") REFERENCES "Sections" ("Id")
);

--drop table "SynchSpecificationAndKPPrep";