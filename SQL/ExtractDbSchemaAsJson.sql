-- Retrieves the entire database schema as a JSON string
SELECT
(
  SELECT
    TABLE_NAME AS [Table],
    COLUMN_NAME AS [Column],
    DATA_TYPE AS [Type]
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
    TABLE_SCHEMA = 'dbo'
  ORDER BY
    TABLE_NAME FOR JSON PATH
) AS json_result;