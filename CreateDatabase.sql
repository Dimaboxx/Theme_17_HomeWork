use MSSQLLocalDemo

CREATE TABLE [dbo].[ClientType] (
    [id]          INT           IDENTITY (0, 1) NOT NULL,
    [Description] NVARCHAR (20) NULL,
    CONSTRAINT [PK_ClientTypeId] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[Clients] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (25)  NULL,
    [MidleName]        NVARCHAR (25)  NULL,
    [LastName]         NVARCHAR (25)  NULL,
    [ClientType]       INT            DEFAULT ((0)) NOT NULL,
    [OrganisationName] NVARCHAR (100) NULL,
    [FullName]         AS             (case when [ClientType]=(0) then ((([FirstName]+' ')+[MidleName])+' ')+[LastName] else [OrganisationName] end),
    [GoodHistory]      BIT            NULL,
    CONSTRAINT [PK_Clients_Id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [CHK_oneName] CHECK ([FirstName] IS NOT NULL OR [OrganisationName] IS NOT NULL)
);

CREATE TABLE [dbo].[AccauntType] (
    [id]          INT           IDENTITY (0, 1) NOT NULL,
    [Description] NVARCHAR (20) NULL,
    CONSTRAINT [PK_AccauntTypeId] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[ratesType] (
    [id]          INT           NOT NULL,
    [Description] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ratesTypes] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[Accaunts] (
    [id]             INT   IDENTITY (1, 1) NOT NULL,
    [OpenDate]       DATE  NOT NULL,
    [EndDate]        DATE  NULL,
    [Type]           INT   NOT NULL,
    [rates]          REAL  NULL,
    [Balans]         MONEY DEFAULT ((0)) NOT NULL,
    [OwnerId]        INT   NOT NULL,
    [Capitalisation] BIT   NULL,
    [ratesTypeid]    INT   NOT NULL,
    CONSTRAINT [fk_ratetype_ratetype_id] FOREIGN KEY ([ratesTypeid]) REFERENCES [dbo].[ratesType] ([id]),
    CONSTRAINT [FK_Type_AccauntType_id] FOREIGN KEY ([Type]) REFERENCES [dbo].[AccauntType] ([id]),
    CONSTRAINT [FK_OwnerID_Clients_id] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Clients] ([id]) ON DELETE CASCADE ON UPDATE CASCADE);
go


insert into [dbo].[AccauntType](Description) VALUES ('Simple'),('Deposite'),('Credit')

insert into [dbo].[ClientType](Description) VALUES (N'Физ.Лицо'),(N'Организация')

insert into [dbo].[ratesType]([id], [Description]) values (0,N'без процентов'),(1, N'Ежедневно'), (2,N'Ежемесячно'),(3, N'Ежегодно'),(4, N'В конце срока')

go 
insert into [dbo].[Clients] (ClientType,FirstName,MidleName,LastName)  Values(0 ,N'fn','mn','ln');
insert into [dbo].[Clients] (ClientType,OrganisationName)  Values(1,N'Orgn');
go 3


