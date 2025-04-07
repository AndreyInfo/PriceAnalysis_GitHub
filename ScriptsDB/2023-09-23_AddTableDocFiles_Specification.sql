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
	 ('м2','Квадратный метр
',1,NULL,55,NULL),
	 ('см2','Квадратный сантиметр
',1,NULL,51,NULL),
	 ('м3','Кубический метр
',2,NULL,113,NULL),
	 ('см3','Кубический сантиметр
',2,NULL,111,NULL),
	 ('кг.','Килограмм
',3,'кг',166,NULL),
	 ('т.','Тонна
',3,'т',168,NULL),
	 ('см.','Сантиметр
',4,'см.',4,NULL),
	 ('ящ.','Ящик
',5,'ящ',812,NULL),
	 ('пачка
',NULL,5,'пачка
',NULL,NULL),
	 ('комплект
','Комплект',5,'компл.',NULL,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('мм.','Миллиметр',6,NULL,3,NULL),
	 ('км.','Километр',6,NULL,8,NULL),
	 ('мм2','Квадратный миллиметр',6,NULL,50,NULL),
	 ('мм3','Кубический миллиметр',6,NULL,110,NULL),
	 ('л.','Литр',6,NULL,112,NULL),
	 ('г.','Год',6,NULL,366,NULL),
	 ('ц.','Центнер',6,NULL,208,NULL),
	 ('с.','Секунда',6,NULL,354,NULL),
	 ('мин.','Минута',6,NULL,355,NULL),
	 ('ч.','Час',6,NULL,356,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('дн.','Сутки',6,NULL,359,NULL),
	 ('мес.','Месяц',6,NULL,362,NULL),
	 ('боб.','Бобина',6,NULL,616,NULL),
	 ('лист','Лист',6,NULL,625,NULL),
	 ('изд.','Изделие',6,NULL,657,NULL),
	 ('набор','Набор',6,NULL,704,NULL),
	 ('рул.','Рулон',6,NULL,736,NULL),
	 ('упак.','Упаковка',6,NULL,778,NULL),
	 ('% mds.','Крепость спирта по массе',6,NULL,820,NULL),
	 ('% vol.','Крепость спирта по массе',6,NULL,821,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('руб.','Рубль',6,NULL,383,NULL),
	 ('доз','Доза',6,NULL,639,NULL),
	 ('мест','Место',6,NULL,698,NULL),
	 ('%','Процент',6,NULL,744,NULL),
	 ('чел.','Человек',6,NULL,792,NULL),
	 ('яч.','Ячейка',6,NULL,810,NULL),
	 ('компл.','Комплект',6,NULL,839,NULL),
	 ('секц.','Секция',6,NULL,840,NULL),
	 ('бут.','Бутылка',6,NULL,868,NULL),
	 ('ампул.','Ампула',6,NULL,870,NULL);
INSERT INTO "UnitOfMeasurement" ("Name","FullName","GroupUnitOfMeasurementId","Aliases","Code","Accordance") VALUES
	 ('флак.','Флакон',6,NULL,872,NULL),
	 ('усл ед.','Условная единица',6,NULL,876,NULL),
	 ('ном.','Номер',6,NULL,908,NULL),
	 ('знак','Знак',6,NULL,922,NULL),
	 ('пар.','Пара (2 шт.)',6,NULL,715,NULL),
	 ('шт.','штука
',5,'шт#сборных конструкций#сборные конструкции',796,NULL),
	 ('м.','Метр
',4,'м.',6,12),
	 ('пог м.','Погонный метр',4,'п.м.#мп#п. м.#пог.м#пм',18,7);