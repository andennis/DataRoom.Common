CREATE PROCEDURE [cmntst].[Get_TestEntity1]
	@Id BIGINT
AS
BEGIN
	SELECT * from cmntst.TestEntity1 where MyId = @Id
END
