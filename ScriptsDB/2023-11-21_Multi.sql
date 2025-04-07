ALTER TABLE "Specification"
RENAME COLUMN "Vendor" TO "Manufacturer";

ALTER TABLE public."Specification" DROP CONSTRAINT "Specification_ChapterId_fkey";
ALTER TABLE public."Specification" DROP COLUMN "ChapterId";
drop  table "ChapterForSpecification";

CREATE TABLE "ChapterForSpecification"
(
    "Id" uuid PRIMARY KEY,
    "Name" CHARACTER VARYING(250)
);

ALTER TABLE "Specification"
ADD "ChapterId" uuid NULL;

ALTER TABLE public."Specification" 
ADD CONSTRAINT specification_fk FOREIGN KEY ("ChapterId") REFERENCES public."ChapterForSpecification"("Id");

ALTER TABLE public."Specification" ALTER COLUMN "Code" TYPE varchar USING "Code"::varchar;
ALTER TABLE public."Specification" ALTER COLUMN "Manufacturer" TYPE varchar USING "Manufacturer"::varchar;