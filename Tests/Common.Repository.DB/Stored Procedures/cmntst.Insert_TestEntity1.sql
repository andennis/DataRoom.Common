CREATE PROCEDURE [cmntst].[Insert_TestEntity1]
	@Id BIGINT OUTPUT,
	@Name nvarchar(50),
	@Value nvarchar(50)
AS
BEGIN
	insert into cmntst.TestEntity1 ([Name], [Value])
	values (@Name, @Value)
	SET @Id = SCOPE_IDENTITY()
END
