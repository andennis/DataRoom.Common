CREATE PROCEDURE [cmntst].[TestEntity1_GetView]
	@ID BIGINT
AS
BEGIN
	SELECT * FROM cmntst.TestEntity1 WHERE MyId = @ID
END
