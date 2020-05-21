use test;

show tables;

drop table cartlist;

delete from cartlist;

create table shop(
snum int auto_increment primary key,
sname varchar(15) not null,
sid varchar(15) not null unique,
spw varchar(25) not null
);

create table shopmember(
smnum int auto_increment primary key,
smid varchar(15) not null unique,
smpw varchar(15) not null,
smname varchar(10) not null,
snum int not null,
smauth int not null,
foreign key (snum) references shop(snum)
);

create table userinfo(
unum int auto_increment primary key,
uid varchar(15) not null unique,
upw varchar(25) not null,
uname varchar(20) not null,
uhp int not null unique
);

create table customer(
snum int,
cnum int auto_increment primary key,
onum int,
foreign key (snum) references shop(snum),
foreign key (onum) references orderlist(onum)
);

create table orderlist (
snum int,
onum int auto_increment primary key,
otablenum int,
otime timestamp default NOW(),
ostate int default 1,
ocookstate int default 1,
foreign key (snum) references shop(snum)
);

create table orderdetail (
onum int,
mnum int,
mquantity int,
foreign key (onum) references orderlist(onum),
foreign key (mnum) references menu(mnum)
);

create table menu (
snum int not null,
mnum int auto_increment primary key,
mname varchar(25) not null,
mprice int not null,
mcontent varchar(100) default '냠냠마시졍',
foreign key (snum) references shop(snum)
);

create table shoptable (
stablenum int primary key,
stablestate int not null default 0,
stablecookstate int not null default 0
);

create table cartlist (
mnum int,
mname varchar(50),
mprice int,
mquantity int,
otablenum int
);

show tables;

select * from shop;

select * from shopmember;

select * from orderlist order by otime ASC;

select * from menu order by mnum ASC;

select * from orderdetail;

select * from shoptable order by stablenum ASC;

select * from customer;

select * from userinfo;

SELECT * FROM CARTLIST ORDER BY MNUM ASC;

SELECT SUM(MPRICE * MQUANTITY) AS TOTALPRICE FROM CARTLIST;

---------------------------------------------------
#샵폼 관련 쿼리문

#orderlist와 orderdetail과 menu 3개 테이블 조인
select * from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum;

#샵폼에 나타낼 활성화된 테이블 내용 (활성화된 주문, 활성화된 테이블)
select orderdetail.onum, menu.mname, orderdetail.mquantity from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum where (orderlist.ostate = 1 or orderlist.ocookstate =1) and otablenum = 6;

#샵폼에 표시해줄 총 가격
select sum(orderdetail.mquantity * menu.mprice) as totalprice from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum where (orderlist.ostate = 1 or orderlist.ocookstate =1) and otablenum = 2;

#활설화된 테이블 번호 가져오기(shoptable기준)
select stablenum from shoptable where stablestate = 1;


-------------------------------------------------
#서브포스기 관련 쿼리문

#요리 안나간 테이블 번호 가져오기(shoptable기준)
select stablenum from shoptable where stablecookstate = 1;

#요리 안나간 테이블 번호의 주문넘버 시간 순서로 가져오기
select onum, otime from orderlist where ocookstate = 1 order by otime ASC;

#요리 안나간 메뉴 정보 주문넘버가져오기
select otablenum, mname, mquantity from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum where orderlist.ocookstate = 1 and orderlist.onum = 1;

#주문 요리들 모두 완성하였을 때 요리 상태 완료로 바꾸기(테이블넘버로)
update orderlist set ocookstate = 0 where ocookstate = 1 and otablenum = 4;
update shoptable set stablecookstate = 0 where stablecookstate = 1 and stablenum = 4;

---------------------------------------------------
#결제폼 관련 쿼리문

#활성화된 테이블 클릭시 나올 주문 상세 내역의 listview에 넣어야할 문 ( 수정 필요 )
select mname, mquantity, (mprice*mquantity) as price from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum where ostate = 1 and otablenum = 4;

#총 가격
select sum(mquantity * mprice) as totalprice from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum inner join menu on orderdetail.mnum = menu.mnum where ostate = 1 and otablenum = 4;

#메뉴 버튼 붙이기
select mname, mprice from menu;

---------------------------------------------------
#장바구니 총합
select sum(mprice * mquantity) as totalprice from cartlist;


---------------------------------------------------
#웹 지도 남은 자리
select count(stablestate) as remaintablenum from shoptable where stablestate = 0;


select stablenum from shoptable where stablestate = 1;

insert into userinfo(uid, upw, uname, uhp) values ('user', '1234', '홍길동', 01012341234);

insert into shopmember(smid, smpw, smname, snum, smauth) values ('eeditor', 'ed1234', 'ed1', 1, 1);

insert into shop(sname, sid, spw) values ('에델리아', 'edelia', 'e1234');

insert into customer(snum) values (1);

insert into orderlist(snum, otablenum) values (1, 4);

insert into menu(snum, mname, mprice) values (1, '트레비', 4000);

insert into menu(snum, mnum, mname, mprice) values (1, 2, '레보스', 4000);

insert into orderdetail(onum, mnum, mquantity) values (2, 6, 1);

insert into shoptable values (10, 0, 0);

update orderlist set ocookstate = 1 where onum = 1;

update orderlist set ocookstate = 1 where otablenum = 2;

update shoptable set stablestate = 1 where stablenum = 2;

update shoptable set stablestate = 0 where stablenum = 2;

update shoptable set stablecookstate = 1 where stablenum = 2;

update shoptable set stablecookstate = 0 where stablenum = 1;

update menu set mname = "초코프라프치노" where mname = "초코 프라프치노";

update customer set onum = 2 where cnum = 2;

delete from menu where mnum = 2;

delete from orderlist where onum = 5;

delete from orderdetail where mnum = 2;

delete from shoptable where stablenum = 3;

alter table cartlist alter otablenum set default null;

desc cartlist;

SELECT * FROM MENU ORDER BY MNUM ASC;

commit;