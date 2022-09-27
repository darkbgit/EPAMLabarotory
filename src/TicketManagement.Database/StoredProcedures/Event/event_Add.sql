CREATE PROCEDURE [dbo].[event_Add]
	(
	@layoutId int,
	@description nvarchar(MAX),
	@name nvarchar(120),
	@startDate datetime2(7),
	@endDate datetime2(7),
	@imageUrl nvarchar(MAX),
	@id int OUT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM [dbo].[Layout] WHERE Id = @layoutId)
	BEGIN

		IF NOT EXISTS (SELECT 1	FROM [dbo].[Event]	WHERE LayoutId = @layoutId AND (Description = @description OR Name = @name OR (StartDate < @startDate AND EndDate > @startDate) OR (StartDate < @endDate AND EndDate > @endDate)))
		BEGIN
			INSERT INTO [dbo].[Event] (LayoutId, Description, Name, StartDate, EndDate, ImageUrl)
			VALUES(@layoutId, @description, @name, @startDate, @endDate, @imageUrl)
			SET @id = (SELECT CAST(SCOPE_IDENTITY() AS int))
		END;
	END;

END
GO