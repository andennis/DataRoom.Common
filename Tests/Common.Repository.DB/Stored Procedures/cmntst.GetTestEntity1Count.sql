﻿CREATE PROCEDURE [cmntst].[GetTestEntity1Count]
	@Id BIGINT = 0
AS
BEGIN
	SELECT CAST(COUNT(*) AS BIGINT) FROM cmntst.TestEntity1 WHERE MyId = @Id
END
