CREATE PROCEDURE [cmntst].[GetTestEntity1Count]
	@Id BIGINT = 0
AS
BEGIN
	SELECT count(*) from cmntst.TestEntity1 where MyId = @Id
END
