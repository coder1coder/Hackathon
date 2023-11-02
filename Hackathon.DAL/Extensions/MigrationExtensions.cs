using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon.DAL.Extensions
{
    public static class MigrationExtensions
    {
        public static void SqlFile(this MigrationBuilder migrationBuilder, string sqlFile,
            bool suppressTransaction = false)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var filePath = Path.Combine(AppContext.BaseDirectory, sqlFile);
            // ReSharper restore AssignNullToNotNullAttribute
            migrationBuilder.Sql(File.ReadAllText(filePath), suppressTransaction);
        }
    }
}
