
--事件存储表结构脚本
create table EventStore(
aggregateId varchar not null,
version  int not null,
events binary not null,
created datetime not null,
);

--快照存储表结构脚本
create table snapshoting(
  aggregateId varchar not null,
  version int not null,
  aggregate binary not null,
  created datetime not null
)

