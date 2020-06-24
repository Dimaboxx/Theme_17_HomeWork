use MSSQLLocalDemo

--DROP TABLE IF EXISTS [dbo].[Accaunts]
--drop TABLE IF EXISTS [dbo].[ClientType]
--drop TABLE IF EXISTS [dbo].[Clients]
--drop TABLE IF EXISTS [dbo].[Organisations]
--drop TABLE IF EXISTS [dbo].[AccauntType]
--drop TABLE IF EXISTS [dbo].[ratesType]
--drop PROCEDURE IF EXISTS NextClientId
--drop PROCEDURE IF EXISTS NextAccauntId


CREATE TABLE [dbo].[ClientType] (
    [id]          INT           IDENTITY (0, 1) NOT NULL,
    [Description] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ClientTypeId] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[Clients] (
    [id]               INT            nOT NULL,
    [FirstName]        NVARCHAR (25)  not NULL,
    [MidleName]        NVARCHAR (25)  NULL,
    [LastName]         NVARCHAR (25)  not NULL,
    [ClientType]       INT            DEFAULT ((0)) NOT NULL,
    [FullName]         AS             ([FirstName]+' '+[MidleName]+' '+[LastName] ),
    [GoodHistory]      BIT            DEFAULT ((0)) not NULL,
    [Documents]        NVARCHAR (50)  not NULL,
    CONSTRAINT [PK_Clients_Id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT AK_UnicClient UNIQUE (FullName, Documents)
);

CREATE TABLE [dbo].[Organisations] (
    [id]               INT            NOT NULL,
    [ClientType]       INT            DEFAULT ((1)) NOT NULL,
    [OrganisationName] NVARCHAR (100) not null,
    [GoodHistory]      BIT            DEFAULT ((0)) not NULL,
    [BankDetails]      NVARCHAR (100) not null,
    [Adress]           NVARCHAR (100) not null,
    CONSTRAINT [PK_Organisation_Id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT AK_UnicOrganisation UNIQUE (OrganisationName, BankDetails)
);

----go
--    create view ClientsAllTypes_id
--    as  
--    select id from Clients 
--    union
--    select id from Organisation
--go

CREATE TABLE [dbo].[AccauntType] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_AccauntTypeId] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[ratesType] (
    [id]          INT           NOT NULL,
    [Description] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ratesTypes] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[Accaunts] (
    [id]             INT    NOT NULL,
    [OpenDate]       DATE  NOT NULL,
    [EndDate]        DATE  NULL,
    [TypeId]         INT   NOT NULL,
    [Rates]          REAL  DEFAULT ((0)) NOT NULL,
    [Balans]         MONEY DEFAULT ((0)) NOT NULL,
    [OwnerId]        INT   NOT NULL,
    [Capitalisation] BIT   DEFAULT ((0)) NOT NULL,
    [RatesTypeid]    INT   NOT NULL,
    CONSTRAINT [fk_ratetype_ratetype_id] FOREIGN KEY ([RatesTypeid]) REFERENCES [dbo].[ratesType] ([id]),
    CONSTRAINT [FK_Type_AccauntType_id] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[AccauntType] ([id])--,
    ---CONSTRAINT [FK_OwnerID_Clients_id] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[ClientsAllTypes_id] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
    );
go

--USE productsdb;
GO
CREATE PROCEDURE NextClientId AS
BEGIN
    declare @res int;
    set @res = (select top(1) id from (select id from Clients union select id from Organisations ) as c order by c.id desc)
    set @res = isnull(@res,0)+1
	return @res
END
go

GO
CREATE PROCEDURE NextAccauntId AS
BEGIN
    declare @res int;
    set @res = (select top(1) id from Accaunts order by id desc)
    set @res = isnull(@res,0)+1
	return @res
END
go




insert into [dbo].[AccauntType](Description) VALUES ('Simple'),('Deposite'),('Credit')

insert into [dbo].[ClientType](Description) VALUES (N'Физ.Лицо'),(N'Организация')

insert into [dbo].[ratesType]([id], [Description]) values (0,N'без процентов'),(1, N'Ежедневно'), (2,N'Ежемесячно'),(3, N'Ежегодно'),(4, N'В конце срока')

--set identity_insert [dbo].Clients on
go 
declare @id int;
declare @accid int;
declare @nclient int;
declare @norgs int;


set @nclient =  ( select count(*) from Clients )+1


exec @id = NextClientId 

select @id
insert into [dbo].[Clients] 
                            (id
                            ,FirstName
                            ,MidleName
                            ,LastName
                            ,Documents) 
                    Values(@id
                    ,N'fn'+str(@nclient),
                    'mn'+str(@nclient),
                    'ln'+str(@nclient),
                    'Doc'+str(@nclient)
                    );
exec @accid = NextAccauntId
insert into [dbo].Accaunts(id,
                           OpenDate,
                           EndDate,
                           OwnerId,
                           TypeId,
                           Balans,
                           ratesTypeid,
                           rates,
                           Capitalisation) 
           Values(@accid,
           CAST(getdate() as date),
           DATEADD(month, 12 , getdate()),
           @id,
           1,
           @id*1100,
           1,
           6,
           'FALSE'
           )
exec @id = NextClientId
set @norgs =  ( select count(*) from Organisations )+1
insert into [dbo].Organisations(Id,OrganisationName,[BankDetails],Adress)  Values(@id,N'Orgn'+str(@norgs),'Req '+str(@norgs), 'Adr '+str(@norgs));
exec @accid = NextAccauntId
insert into [dbo].Accaunts(id,
                           OpenDate,
                           EndDate,
                           OwnerId,
                           TypeId,
                           Balans,
                           ratesTypeid,
                           rates,
                           Capitalisation) 
           Values(@accid,
           CAST(getdate() as date),
           DATEADD(month, 12 , getdate()),
           @id,
           1,
           @id*1100,
           1,
           6,
           'FALSE'
           )
go 5




--set identity_insert [dbo].Clents off
select 
    id, [Name] ,ClientType,[Type],GoodHistory  
from 
    (select 
        id,FullName as N'Name',ClientType, 0 as [Type],GoodHistory 
        from [Clients] 
    union 
    select 
        id, OrganisationName as'Name',ClientType, 1 as [Type],GoodHistory 
    from Organisations) as c
order by id




select
       a.id,
       act.Description as 'TypeDesc',
       rt.Description as 'RatesType',
       c.FullName as 'Owner',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       a.OwnerId,
       a.ratesTypeid,
       a.Capitalisation,
       a.TypeId
        
    from
    [Accaunts] as a 
    left join AccauntType as act on a.[TypeId] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id
    left join (select 
        id,FullName as 'FullName',ClientType
    from [Clients] 
    union 
    select 
        id, OrganisationName as'FullName',ClientType
    from Organisations) as c on a.OwnerId = c.id


    select
       a.id,
       act.Description as 'TypeDesc',
       rt.Description as 'RatesType',
       c.FullName as 'Owner',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       a.OwnerId,
       a.ratesTypeid,
       a.Capitalisation,
       a.TypeId
        
    from
    [Accaunts] as a 
    left join AccauntType as act on a.[TypeId] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id
    RIGHT JOIN (select 
        id,FullName as 'FullName',ClientType
    from [Clients] 
    --union 
    --select 
    --    id, OrganisationName as'FullName',ClientType
    --from Organisations
    ) as c on a.OwnerId = c.id
     

         select
       a.id,
       act.Description as 'TypeDesc',
       rt.Description as 'RatesType',
       c.FullName as 'Owner',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       a.OwnerId,
       a.ratesTypeid,
       a.Capitalisation,
       a.TypeId
        
    from
    [Accaunts] as a 
    left join AccauntType as act on a.[TypeId] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id
    RIGHT JOIN (
    --select 
    --    id,FullName as 'FullName',ClientType
    --from [Clients] 
    --union 
    select 
        id, OrganisationName as'FullName',ClientType
    from Organisations
    ) as c on a.OwnerId = c.id