--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
--module projects sql script 01.12.23
--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

-- добавляем таблицу компаний для справочника организаций;

DROP TABLE IF EXISTS public."Companies";

CREATE TABLE IF NOT EXISTS public."Companies"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid(),
    "Key" character varying(8) COLLATE pg_catalog."default",
    "Name" character varying(200) COLLATE pg_catalog."default",
    "Type" integer,
    "Activity" text COLLATE pg_catalog."default",
    "INN" character varying(12) COLLATE pg_catalog."default",
    "KPP" character varying(12) COLLATE pg_catalog."default",
    "OGRN" character varying(15) COLLATE pg_catalog."default",
    "OKPO" character varying(15) COLLATE pg_catalog."default",
    "OKVED" character varying(8) COLLATE pg_catalog."default",
    "DirectorName" character varying(200) COLLATE pg_catalog."default",
    "ActBasis" character varying(500) COLLATE pg_catalog."default",
    "CompanyPhone" character varying(100) COLLATE pg_catalog."default",
    "CompanyEmail" character varying(256) COLLATE pg_catalog."default",
    "CompanyAddress" character varying(500) COLLATE pg_catalog."default",
    "CompanyLegalAddress" character varying(500) COLLATE pg_catalog."default",
    "CompanyDeliveryAddress" character varying(500) COLLATE pg_catalog."default",
    "CompanyLegalName" character varying(500) COLLATE pg_catalog."default",
    "AccountantName" character varying(200) COLLATE pg_catalog."default",
    "AccountantEmail" character varying(256) COLLATE pg_catalog."default",
    "CheckingAccount" character varying(20) COLLATE pg_catalog."default",
    "BankName" character varying(500) COLLATE pg_catalog."default",
    "CorrespondentAccount" character varying(20) COLLATE pg_catalog."default",
    "BIK" character varying(9) COLLATE pg_catalog."default",
    "BankCity" character varying(500) COLLATE pg_catalog."default",
    "ContactPersonName" character varying(500) COLLATE pg_catalog."default",
    "ContactPersonPhoneNumber" character varying(20) COLLATE pg_catalog."default",
    "ContactPersonEmail" character varying(256) COLLATE pg_catalog."default",
    "Description" text COLLATE pg_catalog."default",
    "WithNDS" boolean NOT NULL DEFAULT false,
    "ScladId" integer,
    "Status" integer NOT NULL DEFAULT 0,
    "LogoPath" text COLLATE pg_catalog."default",
    "CreatedBy" character varying(200) COLLATE pg_catalog."default",
    "CreatedById" integer,
    "CreatedOn" timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastModifiedBy" character varying(200) COLLATE pg_catalog."default",
    "LastModifiedById" integer,
    "LastModifiedOn" timestamp without time zone,
    "DeletedBy" character varying(200) COLLATE pg_catalog."default",
    "DeletedById" integer,
    "DeletedOn" timestamp without time zone,
    CONSTRAINT "Companies_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Companies"
    OWNER to postgres;


--удаляем привязку разделов к проектам

ALTER TABLE IF EXISTS public."Sections" 
DROP CONSTRAINT IF EXISTS "Sections_ProjectId_fkey";


--переименовываем существующую таблицу проектов

ALTER TABLE IF EXISTS public."Projects"
RENAME TO "Projects_tmp";


--создаем новую таблицу проектов

DROP TABLE IF EXISTS public."Projects";
	
