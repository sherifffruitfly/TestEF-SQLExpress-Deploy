CREATE TABLE [dbo].[TestTable] (
    [pkid]      INT           IDENTITY (1, 1) NOT NULL,
    [DataValue] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([pkid] ASC)
)

go

/*
insert into TestTable (DataValue) values ('woot!')

go

insert into TestTable (DataValue) values ('i live for this shit');

go


insert into TestTable (DataValue) values ('hi booboo!');

go


*/