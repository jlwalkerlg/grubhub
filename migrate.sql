alter table billing_accounts drop constraint "PK_billing_accounts";
alter table billing_accounts drop column id;
alter table billing_accounts rename column account_id to id;
alter table restaurants add column billing_account_id text;
update restaurants set billing_account_id = 'acct_1IIDXKPRU0NZyTXU';
alter table billing_accounts drop column restaurant_id;
truncate table billing_accounts;
insert into billing_accounts (id, billing_enabled) values ('acct_1IIDXKPRU0NZyTXU', true);
alter table billing_accounts add primary key (id);
alter table restaurants add constraint "IX_restaurants_billing_accounts_id" foreign key (billing_account_id) references billing_accounts(id) on delete set null;
create index "IX_restaurants_billing_accounts_id" on restaurants (billing_account_id);