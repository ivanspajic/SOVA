-- Data script

\timing

create table so_members (
	id int4 primary key not null unique,
  	display_name text not null,
	age int4,
	location text,
	creation_date timestamp not null
);

create table users (
	id serial primary key not null unique,
	name text not null unique,
	password text not null
);

create table history (
	id serial primary key not null unique,
	search_term text not null,
	search_date timestamp not null
);

create table submissions (
	id int4 primary key not null unique,
	body text not null,
	creation_date timestamp not null,
	score int4 not null,
	so_member_id int4 references so_members(id)
);

create table markings(
    submission_id int4 references submissions(id),
    user_id int4 references users(id)
);

create table questions (
	submission_id int4 primary key not null unique references submissions(id),
	closed_date timestamp,
	title text not null
);

create table answers (
	submission_id int4 primary key not null unique references submissions(id),
	parent_id int4 references questions(submission_id),
	accepted bool not null
);

create table annotations (
	annotation text not null,
	user_id int4 references users(id),
	submission_id int4 references submissions(id)
);

create table user_history (
	user_id int4 references users(id),
	history_id int4 references history(id)
);

create table tags (
	id SERIAL primary key not null unique,
	tag text not null unique
);
create table questions_tags (
	question_id int4 references questions(submission_id),
	tag_id int4 references tags(id)
);

create table link_posts (
	question_id int4 references questions(submission_id),
	link_post_id int4 references questions(submission_id)
);

create table comments (
	id int4 references submissions(id),
	submission_id int4 references submissions(id)
);

create table indexed_terms(
    submission_id int4 references submissions(id),
    is_title boolean not null,
    sen int4 not null,
    idx int4 not null,
    raw_text varchar not null,
    pos varchar not null,
    lemma varchar not null
);

insert into so_members (id, display_name, creation_date, location, age)
	select distinct ownerid, ownerdisplayname, ownercreationdate, ownerlocation, ownerage from posts_universal;

insert into so_members (id, display_name, creation_date, location, age)
	select distinct authorid, authordisplayname, authorcreationdate, authorlocation, authorage from comments_universal
   	 where authorid not in (select id from so_members);



insert into submissions (id, body, creation_date, score, so_member_id)
	select distinct id, body, creationdate, score, ownerid from posts_universal;


insert into submissions (id, body, creation_date, score, so_member_id)
	select distinct commentid * -1, commenttext, commentcreatedate, commentscore, authorid    	 from comments_universal;



insert into questions (submission_id, closed_date, title)
	select distinct submissions.id, posts_universal.closeddate, title from submissions join posts_universal on posts_universal.id = submissions.id
	where posts_universal.posttypeid = 1;



insert into answers (submission_id, parent_id, accepted)
	select distinct id, parentid, false
	from posts_universal
	where posttypeid = 2;

update answers set accepted = true
	where submission_id in (select acceptedanswerid from posts_universal);






insert into link_posts(question_id, link_post_id)
	select id, linkpostid
	from posts_universal
	where linkpostid is not null and linkpostid in (select submission_id from questions);

insert into comments (id, submission_id)
   select distinct commentid * (-1), postid from comments_universal;


-- Split concatenated tags into individual tags
DROP FUNCTION IF EXISTS collect_tags();
CREATE FUNCTION collect_tags()
	RETURNS TEXT AS
$$
DECLARE
	tags_result 	text DEFAULT '';
	tags_collection RECORD;
BEGIN
	FOR tags_collection IN
    	SELECT distinct tags
    	FROM posts_universal where tags is not null
    	LOOP
        	tags_result := tags_result || '::' || tags_collection.tags;
    	END LOOP;

	RETURN tags_result;
END;
$$ LANGUAGE plpgsql;


insert into tags(tag)
    select distinct unnest(string_to_array(collect_tags(), '::'));
delete from tags
where tag='';

DROP FUNCTION IF EXISTS fill_q_tags();
CREATE FUNCTION fill_q_tags()
	RETURNS VOID AS
$$
DECLARE
	tags_collection RECORD;
   	 tags_collection2 RECORD;
   	 tags_partial	text DEFAULT '';
BEGIN
	FOR tags_collection IN
    	SELECT distinct tags, id
    	FROM posts_universal where tags is not null
    	LOOP
   					 tags_partial := tags_collection.tags;

   					 for tags_collection2 in
   						 select unnest(string_to_array(tags_partial, '::'))
   						 loop
   						 insert into questions_tags(question_id, tag_id)
   							 select tags_collection.id, (select id from tags where tag like tags_collection2.unnest);
   						 end loop;

    	END LOOP;
END;
$$ LANGUAGE plpgsql;

select * from tags limit 5;

select fill_q_tags();

select * from tags limit 5;

-- Update comments to have negative ids as some have the same id as posts (questions/answers)
update words set id = id * (-1)
	where tablename='comments';

-- Create an inverted index table (D2)
drop table if exists wi;
create table wi as
select id, lower(word) word from words
where word ~* '^[a-z][a-z0-9_]+$'
and tablename = 'posts' and (what='title' or what='body')
group by id,word;

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

select * from wi_weighted limit 5;

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

select * from wi_weighted limit 5;

-- drop original tables
drop table if exists posts_universal;
drop table if exists comments_universal;
