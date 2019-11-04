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

