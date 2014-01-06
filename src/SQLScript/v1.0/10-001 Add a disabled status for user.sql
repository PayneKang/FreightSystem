use [FreightSystemDB]

alter table [dbo].[Users] 
	ADD [Disabled] bit NOT NULL default(0)