CREATE TABLE IF NOT EXISTS public."Projects"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid(),
    "Number" integer NOT NULL,
    "Status" integer NOT NULL DEFAULT 3,
    "TypeId" integer NOT NULL DEFAULT 1,
    "Stage" integer NOT NULL DEFAULT 1,
    "Name" character varying(500) COLLATE pg_catalog."default" NOT NULL,
    "Description" text COLLATE pg_catalog."default",
    "CurrencyCode" character varying(8) COLLATE pg_catalog."default" NOT NULL DEFAULT 'RUB'::character varying,
    "Address" character varying(250) COLLATE pg_catalog."default",
    "GeoCityId" integer,
    "GeoLat" double precision,
    "GeoLng" double precision,
    "GeoZoom" integer,
    "CustomerId" uuid,
    "ProjectResponsibleUserId" integer,
    "GeneralDesignerId" uuid,
    "ProjectManagerId" integer,
    "GeneralContractorId" uuid,
    "ObjectResponsibleUserId" integer,
    "RegisterUserId" integer,
    "RegisterDate" timestamp without time zone,
    "StartDate" date,
    "EndDate" date,
    "SmetDistrictNumber" integer,
    "EstimateCalculationType" integer,
    "GeneralContractCost" double precision,
    "LaborIntensityAdjustmentFactor" double precision,
    "CreatedBy" character varying(200) COLLATE pg_catalog."default",
    "CreatedById" integer,
    "CreatedOn" timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastModifiedBy" character varying(200) COLLATE pg_catalog."default",
    "LastModifiedById" integer,
    "LastModifiedOn" timestamp without time zone,
    "DeletedBy" character varying(200) COLLATE pg_catalog."default",
    "DeletedById" integer,
    "DeletedOn" timestamp without time zone,
    "CustomerIdOld" character varying(8) COLLATE pg_catalog."default",
    "GeneralDesignerIdOld" character varying(8) COLLATE pg_catalog."default",
    "ImageExt" character varying(10) COLLATE pg_catalog."default",
    "ImageUploadedBy" character varying(200) COLLATE pg_catalog."default",
    "ImageUploadedById" integer,
    "ImageUploadedOn" timestamp without time zone,
    "IsPd" boolean NOT NULL DEFAULT false,
    "IsRd" boolean NOT NULL DEFAULT false,
    "IsPdLsr" boolean NOT NULL DEFAULT false,
    "IsRdLsr" boolean NOT NULL DEFAULT false,
    CONSTRAINT "PK_Projects" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Projects"
    OWNER to postgres;

COMMENT ON COLUMN public."Projects"."Number"
    IS 'Номер проекта (изначально - Id из старой базы)';

DROP SEQUENCE IF EXISTS public."Projects_Number_seq";

CREATE SEQUENCE IF NOT EXISTS public."Projects_Number_seq"
    INCREMENT 1
    START 4
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "Projects"."Number";

ALTER SEQUENCE public."Projects_Number_seq"
    OWNER TO postgres;
	
	
--таблица субподрядчиков

DROP TABLE IF EXISTS public."ProjectSubcontractorsRel";

CREATE TABLE IF NOT EXISTS public."ProjectSubcontractorsRel"
(
    "ProjectId" uuid NOT NULL,
    "SubcontractorId" uuid NOT NULL,
    "ProjectResponsibleUserId" integer,
    "CreatedBy" character varying(200) COLLATE pg_catalog."default",
    "CreatedById" integer,
    "CreatedOn" timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "ProjectSubcontractorsRel_pkey" PRIMARY KEY ("ProjectId", "SubcontractorId"),
    CONSTRAINT "ProjectSubcontractorsRel_fkey_pid" FOREIGN KEY ("ProjectId")
        REFERENCES public."Projects" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."ProjectSubcontractorsRel"
    OWNER to postgres;
	
	
--переносим проекты которые были в новую таблицу
INSERT INTO public."Projects"(
	"Number", "Name", "Description", "CreatedBy", "CreatedById")
SELECT "Id", "Name", "FullName", 'Неизвестный пользователь', 0
	FROM public."Projects_tmp";


--удаляем старую таблицу
DROP TABLE IF EXISTS public."Projects_tmp";


--переименовываем колонку привязки к айди проекта
ALTER TABLE IF EXISTS public."Sections" RENAME COLUMN "ProjectId" TO "ProjectId_tmp";


--добавляем новый проектный айди
ALTER TABLE IF EXISTS public."Sections"
ADD COLUMN "ProjectId" uuid;


--обновляем айди
UPDATE public."Sections"
SET "ProjectId" = public."Projects"."Id"
FROM public."Projects"
WHERE public."Sections"."ProjectId_tmp" = public."Projects"."Number";


ALTER TABLE IF EXISTS public."Sections"
ALTER COLUMN "ProjectId" SET NOT NULL;

--удаляем старый айди
ALTER TABLE IF EXISTS public."Sections"
DROP COLUMN "ProjectId_tmp";


--новая привязка разделов к новой таблице проектов
ALTER TABLE IF EXISTS public."Sections"
    ADD CONSTRAINT "Sections_ProjectId_fkey" FOREIGN KEY ("ProjectId")
    REFERENCES public."Projects" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE IF EXISTS public."Projects"
ALTER COLUMN "Number" SET DEFAULT nextval('"Projects_Number_seq"'::regclass);