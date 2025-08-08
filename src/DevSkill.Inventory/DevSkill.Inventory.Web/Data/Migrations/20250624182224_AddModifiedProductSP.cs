using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedProductSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetProducts_Advanced]
                    @PageIndex INT,
                    @PageSize INT,
                    @OrderBy NVARCHAR(50),
                    @Name NVARCHAR(MAX) = '%',
                    @PriceFrom FLOAT = NULL,
                    @PriceTo FLOAT = NULL,
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @sql NVARCHAR(MAX);
                    DECLARE @countsql NVARCHAR(MAX);
                    DECLARE @paramList NVARCHAR(MAX);
                    DECLARE @countparamList NVARCHAR(MAX);

                    -- Total count (unfiltered)
                    SELECT @Total = COUNT(*) FROM Products;

                    -- TotalDisplay (filtered count)
                    SET @countsql = N'
                        SELECT @TotalDisplay = COUNT(*)
                        FROM Products p
                        WHERE 1 = 1
                          AND p.Name LIKE ''%'' + @xName + ''%''';

                    IF @PriceFrom IS NOT NULL
                        SET @countsql += ' AND p.Price >= @xPriceFrom';

                    IF @PriceTo IS NOT NULL
                        SET @countsql += ' AND p.Price <= @xPriceTo';

                    SET @countparamList = N'
                        @xName NVARCHAR(MAX),
                        @xPriceFrom FLOAT,
                        @xPriceTo FLOAT,
                        @TotalDisplay INT OUTPUT';

                    EXEC sp_executesql @countsql, @countparamList,
                        @Name,
                        @PriceFrom,
                        @PriceTo,
                        @TotalDisplay = @TotalDisplay OUTPUT;

                    -- Main query with pagination and sorting
                    SET @sql = N'
                        SELECT 
                            p.Id,
                            p.Name,
                            p.Price,
                            p.PublishedAt,
                            c.Name AS CompanyName,
                            u.Name AS [Unit],
                            p.ImagePath
                        FROM Products p
                        LEFT JOIN Companies c ON p.CompanyId = c.Id
                        LEFT JOIN Units u ON p.UnitId = u.Id
                        WHERE 1 = 1
                          AND p.Name LIKE ''%'' + @xName + ''%''';

                    IF @PriceFrom IS NOT NULL
                        SET @sql += ' AND p.Price >= @xPriceFrom';

                    IF @PriceTo IS NOT NULL
                        SET @sql += ' AND p.Price <= @xPriceTo';

                    SET @sql += ' ORDER BY ' + @OrderBy + '
                        OFFSET @PageSize * (@PageIndex - 1) ROWS
                        FETCH NEXT @PageSize ROWS ONLY';

                    SET @paramList = N'
                        @xName NVARCHAR(MAX),
                        @xPriceFrom FLOAT,
                        @xPriceTo FLOAT,
                        @PageIndex INT,
                        @PageSize INT';

                    EXEC sp_executesql @sql, @paramList,
                        @Name,
                        @PriceFrom,
                        @PriceTo,
                        @PageIndex,
                        @PageSize;

                    PRINT @sql;
                    PRINT @countsql;
                END
                
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts_Advanced]";
            migrationBuilder.Sql(sql);
        }
    }
}
