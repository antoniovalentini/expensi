CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Username" character varying(50) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "PasswordHash" text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Expenses" (
    "Id" uuid NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Category" text NOT NULL,
    "CategorySubType" text NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Currency" text NOT NULL,
    "Remitter" text NOT NULL,
    "ReferenceDate" date NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    CONSTRAINT "PK_Expenses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Expenses_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Expenses_CreatedByUserId" ON "Expenses" ("CreatedByUserId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");
