namespace Catalog.Application.Extensions.Encryptor;
public static class ModelBuilderExtensions
{
	//public static DateTime? ToDateTime(this string dateString, int format) => throw new NotSupportedException();
	//public static string NpgDecrypt(this string encryptString, string securityKey) => throw new NotSupportedException();

	//public static ModelBuilder AddSqlConvertFunction(this ModelBuilder modelBuilder)
	//{
	//	modelBuilder.HasDbFunction(() => ToDateTime(default, default))
	//		.HasTranslation(args => new SqlFunctionExpression(
	//				functionName: "CONVERT",
	//				arguments: args.Prepend(new SqlFragmentExpression("date")),
	//				nullable: true,
	//				argumentsPropagateNullability: new[] { false, true, false },
	//				type: typeof(DateTime),
	//				typeMapping: null));

	//	return modelBuilder;
	//}
	//public static ModelBuilder AddDecryptFunction(this ModelBuilder modelBuilder)
	//{

	//	modelBuilder.HasDbFunction(() => NpgDecrypt(default, default))
	//		.HasTranslation(args =>
	//		{
	//			ColumnExpression columnExpression = args.First() as ColumnExpression;
	//			SqlConstantExpression securityKeyExpression = args.Skip(1).First() as SqlConstantExpression;
	//			SqlFragmentExpression valueExpression =
	//			new($"{columnExpression.Table.Alias}.{columnExpression.Name} ::bytea, {securityKeyExpression.Value}");

	//			return new SqlFunctionExpression(
	//				functionName: "PGP_SYM_DECRYPT",
	//				arguments: args.Prepend(valueExpression),
	//				nullable: true,
	//				argumentsPropagateNullability: new[] { false, true, false },
	//				type: typeof(DateTime),
	//				typeMapping: null);
	//		});

	//	return modelBuilder;
	//}
}
