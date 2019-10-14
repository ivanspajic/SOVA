-- Testing script

\timing
-- D1 & D2
select * from simple_search('nuget');

-- D3
SELECT * from exact_match('regions', 'blocks', 'constructors');

-- D4
SELECT * from best_match('constructors', 'regions', 'blocks');

-- D6
SELECT * from best_match_weighted('chocolate', 'regions');

-- D7
SELECT * from word_2_words('regions');

-- E.
-- testing with script of D3 without indexing
drop index if exists wi_word_inx;
SELECT * from exact_match('regions', 'blocks', 'constructors');

-- testing with script of D3 with indexing
create index wi_word_inx on wi (word);
SELECT * from exact_match('regions', 'blocks', 'constructors');
