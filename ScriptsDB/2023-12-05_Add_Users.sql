CREATE TABLE IF NOT EXISTS public."Users"
(
    "Id" SERIAL PRIMARY KEY,
    "FullName" character varying(250) COLLATE pg_catalog."default",
    "Login" character varying(250) COLLATE pg_catalog."default",
    "Email" character varying(250) COLLATE pg_catalog."default",
    "PhoneNumber" character varying(20) COLLATE pg_catalog."default",
    "IsActive" boolean NOT NULL DEFAULT true,
    "DeletedBy" character varying(200) COLLATE pg_catalog."default",
    "DeletedById" integer,
    "DeletedOn" timestamp without time zone
)