CREATE PROCEDURE [dbo].[event_Update]
	(
	@id int,
	@layoutId int,
	@description nvarchar(MAX),
	@name nvarchar(120),
	@startDate datetime2(7),
	@endDate datetime2(7),
	@imageUrl nvarchar(MAX),
	@rowCount int out
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM [dbo].[Event] WHERE Id = @id)
	BEGIN
		UPDATE [dbo].[Event] 
		SET Description = @description, LayoutId = @layoutId, Name = @name, StartDate = @startDate, EndDate = @endDate, ImageUrl = @imageUrl
		WHERE Id = @id
		SET @rowCount = (CAST(@@ROWCOUNT AS int))
	END
END
GO