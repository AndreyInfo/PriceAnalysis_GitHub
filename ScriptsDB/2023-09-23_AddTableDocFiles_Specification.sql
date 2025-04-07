CREATE TABLE "DocFiles" (
	"Id" SERIAL PRIMARY KEY,
	"FileName" varchar NOT NULL,
	"GUID" uuid NOT NULL,
	"CreateDate" timestamptz NOT NULL,
	"CreatorId" int4 NOT NULL
);

CREATE TABLE "UnitOfMeasurement" (
	"Id" SERIAL PRIMARY KEY,
	"Name" varchar NULL,
	"FullName" varchar NULL,
	"GroupUnitOfMeasurementId" int4 NULL,
	"Aliases" varchar NULL,
	"Code" int4 NULL,
	"Accordance" int4 NULL
);

CREATE TABLE "ChapterForSpecification" (
	"Id" SERIAL PRIMARY KEY,
	"Name" varchar null
);

CREATE TABLE "SpecificationFile" (
	"Id" SERIAL PRIMARY KEY,
	"SectionId" int4 NOT NULL,
	"Name" varchar(250) NOT NULL,
	"DocFilesId" int4 NOT NULL,
	FOREIGN KEY ("SectionId") REFERENCES "Sections" ("Id"),
	FOREIGN KEY ("DocFilesId") REFERENCES "DocFiles" ("Id")
);

CREATE TABLE "Specification" (
	"Id" SERIAL PRIMARY KEY,
	"SectionId" int4 NOT NULL,
	"SID" varchar(250) NOT NULL,
	"Name" varchar(250) NOT NULL,
	"Type" varchar(250) NOT NULL,
	"Code" varchar(10) null,
	"Vendor" varchar(10) null,
	"UnitOfMeasurementId" int4 NOT null,
	"Count" numeric NOT null,
	"ChapterId" int4 null,
	FOREIGN KEY ("SectionId") REFERENCES "Sections" ("Id"),
	FOREIGN KEY ("UnitOfMeasurementId") REFERENCES "UnitOfMeasurement" ("Id"),
	FOREIGN KEY ("ChapterId") REFERENCES "ChapterForSpecification" ("Id")
);

INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('�2','���������� ����
',1,NULL,55,NULL),
	 ('��2','���������� ���������
',1,NULL,51,NULL),
	 ('�3','���������� ����
',2,NULL,113,NULL),
	 ('��3','���������� ���������
',2,NULL,111,NULL),
	 ('��.','���������
',3,'��',166,NULL),
	 ('�.','�����
',3,'�',168,NULL),
	 ('��.','���������
',4,'��.',4,NULL),
	 ('��.','����
',5,'��',812,NULL),
	 ('�����
',NULL,5,'�����
',NULL,NULL),
	 ('��������
','��������',5,'�����.',NULL,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('��.','���������',6,NULL,3,NULL),
	 ('��.','��������',6,NULL,8,NULL),
	 ('��2','���������� ���������',6,NULL,50,NULL),
	 ('��3','���������� ���������',6,NULL,110,NULL),
	 ('�.','����',6,NULL,112,NULL),
	 ('�.','���',6,NULL,366,NULL),
	 ('�.','�������',6,NULL,208,NULL),
	 ('�.','�������',6,NULL,354,NULL),
	 ('���.','������',6,NULL,355,NULL),
	 ('�.','���',6,NULL,356,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('��.','�����',6,NULL,359,NULL),
	 ('���.','�����',6,NULL,362,NULL),
	 ('���.','������',6,NULL,616,NULL),
	 ('����','����',6,NULL,625,NULL),
	 ('���.','�������',6,NULL,657,NULL),
	 ('�����','�����',6,NULL,704,NULL),
	 ('���.','�����',6,NULL,736,NULL),
	 ('����.','��������',6,NULL,778,NULL),
	 ('% mds.','�������� ������ �� �����',6,NULL,820,NULL),
	 ('% vol.','�������� ������ �� �����',6,NULL,821,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('���.','�����',6,NULL,383,NULL),
	 ('���','����',6,NULL,639,NULL),
	 ('����','�����',6,NULL,698,NULL),
	 ('%','�������',6,NULL,744,NULL),
	 ('���.','�������',6,NULL,792,NULL),
	 ('��.','������',6,NULL,810,NULL),
	 ('�����.','��������',6,NULL,839,NULL),
	 ('����.','������',6,NULL,840,NULL),
	 ('���.','�������',6,NULL,868,NULL),
	 ('�����.','������',6,NULL,870,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('����.','������',6,NULL,872,NULL),
	 ('��� ��.','�������� �������',6,NULL,876,NULL),
	 ('���.','�����',6,NULL,908,NULL),
	 ('����','����',6,NULL,922,NULL),
	 ('���.','���� (2 ��.)',6,NULL,715,NULL),
	 ('��.','�����
',5,'��#������� �����������#������� �����������',796,NULL),
	 ('�.','����
',4,'�.',6,12),
	 ('��� �.','�������� ����',4,'�.�.#��#�. �.#���.�#��',18,7);