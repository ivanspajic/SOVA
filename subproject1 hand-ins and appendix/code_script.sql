-- Code script

\timing

-- create a procedure to log the search term (i.e insert to history table)
create or replace function log_search(string_to_log text)
    returns void as
$$
begin
    insert into history(search_term, search_date)
    select string_to_log, now();
end
$$ language plpgsql;

-- D1. Simple search and adding it to history

-- create a function to do the simple_search
create or replace function simple_search(search_string text)
    returns table
            (
                post_id int4,
                body    text
            )
as
$$
begin
    return query
        select s.id, s.body
        from submissions s,
             (select id from wi where word like '%' || search_string || '%') t
        where s.id = t.id;
    -- call the previously created procedure
    perform log_search(search_string);
end ;
$$ language plpgsql;

--d3
drop function if exists exact_match();
CREATE OR REPLACE FUNCTION exact_match(VARIADIC w text[])
RETURNS table(
    postid int4,
    postbody text
)
AS $$
DECLARE
w_elem text;
BEGIN
create table temp_table(post_id int4);
FOREACH w_elem IN ARRAY w
LOOP
insert into temp_table(post_id)
    (select distinct id from wi where wi.word = w_elem);
perform log_search(w_elem);
END LOOP;
FOREACH w_elem IN ARRAY w
LOOP
delete from temp_table
    where post_id not in (select distinct id from wi where wi.word = w_elem);
END LOOP;
return query
    (select distinct post_id, body from temp_table join submissions on post_id=submissions."id");
drop table temp_table;
END $$
LANGUAGE 'plpgsql';


--d4
drop function if exists best_match();
CREATE OR REPLACE FUNCTION best_match(VARIADIC w text[])
    RETURNS table
            (
                postid int4,
                rank   bigint,
                body   text
            )
AS
$$
DECLARE
    w_elem text;
BEGIN
    create table temp_table
    (
        postid int4
    );
    FOREACH w_elem IN ARRAY w
        LOOP
            insert into temp_table(postid)
                    (select distinct id from wi where wi.word = w_elem);
            perform log_search(w_elem);
        END LOOP;
    return query
        (select temp_table.postid, count(temp_table.postid) as rank, submissions.body
         from temp_table
                  join submissions on temp_table.postid = submissions.id
         group by temp_table.postid, submissions.body
         order by rank desc);
    drop table temp_table;
END
$$
    LANGUAGE 'plpgsql';

--d5
drop table if exists wi_weighted;
create table wi_weighted
(
    id     int4,
    word   varchar,
    weight decimal(3, 3)
);


insert into wi_weighted(id, word, weight)
    (select id, lower(word) word, 0
     from words
     where word ~* '^[a-z][a-z0-9_]+$'
       and tablename = 'posts'
       and (what = 'title' or what = 'body')
     group by id, word);

--remove stopwords
delete from wi_weighted
where wi_weighted.word in (select stopwords.word from stopwords);

-- Assign weight based on if it is a title(0.3) or body(0.1)
update wi_weighted
set weight = 0.1
where (id, word) in (select wiw.id, wiw.word
               from wi_weighted wiw
                        join words w on wiw.word = lower(w.word)
                   and wiw.id = w.id
               where what = 'body'
                 and tablename = 'posts');

update wi_weighted
set weight = 0.3 where weight = 0;

-- Multiply weight if it is a noun by 1.5
update wi_weighted
set weight = weight * (1.5)
where (id, word) in (select wiw.id, wiw.word
               from wi_weighted wiw
                        join words w on wiw.word = lower(w.word)
                   and wiw.id = w.id
               where pos like 'N%');

-- Multiply weight if it is a verb by 1.2
update wi_weighted
set weight = weight * (1.2)
where (id, word) in (select wiw.id, wiw.word
               from wi_weighted wiw
                        join words w on wiw.word = lower(w.word)
                   and wiw.id = w.id
               where pos like 'V%');

--apply tfidf
drop function if exists adjust_weights();
create or replace function adjust_weights()
returns void as
$$

begin

create table temp_table as (select *, (word_freq/cast(word_count as decimal)) as ratio
from
(select id, count(word) as word_count
from wi_weighted
group by id) as t1
natural join
(select id, word, count(word) as word_freq, weight
from wi_weighted
group by id, word, weight) as t2
natural join
(select word, count(distinct id) as post_count
from wi_weighted
group by wi_weighted.word) as t3);



update temp_table
	set weight = weight  + log(1 + ratio) * (1 / post_count);

UPDATE wi_weighted
SET	weight = temp_table.weight
FROM   temp_table
WHERE  wi_weighted.id = temp_table.id  and wi_weighted.word = temp_table.word;

drop table temp_table;
end;
$$ LANGUAGE plpgsql;

select adjust_weights();

--d6
drop function if exists best_match_weighted();
CREATE OR REPLACE FUNCTION best_match_weighted(VARIADIC w text[])
	RETURNS table
        	(
            	postid int4,
            	rank   decimal,
            	body   text
        	)
AS
$$
DECLARE
	w_elem text;
BEGIN
	create table temp_table
	(
    	postid int4 unique,
   			 rank   decimal
	);
	FOREACH w_elem IN ARRAY w
    	LOOP
        	insert into temp_table(postid, rank)
                	(select distinct id, 0 from wi_weighted where wi_weighted.word = w_elem
   									 and id not in (select temp_table.postid from temp_table));

   					 update temp_table
   					 set rank = temp_table.rank + wi_weighted.weight
   					 from wi_weighted
   					 where temp_table.postid=wi_weighted.id and wi_weighted.word=w_elem;

        	perform log_search(w_elem);
    	END LOOP;
	return query
    	(select distinct temp_table.postid, temp_table.rank as rank, submissions.body
     	from temp_table
              	join submissions on temp_table.postid = submissions.id
     	group by temp_table.postid, submissions.body, temp_table.rank
     	order by rank desc);
	drop table temp_table;
END
$$
	LANGUAGE 'plpgsql';


--d7
drop function if exists word_2_words();
CREATE OR REPLACE FUNCTION word_2_words(VARIADIC w text[])
    RETURNS table
            (
                weight bigint,
                word   text
            )
AS
$$
DECLARE
    w_elem text;
BEGIN
    create table temp_table
    (
        postid int4 unique
    );
    FOREACH w_elem IN ARRAY w
        LOOP
            insert into temp_table(postid)
                (select distinct id
                 from wi
                 where wi.word = w_elem
                   and id not in (select temp_table.postid from temp_table));

            perform log_search(w_elem);
        END LOOP;

    return query
        (select count(wi.word) as weight, wi.word
         from wi
         where wi.id in (select temp_table.postid from temp_table)
           and wi.word not in (select stopwords.word from stopwords)
         group by wi.word
         order by weight desc);

    drop table temp_table;
END
$$
    LANGUAGE 'plpgsql';
