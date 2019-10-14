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

select fill_q_tags();

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

-- drop original tables
drop table if exists posts_universal;
drop table if exists comments_universal;
