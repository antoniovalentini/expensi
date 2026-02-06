CREATE TABLE "Remitters" (
    "Id" uuid NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Remitters" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Remitters_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Remitters_CreatedByUserId" ON "Remitters" ("CreatedByUserId");
