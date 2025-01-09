-- Books

SELECT TOP (1000) [Id]
      ,[Title]
      ,[Author]
  FROM [contrarian-reads-backend].[dbo].[Books]

SELECT * FROM [contrarian-reads-backend].[dbo].[Books]
WHERE [Id] = 'C2D55C22-A4BF-4274-A503-EB1E50A73626';

DELETE FROM [contrarian-reads-backend].[dbo].[Books]
WHERE [Id] = 'AED3FB76-6898-4975-AEE2-E2949C512BBE';

-- BookAlternatives

SELECT TOP (1000) [Id]
      ,[OriginalBookId]
      ,[AlternativeBookId]
      ,[ApplicationUserId]
  FROM [contrarian-reads-backend].[dbo].[BookAlternatives]

-- BookAlternativeUpvotes

SELECT TOP (1000) [Id]
      ,[BookAlternativeId]
      ,[UserId]
      ,[ApplicationUserId]
  FROM [contrarian-reads-backend].[dbo].[BookAlternativeUpvotes]
