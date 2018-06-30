CREATE PROCEDURE [cmntst].[TestEntity1_Search]
	@Name nvarchar(50),
	@TotalRecords BIGINT OUTPUT
AS
begin
	SET @TotalRecords = 1
	SELECT * FROM cmntst.TestEntity1 WHERE [Name] = @Name
end
