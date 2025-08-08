using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetCompaniesProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetCompanies] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Name nvarchar(max) = '%',
                	@Description nvarchar(max) = '%',
                	@RatingFrom int = NULL,
                	@RatingTo int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                	SET NOCOUNT ON;

                	DECLARE @sql nvarchar(2000);
                	DECLARE @countsql nvarchar(2000);
                	DECLARE @paramList nvarchar(MAX); 
                	DECLARE @countparamList nvarchar(MAX);

                	-- Collecting Total
                	SELECT @Total = COUNT(*) FROM Companies;

                	-- Collecting Total Display
                	SET @countsql = 'SELECT @TotalDisplay = COUNT(*) FROM Companies c WHERE 1 = 1';

                	SET @countsql = @countsql + ' AND c.Name LIKE ''%'' + @xName + ''%''';
                	SET @countsql = @countsql + ' AND c.Description LIKE ''%'' + @xDescription + ''%''';

                	IF @RatingFrom IS NOT NULL
                	    SET @countsql = @countsql + ' AND c.Rating >= @xRatingFrom';

                	IF @RatingTo IS NOT NULL
                	    SET @countsql = @countsql + ' AND c.Rating <= @xRatingTo';

                	SELECT @countparamList = '
                		@xName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xRatingFrom int,
                		@xRatingTo int,
                		@TotalDisplay int OUTPUT';

                	EXEC sp_executesql @countsql, @countparamList,
                		@Name,
                		@Description,
                		@RatingFrom,
                		@RatingTo,
                		@TotalDisplay = @TotalDisplay OUTPUT;

                	-- Collecting Data
                	SET @sql = 'SELECT * FROM Companies c WHERE 1 = 1';

                	SET @sql = @sql + ' AND c.Name LIKE ''%'' + @xName + ''%''';
                	SET @sql = @sql + ' AND c.Description LIKE ''%'' + @xDescription + ''%''';

                	IF @RatingFrom IS NOT NULL
                	    SET @sql = @sql + ' AND c.Rating >= @xRatingFrom';

                	IF @RatingTo IS NOT NULL
                	    SET @sql = @sql + ' AND c.Rating <= @xRatingTo';

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @PageSize * (@PageIndex - 1)
                		ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramList = '
                		@xName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xRatingFrom int,
                		@xRatingTo int,
                		@PageIndex int,
                		@PageSize int';

                	EXEC sp_executesql @sql, @paramList,
                		@Name,
                		@Description,
                		@RatingFrom,
                		@RatingTo,
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
            var sql = "DROP PROCEDURE [dbo].[GetCompanies]";
            migrationBuilder.Sql(sql);
        }
    }
}
