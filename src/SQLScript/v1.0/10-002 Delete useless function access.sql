use [FreightSystemDB]

DELETE FROM [FreightSystemDB].[dbo].[FuncItem]
      WHERE [FuncCode] in ('FILLRD', 'UPDTDETAIL')
GO

