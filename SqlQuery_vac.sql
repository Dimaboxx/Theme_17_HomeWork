--В БД есть таблица клиентов: CustomerId, RegistrationDateTime. 
--И таблица с покупками: CustomerId, PurchaseDateTime, ProductName

--Приложите ссылку на sqlfiddle.com (или альтернативный
-- ресурс с теми же возможностями - rextester.com, db-fiddle.com)
-- с запросом 
 
-- выбирающим клиентов, которые покупали молоко за последнюю неделю, но никогда не покупали сметану.



CREATE TABLE  [dbo].[Clients] (
                CustomerId int IDENTITY (1,1) not null,
                RegistrationDateTime DateTime not null Default GETDATE()
)

Create TABLE [dbo].[Orders] (
    CustomerId int not null,
    PurchaseDateTime DateTime not null,
    ProductName nvarchar(50)
)


insert into [dbo].Clients(RegistrationDateTime) Values(GETDATE()),(GETDATE()),(GETDATE()),(GETDATE()),(GETDATE()),(GETDATE())
--go 10

insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) 
                         VALUES(1,N'Молоко', DATEADD(D,-15,GETdATE())),
                                (1,N'Сметана', DATEADD(D,-10,GETdATE())),
                                (2,N'Молоко', DATEADD(D,-5,GETdATE())),
                                (2,N'Хлеб', DATEADD(D,-5,GETdATE())),
                                (3,N'Молоко', DATEADD(D,-5,GETdATE())),
                                (3,N'Сметана', DATEADD(D,-15,GETdATE())),
                                (3,N'Хлеб', DATEADD(D,-5,GETdATE())),
                                (4,N'Хлеб', DATEADD(D,-5,GETdATE())),
                                (4,N'Яйца', DATEADD(D,-45,GETdATE())),
                                (5,N'Хлеб', DATEADD(D,-5,GETdATE())),
                                (5,N'Яйца', DATEADD(D,-45,GETdATE())),
                                (5,N'Молоко', DATEADD(D,-3,GETdATE())),
                                (6,N'Колбаса', DATEADD(D,-5,GETdATE())),
                                (6,N'Яйца', DATEADD(D,-45,GETdATE())),
                                (6,N'Молоко', DATEADD(D,-3,GETdATE())),
                                (6,N'Сметана', DATEADD(D,-3,GETdATE()))

Select c.*, o.* from [dbo].Clients as c  join   (
select CustomerId  from [dbo].Orders where ProductName = N'Молоко' and PurchaseDateTime > DATEADD(WW,-1,GETdATE()) group by CustomerId
EXCEPT select CustomerId  from [dbo].Orders where ProductName = N'Сметана' group by CustomerId ) as o  on c.CustomerId = o.CustomerId 

-- test zone

select * from dbo.Clients
select * from dbo.Orders


insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(1,N'Сметана', DATEADD(D,-10,GETdATE()))

insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(2,N'Молоко', DATEADD(D,-5,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(2,N'Хлеб', DATEADD(D,-5,GETdATE()))
                                                                               
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(3,N'Молоко', DATEADD(D,-5,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(3,N'Сметана', DATEADD(D,-15,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(3,N'Хлеб', DATEADD(D,-5,GETdATE()))

insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(4,N'Хлеб', DATEADD(D,-5,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(4,N'Яйца', DATEADD(D,-45,GETdATE()))

insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(5,N'Хлеб', DATEADD(D,-5,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(5,N'Яйца', DATEADD(D,-45,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(5,N'Молоко', DATEADD(D,-3,GETdATE()))

insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(6,N'Колбаса', DATEADD(D,-5,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(6,N'Яйца', DATEADD(D,-45,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(6,N'Молоко', DATEADD(D,-3,GETdATE()))
insert into [dbo].[Orders](CustomerId,ProductName,PurchaseDateTime) VALUES(6,N'Сметана', DATEADD(D,-3,GETdATE()))

Select c.*, o.*,COUNT(o.ProductName) from [dbo].Clients as c join 
    (select COUNT(*) as m,ord.ProductName,CustomerId  from [dbo].Orders  as ord  where ord.ProductName = N'Молоко' and ord.PurchaseDateTime > DATEADD(WW,-1,GETdATE()) group by ord.CustomerId) as o on o.CustomerId = c.CustomerId 
        

Select c.*, o.* from [dbo].Clients as c left join   [dbo].Orders as o on c.CustomerId = o.CustomerId 
where o.ProductName = N'Молоко' and o.PurchaseDateTime > DATEADD(WW,-1,GETdATE())
    --(select COUNT(*) as m,ord.ProductName,CustomerId  from [dbo].Orders  as ord  where ord.ProductName = N'Молоко' and ord.PurchaseDateTime > DATEADD(WW,-1,GETdATE()) group by ord.CustomerId) as o on o.CustomerId = c.CustomerId 
      

select COUNT(*),CustomerId  from [dbo].Orders where ProductName = N'Молоко' and PurchaseDateTime > DATEADD(WW,-1,GETdATE()) group by CustomerId


select COUNT(*) as cc,CustomerId  from [dbo].Orders where ProductName = N'Сметана' group by CustomerId 

select CustomerId  from [dbo].Orders where ProductName = N'Молоко' and PurchaseDateTime > DATEADD(WW,-1,GETdATE()) group by CustomerId
EXCEPT select CustomerId  from [dbo].Orders where ProductName = N'Сметана' group by CustomerId 




