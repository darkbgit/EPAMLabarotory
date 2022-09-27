CREATE PROCEDURE [dbo].[event_Remove]
	(@id int,
	@rowCount int out)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM [dbo].[Event] WHERE Id = @id)
	BEGIN
		DELETE
		FROM [dbo].[Event]
		WHERE Id=@id
		SET @rowCount = (CAST(@@ROWCOUNT AS int))
	END
END
GO