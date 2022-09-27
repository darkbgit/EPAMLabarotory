CREATE PROCEDURE [dbo].[event_IsExist]
		@layoutId int,
	@description nvarchar(MAX),
	@name nvarchar(120),
	@startDate datetime2(7),
	@endDate datetime2(7)
AS
BEGIN
	SET NOCOUNT ON

	SELECT CASE WHEN EXISTS (SELECT Description
		FROM [dbo].[Event]
		WHERE LayoutId = @layoutId AND (Description = @description OR Name = @name OR (StartDate < @startDate AND EndDate > @startDate) OR (StartDate < @endDate AND EndDate > @endDate)))
		THEN CAST (1 AS BIT) 
        ELSE CAST (0 AS BIT) END AS Result

END
GO