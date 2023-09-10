﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class Alter_GetPerson_SP_TIN : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
             ALTER PROCEDURE [dbo].[GetAllPersons]
             AS BEGIN
                SELECT PersonId, PersonName, Email, DateOfBirth, Gender,
                CountryId, Address, ReceiveNewsLetters, TaxIdentificationNumber FROM [dbo].[Persons]
             END
            ";
			migrationBuilder.Sql(sp_GetAllPersons);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
             DROP PROCEDURE [dbo].[GetAllPersons]
            ";
			migrationBuilder.Sql(sp_GetAllPersons);
		}
	}
}
