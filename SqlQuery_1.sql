use MSSQLLocalDemo
-- (localdb)\MSSQLLocalDB
--  SQL file for home work
  drop database mssqllocaldemo
create database MSSQLLocalDemo
-- Create Client Table

CREATE TABLE [dbo].[Clients] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (25)  NULL,
    [MidleName]        NVARCHAR (25)  NULL,
    [LastName]         NVARCHAR (25)  NULL,
    [ClientType]       INT            DEFAULT ((0)) NOT NULL,
    [OrganisationName] NVARCHAR (100) NULL,
    [GoodHistory]      bit            DEFAULT (FALSE) not null,
    [FullName]         AS             (case 
                                        when [ClientType]=(0) then 
                                            ((([FirstName]+' ')+[MidleName])+' ')+[LastName] 
                                        else 
                                            [OrganisationName] 
                                        end),
    CONSTRAINT [CHK_oneName] CHECK ([FirstName] IS NOT NULL OR [OrganisationName] IS NOT NULL)
);


CREATE TABLE [dbo].[Accaunts](
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [OpenDate]          date           not null,
    [EndDate]           date           ,
    [Type]              int            not null,
    [rates]             real           ,
    [Balans]            money          not null default 0,
    [OwnerId]            int           not null,
    [Capitalisation]    bit             ,
    [ratesTypeid] int not null          ,
       CONSTRAINT fk_ratetype_ratetype_id foreign key (ratestypeid) references [dbo].ratesType (id),
       constraint FK_Type_AccauntType_id foreign key (Type) references [dbo].AccauntType (id),
       CONSTRAINT FK_OwnerID_Clients_id FOREIGN KEY (OwnerId)
      REFERENCES [dbo].Clients (id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

)

select * from  [dbo].[Accaunts]

update [dbo].Accaunts set Balans = Balans + 20 where id=2

Create table [dbo].[AccauntType]
(
    [id]                int Identity (1,1) not null,
    [Description]       nvarchar(20)
    CONSTRAINT PK_AccauntTypeId PRIMARY KEY  (id)
)

insert into [dbo].[AccauntType](Description) VALUES ('Simple'),('Deposite'),('Credit')
Select * from [dbo].[AccauntType]

Create table [dbo].[ClientType]
(
    [id]                int Identity (0,1) not null,
    [Description]       nvarchar(20)
    CONSTRAINT PK_ClientTypeId PRIMARY KEY  (id)
)



insert into [dbo].[ClientType](Description) VALUES (N'Физ.Лицо'),(N'Организация')
Select * from [dbo].[ClientType]

--drop table [dbo].[ClientType]

create table [dbo].[ratesType](
    [id] int not null,
    [Description] nvarchar(20) not null
    Constraint PK_ratesTypes Primary key (id)
         )
         insert into [dbo].[ratesType]([id], [Description]) values (0,N'без процентов'),(1, N'Ежедневно'), (2,N'Ежемесячно'),(3, N'Ежегодно'),(4, N'В конце срока')
select * from [dbo].ratesType


insert into [dbo].[Clients] (ClientType,FirstName,MidleName,LastName)  Values(0 ,N'fn','mn','ln');
insert into [dbo].[Clients] (ClientType,OrganisationName)  Values(1,N'Orgn');
GO 2

alter table [dbo].Accaunts add constraint FK_Type_AccauntType_id foreign key (Type) references [dbo].AccauntType (id)

insert into [dbo].Accaunts (OpenDate,EndDate,[Type],rates,OwnerId,Capitalisation,ratesTypeid) VALUES
                            (CAST(getdate() as date),DATEADD(month, 12 , getdate()),2,11,1,'FALSE',2)


update [dbo].Clients set    FirstName = 'FirstName ' + cast(id as nchar) ,
                            MidleName = 'MidleName ' + cast(id as nchar), 
                            LastName = 'LastName ' + cast(id as nchar) 
               where ClientType = 0 
update [dbo].Clients set OrganisationName = 'Orgs ' + cast(id as nchar)  where ClientType = 1



select * from [dbo].[Clients]

drop TABLE [dbo].[Clients]


alter table [dbo].[Clients] drop  column if exists [FullName]

alter table [dbo].[Clients] add  [FullName] as IIF(ClientType = 0, FirstName + ' ' + MidleName + ' ' + LastName, OrganisationName)
alter table [dbo].[Clients] add  [ClientType] int  DEFAULT 0 not null;
alter table [dbo].[Clients] add  [OrganisationName] nvarchar(100);
alter table [dbo].[Clients] add  [GoodHistory] bit;


update  [dbo].Clients set ClientType =0

select id, RAND(dbo.Clients.id*2)*3 from dbo.Clients

Select @@IDENTITY
Select SCOPE_IDENTITY()
Select IDENT_CURRENT()
 
select ClientType, Count(*) from [dbo].[Clients] Group by ClientType
Select Count(*) from [dbo].[Clients] where ClientType = 1
--ALTER TABLE [dbo].[Clients]
--   ADD CONSTRAINT CHK_oneName   
--   CHECK ((FirstName is not null and LastName is not null ) OR  OrganisationName is not null);  
--GO 

SELECT GETDATE() as now

ALTER TABLE [dbo].[ClientType]
   ADD CONSTRAINT PK_ClientTypeId PRIMARY KEY  (id);


ALTER TABLE [dbo].Accaunts
   ADD CONSTRAINT FK_OwnerID_Clients_id FOREIGN KEY (OwnerId)
      REFERENCES [dbo].Clients (id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

--ALTER TABLE Employees ADD DEFAULT (GETUTCDATE()) FOR RecordCreatedO
ALTER TABLE [dbo].Accaunts   ADD DEFAULT  0 for balans;


select 
    c.id,
    c.fullname,
    ct.[description] as 'ClientType', 
    ac.accs
from [dbo].[Clients] as c 
    left join [dbo].[ClientType] as ct 
on c.ClientType = ct.id 
    left join (select OwnerId, cast (count(*) as int) as 'accs' from [dbo].Accaunts group by OwnerId ) as ac 
    on c.id = ac.OwnerId
    order by c.id




    select
       a.id,
       act.Description as 'Type',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       rt.Description as 'RatesType'


    from
    [Accaunts] as a 
    left join AccauntType as act on a.[Type] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id  



    create table test (
    id int not null);


    set @res = (select top(1) id as d  from test order by id desc)
    set @res = isnull(@res,0)+1


    set @res = [dbo].NextId
    select @res
    
    declare @res int;
    exec @res = dbo.NextId
    insert into test(id) Values(@res);

    select *  from test order by id desc






