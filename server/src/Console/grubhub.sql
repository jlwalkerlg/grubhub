-- -------------------------------------------------------------
-- TablePlus 3.12.5(364)
--
-- https://tableplus.com/
--
-- Database: grubhub
-- Generation Time: 2021-03-28 11:35:16.4830
-- -------------------------------------------------------------


DROP TABLE IF EXISTS "public"."basket_items";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."basket_items" (
    "id" int4 NOT NULL,
    "menu_item_id" uuid NOT NULL,
    "quantity" int4 NOT NULL,
    "basket_id" int4,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."baskets";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."baskets" (
    "id" int4 NOT NULL,
    "user_id" uuid NOT NULL,
    "restaurant_id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."billing_accounts";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."billing_accounts" (
    "id" text NOT NULL,
    "restaurant_id" uuid NOT NULL,
    "billing_enabled" bool NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."cuisines";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."cuisines" (
    "name" text NOT NULL,
    PRIMARY KEY ("name")
);

DROP TABLE IF EXISTS "public"."events";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."events" (
    "id" int8 NOT NULL,
    "occured_at" timestamp NOT NULL,
    "type" text NOT NULL,
    "json" jsonb NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."menu_categories";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."menu_categories" (
    "id" uuid NOT NULL,
    "name" text NOT NULL,
    "is_deleted" bool NOT NULL DEFAULT false,
    "menu_id" int4 NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."menu_items";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."menu_items" (
    "id" uuid NOT NULL,
    "name" text NOT NULL,
    "description" varchar(280),
    "price" int4 NOT NULL,
    "is_deleted" bool NOT NULL DEFAULT false,
    "menu_category_id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."menus";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."menus" (
    "id" int4 NOT NULL,
    "restaurant_id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."order_items";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."order_items" (
    "id" int4 NOT NULL,
    "menu_item_id" uuid NOT NULL,
    "name" text NOT NULL,
    "price" int4 NOT NULL,
    "quantity" int4 NOT NULL,
    "order_id" text NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."orders";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."orders" (
    "id" text NOT NULL,
    "user_id" uuid,
    "restaurant_id" uuid,
    "delivery_fee" int4 NOT NULL,
    "service_fee" int4 NOT NULL,
    "status" text NOT NULL,
    "mobile_number" text NOT NULL,
    "address_line1" text,
    "address_line2" text,
    "city" text,
    "postcode" text,
    "placed_at" timestamp NOT NULL,
    "confirmed_at" timestamp,
    "accepted_at" timestamp,
    "delivered_at" timestamp,
    "rejected_at" timestamp,
    "cancelled_at" timestamp,
    "payment_intent_id" text NOT NULL,
    "payment_intent_client_secret" text NOT NULL,
    "number" int4 NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."restaurant_cuisines";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."restaurant_cuisines" (
    "restaurant_id" uuid NOT NULL,
    "cuisine_name" text NOT NULL,
    PRIMARY KEY ("restaurant_id","cuisine_name")
);

DROP TABLE IF EXISTS "public"."restaurants";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."restaurants" (
    "id" uuid NOT NULL,
    "manager_id" uuid NOT NULL,
    "name" text NOT NULL,
    "description" varchar(400),
    "phone_number" text NOT NULL,
    "address_line1" text,
    "address_line2" text,
    "city" text,
    "postcode" text,
    "latitude" float4,
    "longitude" float4,
    "status" text NOT NULL,
    "monday_open" interval,
    "monday_close" interval,
    "tuesday_open" interval,
    "tuesday_close" interval,
    "wednesday_open" interval,
    "wednesday_close" interval,
    "thursday_open" interval,
    "thursday_close" interval,
    "friday_open" interval,
    "friday_close" interval,
    "saturday_open" interval,
    "saturday_close" interval,
    "sunday_open" interval,
    "sunday_close" interval,
    "minimum_delivery_spend" int4 NOT NULL,
    "delivery_fee" int4 NOT NULL,
    "max_delivery_distance_in_km" float4 NOT NULL,
    "estimated_delivery_time_in_minutes" int4 NOT NULL,
    PRIMARY KEY ("id")
);

DROP TABLE IF EXISTS "public"."users";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."users" (
    "id" uuid NOT NULL,
    "first_name" text NOT NULL,
    "last_name" text NOT NULL,
    "email" text NOT NULL,
    "password" text NOT NULL,
    "mobile_number" text,
    "address_line1" text,
    "address_line2" text,
    "city" text,
    "postcode" text,
    "role" text NOT NULL,
    PRIMARY KEY ("id")
);

INSERT INTO "public"."basket_items" ("id", "menu_item_id", "quantity", "basket_id") VALUES
(1, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 2, 1);

INSERT INTO "public"."baskets" ("id", "user_id", "restaurant_id") VALUES
(1, '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01');

INSERT INTO "public"."billing_accounts" ("id", "restaurant_id", "billing_enabled") VALUES
('acct_1IID2yPTYOwNQdvb', '015caf13-8252-476b-9e7f-c43767998c02', 't'),
('acct_1IIDXKPRU0NZyTXU', '015caf13-8252-476b-9e7f-c43767998c01', 't');

INSERT INTO "public"."cuisines" ("name") VALUES
('Breakfast'),
('Café'),
('Spanish'),
('Tapas');

INSERT INTO "public"."menu_categories" ("id", "name", "is_deleted", "menu_id") VALUES
('140d5edf-ee89-40c7-b95a-a7df88ea3038', 'Vegetarian Sandwich', 'f', 2),
('19cca5e4-b0b2-4cb0-9cea-2af11dd3b854', 'Toasties', 'f', 2),
('2351ad34-5170-4ca6-b94b-007f580371f7', 'Homemade Pasta''s', 'f', 1),
('2381e7d9-b192-4169-b222-de6d5311a7b0', 'Jacket Potatoes', 'f', 1),
('3f1bec1b-a041-4a9b-8303-e84fc1d6cfc0', 'Tea & Hot Choc', 'f', 2),
('408467b8-0857-4a6e-a75e-30cb07586b76', 'Gourmet Sandwiches', 'f', 1),
('42b99c7c-5248-4364-b6ce-de9ba3585a96', 'Cold Coffee', 'f', 1),
('4c8329a1-ef50-4a11-96e3-953694ed65ea', 'Fresh Salad Bowls', 'f', 2),
('50e97eca-62bf-41a5-9999-a3bfe50e81f2', 'Cold Drinks', 'f', 2),
('60d52634-6df2-4234-9e94-6bf2a1083ed0', 'Flatbread Pizza', 'f', 1),
('6b254232-58c2-496c-b6ff-59d55abf1fec', 'Grilled Sandwiches', 'f', 2),
('73b8b051-c988-45ab-9396-cfdbb5a79751', 'Fruit Teas', 'f', 2),
('76a8b001-bc16-4179-bc0e-b1de9d931433', 'Smoothies', 'f', 1),
('975b6d34-4aa5-41ef-87d5-efe6e550ce35', 'Sharing Plates', 'f', 1),
('9a6dd1c8-8a96-4114-9f04-d465405616dc', 'Coffee', 'f', 1),
('9de1eb81-8cb3-4b5e-9f7c-c744c0eb4cc1', 'Jacket Potatoes', 'f', 2),
('a1540e1f-51ba-4c28-b7ff-dc619d17bf10', 'Kiddies Corner', 'f', 1),
('a7519c79-5e5e-4b9f-ac49-94bbb1f97968', 'Cold Coffee', 'f', 2),
('b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5', 'Fresh Salad Bowls', 'f', 1),
('bb69f5e7-634a-4d23-97a6-68aaf49ae447', 'Homemade Pasta''s', 'f', 2),
('c32cbf15-8dd8-4df0-bb60-4944fc86d9d7', 'All Day Breakfast', 'f', 1),
('cabe6637-6e0a-4c95-83e3-e3e95d5d2e69', 'Flatbread Pizza', 'f', 2),
('d3887c25-3024-49ec-8906-e3f00f805ecc', 'Gourmet Sandwiches', 'f', 2),
('d69155ce-a1b3-443c-9dca-14ed241ee97b', 'Toasties', 'f', 1),
('da571921-af39-4c43-9391-6d54bc4504e3', 'Tea & Hot Choc', 'f', 1),
('da6e8911-0a0a-4c21-ab66-837e6ee5c18a', 'Smoothies', 'f', 2),
('ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc', 'Sharing Plates', 'f', 2),
('eb89ba2e-377c-4579-bbdd-f38dec043417', 'Quiche Meals', 'f', 2),
('eca982ef-2f85-4ef3-8d30-fd7aa5a487ad', 'All Day Breakfast', 'f', 2),
('f0eccf5e-5414-4fff-abbe-7cc016f55398', 'Vegetarian Sandwich', 'f', 1),
('f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c', 'Grilled Sandwiches', 'f', 1),
('f2fa4032-56e8-4b60-ad48-fb195c032f92', 'Milkshakes', 'f', 1),
('f44713b0-e341-43a6-9b28-333be2016d81', 'Milkshakes', 'f', 2),
('f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1', 'Coffee', 'f', 2),
('fae067c0-6fc8-4e7b-8a3e-0e28dfebf2c2', 'Kiddies Corner', 'f', 2);

INSERT INTO "public"."menu_items" ("id", "name", "description", "price", "is_deleted", "menu_category_id") VALUES
('009d69ca-8844-4cfd-b4ab-cb9ddfade0c9', 'Houmous Dip', 'Served with pitted olives, homemade houmous topped with extra virgin olive oil, mixed herbs, cuts of bloomer & salad.', 495, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('021722f5-9264-45ec-9626-72f768d27e5b', 'Tuna Crunch Jacket Potato', 'Tuna flakes mixed with red onions, sweetcorn, lettuce and luxury mayonnaise.', 595, 'f', '9de1eb81-8cb3-4b5e-9f7c-c744c0eb4cc1'),
('0341729c-1bd8-4b0c-ad3a-a12cbed3d032', 'Pesto Flatbread Pizza', 'Organic pesto, bell peppers, cherry tomatoes, green chillies, red onions, oregano, black pepper topped with 3 cheeses, feta, cheddar and mozzarella.', 695, 'f', 'cabe6637-6e0a-4c95-83e3-e3e95d5d2e69'),
('0483412c-9c23-42f2-b152-d42c819f655b', 'Veggie Breakfast', 'Vegetarian sausages, hash browns, fried egg, homemade tomato sauce, mushrooms, baked beans with toast and butter. (40p Extra for scrambled egg)', 750, 'f', 'c32cbf15-8dd8-4df0-bb60-4944fc86d9d7'),
('04fe5744-925c-4669-83b5-25f647b19bc8', 'Tuna Crunch Gourmet Cold Sandwich', 'Tuna flakes mixed with red onions, sweetcorn, lettuce and luxury mayonnaise.', 595, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('055bfe25-6831-43c2-a78b-66620f26b08b', 'Hazelnut Mocha', 'Espresso mixed with chocolate, hazelnut syrup and topped with steamed milk & fresh vanilla whipped cream.', 395, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('05955f36-a419-4d3c-b92d-b3ef8f6715ec', 'Machiatto', 'An espresso with a dash of frothed milk.', 225, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('06588cab-7f47-40e1-8d09-4a9621a7639c', 'Beef Salad', 'Strips of roast beef seasoned with salt, black pepper, extra virgin olive oil and hint of chilli sauce. Mixed with red onions, carrots, sweetcorn, lettuce, cucumber, green olives and cherry tomatoes.', 899, 'f', '4c8329a1-ef50-4a11-96e3-953694ed65ea'),
('06948c4a-4827-4541-b13b-453047f38fc0', 'Chicken Caesar Fresh Salad Bowl', 'Lettuce, cucumber, red onions, sweetcorn, green olives, cherry tomatoes, julien carrots topped with garlic infused chicken breast, dash of extra virgin olive oil and Caesar sauce.', 799, 'f', '4c8329a1-ef50-4a11-96e3-953694ed65ea'),
('0af1fae2-41e2-4e3d-8c57-e0fa85037674', 'Chicken Toasty', 'Chicken breast, cherry tomatoes, red onions, jalapenos, black pepper, extra virgin olive oil and cheese.', 550, 'f', '19cca5e4-b0b2-4cb0-9cea-2af11dd3b854'),
('0dc21e00-6f6f-475e-a957-fef531db0e4f', 'Meat Flatbread Pizza', 'Grilled mincemeat, bell peppers, cherry tomatoes, green chillies, red onions, sprinkle of olive oil, topped with 3 cheeses, feta, cheddar and mozzarella.', 675, 'f', 'cabe6637-6e0a-4c95-83e3-e3e95d5d2e69'),
('0dc255e0-cdf1-468a-9ecd-50eb510deb3c', 'Veggie Supreme Vegetarian Grilled Sandwich', 'Cherry tomatoes, olives, sweetcorn, red onions, topped with melted cheddar cheese. Add jalapenos (40p) for some extra spice.', 495, 'f', 'f0eccf5e-5414-4fff-abbe-7cc016f55398'),
('0dfde2d4-711e-4660-b9ed-b8c4313c844a', 'Espresso Single', 'An instant hit, guaranteed to wake you up!', 195, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('0e599fdf-8e35-4007-ab92-0a1a5abb532f', 'Berry Go Round - 500ml', 'Raspberry, blackberry, strawberry', 450, 'f', '76a8b001-bc16-4179-bc0e-b1de9d931433'),
('0ebb8363-2a61-4d55-88b5-d022202a93cb', 'Pesto Pasta', 'Penne pasta cooked in extra virgin olive oil, pesto and fresh basil topped with Parmesan. Why not ask for melted cheese on top?', 750, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('15fc82b2-eb25-491f-b436-436fafea45fc', 'Mexicana Grilled Sandwich', 'Spicy chilli con carne, jalapenos, kidney beans, tomatoes, red onion with melted cheese. If you don’t want jalapenos, please ask!', 550, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('16608e53-9a2c-4c9e-a81a-07cb8f9ed24c', 'Jacket Wedges', 'Served with our homemade sour cream.', 450, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('18cbba97-e6fa-42c2-9d8a-ee5c59484a98', 'Alpollo', 'Garlic infused chicken breast cooked in a scrumptious creamy sauce. You can add mushrooms and jalapenos for 80p extra.', 850, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('193335ca-9ce6-4b3a-b48a-3126f4875022', 'Tuna Melt Grilled Sandwich', 'Tuna flakes, green olives, red onions, black pepper, mayo, dash of extra virgin olive oil and melted cheese. Ask for any other toppings , jalapeno, sweetcorn, tomatos (40p per topping)', 625, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('1a743472-a1c3-4254-8277-4740979d1d8f', 'Tropical Fusion - 500ml', 'Mango, strawberry', 450, 'f', '76a8b001-bc16-4179-bc0e-b1de9d931433'),
('1beaec22-a565-401c-a235-933e6dd0bbba', 'Nachos with Chilli', 'Add melted cheese for £1', 695, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('1d4eb959-d850-4953-a63a-d84eff0b3cec', 'Hot Chocolate', 'Add homemade whipped cream for an additional 40p', 399, 'f', 'da571921-af39-4c43-9391-6d54bc4504e3'),
('1da40482-3c92-409a-8855-30f0d8cfc7b3', 'Chicken Flatbread Pizza', 'Marinated chicken pieces, bell peppers, cherry tomatoes, sweetcorn, green chillies, red onions, topped with 3 cheeses, feta, cheddar and mozzarella.', 675, 'f', 'cabe6637-6e0a-4c95-83e3-e3e95d5d2e69'),
('1f3a8b46-67c0-4528-b9fb-9dfe14046a53', 'Coronation Chicken Jacket Potato', 'Roast chicken breast diced and mixed with homemade curry spice, chopped apricots & mixed chopped nuts in a creamy mayonnaise.', 650, 'f', '9de1eb81-8cb3-4b5e-9f7c-c744c0eb4cc1'),
('1f89fd09-0e4e-4db3-b17c-3996ccbca6af', 'Greek Fresh Salad Bowl', 'Traditional Greek feta served on a bed of crisp lettuce, cucumber, cherry tomatoes, green olives, red onions mixed with extra virgin olive oil and Italian herbs.', 750, 'f', 'b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5'),
('20a9d9bd-5923-4f62-a823-f88085eeac66', 'Pasta Aglio Olio Peperoncino', 'Classic Italian pasta with garlic, extra virgin olive oil, red chilli flakes, parmesan and basil. Add melted cheese for £1.', 750, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('20b87de9-a2be-4655-bedb-dc5612f88624', 'Mexicana Grilled Sandwich', 'Spicy chilli con carne, jalapenos, kidney beans, tomatoes, red onion with melted cheese. If you don’t want jalapenos, please ask!', 550, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('21070578-24e7-480f-9e25-1e7ded6c4ad5', 'Coronation Chicken Gourmet Cold Sandwich', 'Roast chicken breast diced and mixed with homemade curry spice, chopped apricots & mixed chopped nuts in a creamy mayonnaise. ', 595, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('223d0943-c40e-4f71-bd02-bb2ae39bea14', 'Coronation Chicken Gourmet Cold Sandwich', 'Roast chicken breast diced and mixed with homemade curry spice, chopped apricots & mixed chopped nuts in a creamy mayonnaise. ', 595, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('23af53eb-bdc0-491d-8d1e-3581ba221838', 'Cappuccino', 'An espresso topped with equal amounts of hot milk & froth topped with a sprinkle of chocolate.', 399, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('23c387d5-814b-494d-9419-9fd46423de04', 'Pesto Chilli Chicken', 'Chicken cooked in garlic infused extra virgin olive oil, pesto, cherry tomatoes, green chillies and fresh basil. Fancy melted cheese on top? Just ask', 799, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('23e390a7-96fb-49e7-a429-693366ddddce', 'Tuna Crunch Jacket Potato', 'Tuna flakes mixed with red onions, sweetcorn, lettuce and luxury mayonnaise.', 595, 'f', '2381e7d9-b192-4169-b222-de6d5311a7b0'),
('23ef4320-a83d-4c84-8f42-96a87ab70ffa', 'Cheese Savoury Gourmet Cold Sandwich', 'Mild mature cheddar mixed with red onions, sweetcorn, lettuce and luxury mayonnaise', 550, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('26f28d65-21dc-445e-a5f4-fb6076081b6e', 'Smorgasbord Special Salad', 'Tuna mayo, smoked salmon, cheddar cheese, lettuce, apricots, red onions, tomatoes, carrots, olives, mixed nuts, sweetcorn topped with extra virgin olive oil and ceasar dressing.', 995, 'f', '4c8329a1-ef50-4a11-96e3-953694ed65ea'),
('2d302d7c-baf5-4870-bba1-1f26e3309b42', 'Americano', 'An espresso topped with hot water & served with or without milk, a stronger alternative to filtered coffee', 399, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('32aac9ae-0304-479a-8b57-eb7d327e854f', 'Coco Loco - 500ml', 'Mango, pineapple, coconut, lime, mint', 450, 'f', '76a8b001-bc16-4179-bc0e-b1de9d931433'),
('36c6f057-a31e-47ba-a76b-4017227ef9b4', 'Tropical Fusion - 500ml', 'Mango, strawberry', 450, 'f', 'da6e8911-0a0a-4c21-ab66-837e6ee5c18a'),
('377d2a79-6abe-49d7-89af-b5b10491feea', 'Beef Salad', 'Strips of roast beef seasoned with salt, black pepper, extra virgin olive oil and hint of chilli sauce. Mixed with red onions, carrots, sweetcorn, lettuce, cucumber, green olives and cherry tomatoes.', 899, 'f', 'b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5'),
('37acd047-07d6-43ea-ba48-d99f9b31afd6', 'Americano', 'An espresso topped with hot water & served with or without milk, a stronger alternative to filtered coffee', 399, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('398850ac-3c3a-4f14-af48-1a1c78a6a4dd', 'Detox Zing - 500ml', 'Blueberry, carrot, ginger, banana, courgette', 450, 'f', '76a8b001-bc16-4179-bc0e-b1de9d931433'),
('3ab6942c-0b19-4d02-a572-d47f50f7a178', 'Salmon Classic Gourmet Cold Sandwich', 'Smoked salmon, cucumber, lettuce, black pepper, dash of extra virgin olive oil topped with creamy homemade dill and chive sauce.', 670, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('3d94ae9b-edc6-4dea-ae9f-4e6b26adb446', 'Chicken Goujons Kiddies Corner', 'Chicken Goujons, Chips and Beans or Salad', 550, 'f', 'fae067c0-6fc8-4e7b-8a3e-0e28dfebf2c2'),
('3f1221b1-19d8-4fb2-8772-617d8e0b6fd9', 'Jacket Wedges', 'Served with our homemade sour cream.', 450, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('42209f7d-6bbf-4d17-a80c-e6e817bd4c61', 'Tomato & Cheese Melt Vegetarian Grilled Sandwich', 'Our version of the classic melt! Filled with mature cheddar, mozzarella topped with caramelized onion and oregano. Why not add some jalapenos (40p) for a bit of spiciness!', 575, 'f', 'f0eccf5e-5414-4fff-abbe-7cc016f55398'),
('427c9561-11f4-4c92-baa4-4cf6beb4f987', 'Roast Beef (Cold) Gourmet Cold Sandwich', 'Strips of beef slow roasted with red onions, lettuce, tomatoes, creamy mayonnaise, jalapenos, freshly ground black pepper and dijon mustard. If you are not a fan of mustard, please ask to exclude it or replace with mayo', 595, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('42c49045-be1d-4711-84be-aef67f9e2ba7', 'Veg Samosa', '3 Veg samosa’s salad & mint sauce.', 275, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('4327ef0a-4a27-4a1b-9e9a-dc122b636e94', 'Pasta Aglio Olio Peperoncino', 'Classic Italian pasta with garlic, extra virgin olive oil, red chilli flakes, parmesan and basil. Add melted cheese for £1.', 750, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('43343101-8186-4ba3-98eb-d9dfbacadb41', 'Coco Loco - 500ml', 'Mango, pineapple, coconut, lime, mint', 450, 'f', 'da6e8911-0a0a-4c21-ab66-837e6ee5c18a'),
('44156835-2595-4722-ad67-641b80cf1aec', 'Coronation Chicken Jacket Potato', 'Roast chicken breast diced and mixed with homemade curry spice, chopped apricots & mixed chopped nuts in a creamy mayonnaise.', 650, 'f', '2381e7d9-b192-4169-b222-de6d5311a7b0'),
('44367084-c87f-4f3e-9770-c35e0d6bc25f', 'Tuscani Vegetarian Grilled Sandwich', 'Pesto, mushrooms and oozing with melted cheese. Highly recommended - absolutely delicious! Add jalapenos (40p) for some extra spice', 575, 'f', 'f0eccf5e-5414-4fff-abbe-7cc016f55398'),
('461bc309-2bf5-450e-87d5-7672a4813000', 'Ambro’s Belly Buster Grilled Sandwich', 'Named after our valued customer, ambro who made this sandwich and it’s been a hit with all our customers. Spicy beef sausage meat, dijon mustard, caramelised onions, mushrooms, jalapenos, scrambled egg and melted cheese.', 785, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('46fad7f6-f211-4a9f-8940-006b3cb7a5da', 'Tuscani Vegetarian Grilled Sandwich', 'Pesto, mushrooms and oozing with melted cheese. Highly recommended - absolutely delicious! Add jalapenos (40p) for some extra spice', 575, 'f', '140d5edf-ee89-40c7-b95a-a7df88ea3038'),
('4d93f15e-3382-449f-a7c2-054d624fd95f', 'Vanilla Latte', 'A long coffee made with 2 shots of espresso, steamed milk & a topping of small froth and vanilla syrup', 325, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('540365b4-6f82-4f43-adcc-148c54f5d748', 'Nachos with Chilli', 'Add melted cheese for £1', 695, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('560d2e53-841d-4bae-b7ba-59878c222853', 'Bolognaise', 'Mildly spiced bolognaise sauce cooked to perfection over 4 hours.', 799, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('5782af28-b950-4b13-b32b-dc24707ae746', 'Spicy Feta Gourmet Cold Sandwich', 'Crumbly feta cheese mixed with extra virgin olive oil, red onions, green olives, cherry tomatoes, jalapenos, salsa and mixed herbs.', 590, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('59b2c2c0-c338-4121-942b-da2741454081', 'Smorgasbord Special Salad', 'Tuna mayo, smoked salmon, cheddar cheese, lettuce, apricots, red onions, tomatoes, carrots, olives, mixed nuts, sweetcorn topped with extra virgin olive oil and ceasar dressing.', 995, 'f', 'b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5'),
('5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 'Spicy beef sausages, hash browns, fried egg, homemade tomato sauce, mushrooms, baked beans with toast and butter. (40p Extra for scrambled egg).', 750, 'f', 'c32cbf15-8dd8-4df0-bb60-4944fc86d9d7'),
('5f3af28b-18d1-4f5e-8d7e-059fb426ae0f', 'Going South Grilled Sandwich', 'Slow roasted chicken, tomato, jalapeno peppers with a spicy homemade sauce. Add some caramelised onions for a sweet and savoury taste (50p) - just ask.', 575, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('5fd68f1a-b925-484c-b13d-c244072ee9a0', 'Breakfast Kiddies Corner', 'One meat or veg sausage, fried egg, beans, hash brown & toast', 550, 'f', 'fae067c0-6fc8-4e7b-8a3e-0e28dfebf2c2'),
('61e66e05-5305-4682-967a-2eb927f6d6b6', 'Veggie Breakfast', 'Vegetarian sausages, hash browns, fried egg, homemade tomato sauce, mushrooms, baked beans with toast and butter. (40p Extra for scrambled egg)', 750, 'f', 'eca982ef-2f85-4ef3-8d30-fd7aa5a487ad'),
('63f0a9b8-cd3c-4f91-b5e0-9b7f9d99d0fb', 'The Med Vegetarian Grilled Sandwich', 'Garlic infused butter ciabatta with fresh tomatoes and finished with melted cheese. Tastes absolutely fantastic! Add jalapenos (40p) for some extra spice', 525, 'f', '140d5edf-ee89-40c7-b95a-a7df88ea3038'),
('642647fa-40ca-4126-80c8-47ac7b46d881', 'Chicken Toasty', 'Chicken breast, cherry tomatoes, red onions, jalapenos, black pepper, extra virgin olive oil and cheese.', 550, 'f', 'd69155ce-a1b3-443c-9dca-14ed241ee97b'),
('648aba26-7306-4b5b-9c96-cb62cefaa5fe', 'Going South Grilled Sandwich', 'Slow roasted chicken, tomato, jalapeno peppers with a spicy homemade sauce. Add some caramelised onions for a sweet and savoury taste (50p) - just ask.', 575, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('64e5b2fa-cead-412b-b418-29de86227df6', 'Latte', 'A long coffee made with 2 shots of espresso, steamed milk & a topping of small froth.', 295, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('67a25d47-d210-4a80-937e-23f5f8fa3c63', 'Cheese Savoury Gourmet Cold Sandwich', 'Mild mature cheddar mixed with red onions, sweetcorn, lettuce and luxury mayonnaise', 550, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('6d2f5bb0-231e-41ae-8d4f-58e57bc3d8ca', 'Med Omelette', 'A deliciously light two egg omelette mixed with red onions, sweetcorn, mushrooms, cherry tomatoes, green olives, mixed herbs, topped with melted cheese, cooked to perfection under the grill. Served with toast & butter. Ask for jalapenos or red chilli flakes for added spice.', 795, 'f', 'c32cbf15-8dd8-4df0-bb60-4944fc86d9d7'),
('6d4e09af-918a-421a-a4b4-574f6986a1b3', 'Kale Kick - 500ml', 'Mango, spinach, kale', 450, 'f', 'da6e8911-0a0a-4c21-ab66-837e6ee5c18a'),
('741f66c5-651c-4e82-a8d9-880fcbe05da0', 'Frappuccino - 500ml', 'A cold creamy coffee. Blended espresso, ice & milk with your choice of syrup. Caramel Frappe is quite nice!', 350, 'f', 'a7519c79-5e5e-4b9f-ac49-94bbb1f97968'),
('75450602-0c55-4ae3-ba85-95ed6d3c2449', 'Frappuccino - 500ml', 'A cold creamy coffee. Blended espresso, ice & milk with your choice of syrup. Caramel Frappe is quite nice!', 350, 'f', '42b99c7c-5248-4364-b6ce-de9ba3585a96'),
('7627dedf-cdea-442d-b766-dc74fb3c762e', 'Breakfast Kiddies Corner', 'One meat or veg sausage, fried egg, beans, hash brown & toast', 550, 'f', 'a1540e1f-51ba-4c28-b7ff-dc619d17bf10'),
('78180e6e-3303-4cd3-a2a3-42e26a497a3b', 'Ambro’s Belly Buster Grilled Sandwich', 'Named after our valued customer, ambro who made this sandwich and it’s been a hit with all our customers. Spicy beef sausage meat, dijon mustard, caramelised onions, mushrooms, jalapenos, scrambled egg and melted cheese.', 785, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('79756d3a-448b-4013-8971-ec653b3bd72d', 'Detox Zing - 500ml', 'Blueberry, carrot, ginger, banana, courgette', 450, 'f', 'da6e8911-0a0a-4c21-ab66-837e6ee5c18a'),
('7999f32f-c3ed-421d-8a32-e80faba7e989', 'Caramel Ice - 500ml', '2 Shots of espresso mixed with ice, milk and caramel syrup.', 295, 'f', 'a7519c79-5e5e-4b9f-ac49-94bbb1f97968'),
('79c0f060-50d5-45a8-83c5-1b933dc1678e', 'Pesto Flatbread Pizza', 'Organic pesto, bell peppers, cherry tomatoes, green chillies, red onions, oregano, black pepper topped with 3 cheeses, feta, cheddar and mozzarella.', 695, 'f', '60d52634-6df2-4234-9e94-6bf2a1083ed0'),
('7b8886a1-85d6-4b5c-882e-3ea9a895f298', 'Winter Warmer', 'Coffee, pumpkin spice syrup topped with whipped cream & cinnamon sugar', 395, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('7c44282b-90a4-4eed-8d9d-9386de1e39c9', 'Caramel Machiatto', 'Vanilla syrup, topped with hot milk, freshly ground espresso and a drizzle of caramel.', 325, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('7d21ca11-00d8-4eba-a394-9e2654ace32c', 'Mochachino', 'Espresso mixed with chocolate and topped with steamed milk & froth.', 345, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('7db33ec3-e8b8-41f9-b001-9de981e6313c', 'Chicken Flatbread Pizza', 'Marinated chicken pieces, bell peppers, cherry tomatoes, sweetcorn, green chillies, red onions, topped with 3 cheeses, feta, cheddar and mozzarella.', 675, 'f', '60d52634-6df2-4234-9e94-6bf2a1083ed0'),
('84295bb9-5c22-421b-8dcd-06c9969e1af7', 'Caramel Machiatto', 'Vanilla syrup, topped with hot milk, freshly ground espresso and a drizzle of caramel.', 325, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('8701c375-c9e0-4111-871a-c91b0de43bad', 'Lamb Shish Grilled Sandwich', 'Mouth-watering lamb shish topped with caramelised onions, tomatoes, homemade spicy sauce and melted cheddar cheese. A personal recommendation from the chef!', 595, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('887d5ee4-8cf2-4ea3-9711-fcd13c0897e5', 'Nachos', 'Tortilla chips with jalapeno, sour cream & melted cheese.', 495, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('88c61a5c-c0c9-4997-8095-85e730075488', 'Piri Piri Chicken Grilled Sandwich', 'Chicken pieces marinated overnight and cooked to perfection. Includes fresh juicy cherry tomatoes, red onions, homemade piri sauce topped with melted cheese.', 575, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('9101d399-9ece-42cb-a149-e40c8b0795a9', 'Chicken & Sweetcorn Gourmet Cold Sandwich', 'Slow roast chicken breast, mixed with luxury mayonnaise, sweetcorn and lettuce with a sprinkle of fresh ground black pepper.', 575, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('91b1bc77-d73c-4d9d-8610-8ed21fcc8cde', 'Cheese Savoury Jacket Potato', 'Cheese mixed with red onions, sweetcorn, lettuce and creamy mayonnaise', 575, 'f', '9de1eb81-8cb3-4b5e-9f7c-c744c0eb4cc1'),
('9231c448-3e32-4462-8619-418f579b74a7', 'Full English Breakfast', 'Spicy beef sausages, hash browns, fried egg, homemade tomato sauce, mushrooms, baked beans with toast and butter. (40p Extra for scrambled egg).', 750, 'f', 'eca982ef-2f85-4ef3-8d30-fd7aa5a487ad'),
('9382aff8-b353-4bdf-9ccb-252a7ea84b89', 'Meat Flatbread Pizza', 'Grilled mincemeat, bell peppers, cherry tomatoes, green chillies, red onions, sprinkle of olive oil, topped with 3 cheeses, feta, cheddar and mozzarella.', 675, 'f', '60d52634-6df2-4234-9e94-6bf2a1083ed0'),
('93ab5238-f50d-4d6b-9aa2-282af68a95ad', 'Roast Beef (Cold) Gourmet Cold Sandwich', 'Strips of beef slow roasted with red onions, lettuce, tomatoes, creamy mayonnaise, jalapenos, freshly ground black pepper and dijon mustard. If you are not a fan of mustard, please ask to exclude it or replace with mayo', 595, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('95492d8b-8e17-43e1-bddf-e69b1c967714', 'Veggy Flatbread Pizza', 'Mushrooms, peppers, spring onions, rocket, cherry tomatoes, green chillies, olives, 3 cheeses, feta, mozzarella and cheddar.', 625, 'f', 'cabe6637-6e0a-4c95-83e3-e3e95d5d2e69'),
('97764c8d-8656-4dee-8dd2-5574dd071516', 'Veggy Flatbread Pizza', 'Mushrooms, peppers, spring onions, rocket, cherry tomatoes, green chillies, olives, 3 cheeses, feta, mozzarella and cheddar.', 625, 'f', '60d52634-6df2-4234-9e94-6bf2a1083ed0'),
('98da6757-fcd5-4bc2-b117-5f7ca552e606', 'Smoked Salmon Fresh Salad Bowl', 'Smoked salmon mixed with red onions, sweetcorn, lettuce, cucumber, green olives carrots and cherry tomatoes. Seasoned with black pepper and topped with extra virgin olive oil and caesar dressing.', 950, 'f', '4c8329a1-ef50-4a11-96e3-953694ed65ea'),
('9bbcb05b-f9a3-407f-b8ce-3d46fc43cddd', 'Caramel Ice - 500ml', '2 Shots of espresso mixed with ice, milk and caramel syrup.', 295, 'f', '42b99c7c-5248-4364-b6ce-de9ba3585a96'),
('9de7ded2-adf8-4e08-b958-161786e59a70', 'Salmon Classic Gourmet Cold Sandwich', 'Smoked salmon, cucumber, lettuce, black pepper, dash of extra virgin olive oil topped with creamy homemade dill and chive sauce.', 670, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('9e16c35d-ecac-4904-9aef-7ec1a6ed0e91', 'Caramel Peanut Butter', 'Contains: Mixed Nuts', 395, 'f', 'f2fa4032-56e8-4b60-ad48-fb195c032f92'),
('a6e2dca3-d795-4eb6-abaf-27f48fc2a5aa', 'Pesto Chicken Grilled Sandwich', 'Roast chicken slices, pesto, caramelised onions, cherry tomatoes, jalapenos, red onions and melted cheese.', 625, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('a85a609a-eb25-4b7a-a114-3bf98c10e64d', 'Cappuccino', 'An espresso topped with equal amounts of hot milk & froth topped with a sprinkle of chocolate.', 399, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('ae0b47a0-0fe2-41d0-9e39-6b52d77d0bf3', 'Bolognaise', 'Mildly spiced bolognaise sauce cooked to perfection over 4 hours.', 799, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('b0edd78b-0d87-482c-84a2-369bc84b0538', 'Spicy Feta', 'Feta cheese, extra virgin olive oil, fresh tomatoes, red onions, chopped jalapenos, salsa, oregano, cuts of bloomer and fresh salad.', 450, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('b1643fbf-40ba-4737-a646-570d460dc6ec', 'Chicken Arabiatta', 'Homemade arabiatta sauce cooked with chicken, tomatoes, red onions and a sprinkle of cheese. Spice it up with red chilli flakes or jalapenos. Just ask!', 850, 'f', 'bb69f5e7-634a-4d23-97a6-68aaf49ae447'),
('b438238d-57ee-42d0-a9db-1acfae7ae64b', 'Kale Kick - 500ml', 'Mango, spinach, kale', 450, 'f', '76a8b001-bc16-4179-bc0e-b1de9d931433'),
('b8c38872-d0b7-46af-8e41-42083ed32a3b', 'Smoked Salmon Fresh Salad Bowl', 'Smoked salmon mixed with red onions, sweetcorn, lettuce, cucumber, green olives carrots and cherry tomatoes. Seasoned with black pepper and topped with extra virgin olive oil and caesar dressing.', 950, 'f', 'b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5'),
('babc8359-5edd-4837-9478-0956389e39f8', 'Winter Warmer', 'Coffee, pumpkin spice syrup topped with whipped cream & cinnamon sugar', 395, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('bb2fb510-73a7-429f-a6d8-facb978d117b', 'Veg Samosa', '3 Veg samosa’s salad & mint sauce.', 275, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('bd481d65-f1fc-4b75-bb16-e289618030d5', 'Chilli Con Carne with Chips', 'Add melted cheese for £1', 695, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('bd75e7e1-5003-46fd-bf12-ea8fefe07e5d', 'Berry Go Round - 500ml', 'Raspberry, blackberry, strawberry', 450, 'f', 'da6e8911-0a0a-4c21-ab66-837e6ee5c18a'),
('c369e4df-c004-4f35-9c0e-b4507cc63487', 'Chilli Con Carne with Chips', 'Add melted cheese for £1', 695, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('c4462f91-ccae-4ad2-a7c1-28c1648d2fef', 'Greek Fresh Salad Bowl', 'Traditional Greek feta served on a bed of crisp lettuce, cucumber, cherry tomatoes, green olives, red onions mixed with extra virgin olive oil and Italian herbs.', 750, 'f', '4c8329a1-ef50-4a11-96e3-953694ed65ea'),
('c5a3da5a-ae23-4f86-87dd-0adb4f31a0a1', 'Mochachino', 'Espresso mixed with chocolate and topped with steamed milk & froth.', 345, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('c5e409bd-d54e-418e-9263-d65e0e048797', 'Chicken Goujons Kiddies Corner', 'Chicken Goujons, Chips and Beans or Salad', 550, 'f', 'a1540e1f-51ba-4c28-b7ff-dc619d17bf10'),
('ca0837a7-d5a3-49fb-a4f2-99508ffcdc2a', 'Espresso Single', 'An instant hit, guaranteed to wake you up!', 195, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('cc17b6f3-23fb-4756-914e-83cc6ae87604', 'Caramel Peanut Butter', 'Contains: Mixed Nuts', 395, 'f', 'f44713b0-e341-43a6-9b28-333be2016d81'),
('ccf1bc8d-14ec-4d11-8c6b-c6c6035821a2', 'Nachos', 'Tortilla chips with jalapeno, sour cream & melted cheese.', 495, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('cdd7502c-0d31-4e6e-8478-5cc70c0ddb85', 'Steak and Onion Grilled Sandwich', 'Beef steak and fried onions cooked in extra virgin olive oil topped with our homemade chilli and mustard mayo sauce. Served with steakhouse chips.', 795, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('cfa4543e-7dc7-4eba-b9b7-c25041c70784', 'Tuna Melt Grilled Sandwich', 'Tuna flakes, green olives, red onions, black pepper, mayo, dash of extra virgin olive oil and melted cheese. Ask for any other toppings , jalapeno, sweetcorn, tomatos (40p per topping)', 625, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('d1e4623c-43da-424b-836c-58763d4d5d6d', 'Seekh Kebabs', 'Two chunky seekh kebabs, salad & mint sauce.', 375, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('d4a82b77-2105-4392-bb73-bdf9c1498865', 'Seekh Kebabs', 'Two chunky seekh kebabs, salad & mint sauce.', 375, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('d6f6f604-b5f0-4dcf-9efd-df1728fc35a0', 'Houmous Dip', 'Served with pitted olives, homemade houmous topped with extra virgin olive oil, mixed herbs, cuts of bloomer & salad.', 495, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('d8c69189-0b1c-4892-8ab0-27b9f7047f38', 'Nut Cracker', 'Coffee, toffee nut syrup, chocolate, caramel, whipped cream & crushed nuts', 350, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('d94079d8-1c7e-46d2-b301-60759ee66b93', 'The Med Vegetarian Grilled Sandwich', 'Garlic infused butter ciabatta with fresh tomatoes and finished with melted cheese. Tastes absolutely fantastic! Add jalapenos (40p) for some extra spice', 525, 'f', 'f0eccf5e-5414-4fff-abbe-7cc016f55398'),
('da3befe3-6fc4-470a-8e4f-09c03f89e71a', 'Meat Samosa', '3 Meat samosa’s salad & mint sauce.', 375, 'f', '975b6d34-4aa5-41ef-87d5-efe6e550ce35'),
('daf7731e-f7fd-47d7-a5ed-ab9df5277e32', 'Nut Cracker', 'Coffee, toffee nut syrup, chocolate, caramel, whipped cream & crushed nuts', 350, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('dbcaa484-71a8-4037-b57e-7712c63f9887', 'Roast Beef Melt Grilled Sandwich', 'Slices of roast beef with dijon mustard, red onion, juicy ripe tomatoes, freshly ground black pepper and a dash of extra virgin olive oil. Add some spice with jalapenos (40p) just ask.', 595, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('dc552b5f-563e-46cf-9785-5007a01bac84', 'Cheese Salad Gourmet Cold Sandwich', 'Mature cheddar, cucumber, cherry tomatoes, lettuce, red onions and a dash of extra virgin olive oil.', 495, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('dd59eb04-bcfe-4ef8-bdf2-f8b3b843f469', 'Vanilla Latte', 'A long coffee made with 2 shots of espresso, steamed milk & a topping of small froth and vanilla syrup', 325, 'f', '9a6dd1c8-8a96-4114-9f04-d465405616dc'),
('ddeb77e6-b1a7-44e4-859a-046665040869', 'Roast Beef Melt Grilled Sandwich', 'Slices of roast beef with dijon mustard, red onion, juicy ripe tomatoes, freshly ground black pepper and a dash of extra virgin olive oil. Add some spice with jalapenos (40p) just ask.', 595, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('de236967-31f9-4583-b6b9-ebe00a274a34', 'Alpollo', 'Garlic infused chicken breast cooked in a scrumptious creamy sauce. You can add mushrooms and jalapenos for 80p extra.', 850, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('e0ac1a31-8be2-4e50-9181-b299fd0ef097', 'Piri Piri Chicken Grilled Sandwich', 'Chicken pieces marinated overnight and cooked to perfection. Includes fresh juicy cherry tomatoes, red onions, homemade piri sauce topped with melted cheese.', 575, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('e1fe4c58-b7ce-422f-aa92-706a0d87d0db', 'Med Omelette', 'A deliciously light two egg omelette mixed with red onions, sweetcorn, mushrooms, cherry tomatoes, green olives, mixed herbs, topped with melted cheese, cooked to perfection under the grill. Served with toast & butter. Ask for jalapenos or red chilli flakes for added spice.', 795, 'f', 'eca982ef-2f85-4ef3-8d30-fd7aa5a487ad'),
('e1fe65ce-6a60-4535-bba9-791fe164d08b', 'Chicken & Sweetcorn Gourmet Cold Sandwich', 'Slow roast chicken breast, mixed with luxury mayonnaise, sweetcorn and lettuce with a sprinkle of fresh ground black pepper.', 575, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('e45bbef2-d6e9-4c33-924b-4f2374a60c9d', 'Hazelnut Mocha', 'Espresso mixed with chocolate, hazelnut syrup and topped with steamed milk & fresh vanilla whipped cream.', 395, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('e4698ac1-8de4-489b-b8b8-ac4a4f038643', 'Tomato & Cheese Melt Vegetarian Grilled Sandwich', 'Our version of the classic melt! Filled with mature cheddar, mozzarella topped with caramelized onion and oregano. Why not add some jalapenos (40p) for a bit of spiciness!', 575, 'f', '140d5edf-ee89-40c7-b95a-a7df88ea3038'),
('e47783d4-bcbc-49df-a609-fa891907ec95', 'Steak and Onion Grilled Sandwich', 'Beef steak and fried onions cooked in extra virgin olive oil topped with our homemade chilli and mustard mayo sauce. Served with steakhouse chips.', 795, 'f', '6b254232-58c2-496c-b6ff-59d55abf1fec'),
('e4cc8ef4-3466-4159-b0cd-cbb327c8c5c7', 'Pesto Chilli Chicken', 'Chicken cooked in garlic infused extra virgin olive oil, pesto, cherry tomatoes, green chillies and fresh basil. Fancy melted cheese on top? Just ask', 799, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('e815d9d0-ab1e-4c14-bbc9-72adde1bbb84', 'Chicken Caesar Fresh Salad Bowl', 'Lettuce, cucumber, red onions, sweetcorn, green olives, cherry tomatoes, julien carrots topped with garlic infused chicken breast, dash of extra virgin olive oil and Caesar sauce.', 799, 'f', 'b8eea0ec-c494-4f4e-aa7c-93dc4dce8cc5'),
('eb6e0e22-b329-4550-a7f2-2d08fc7d8075', 'Hot Chocolate', 'Add homemade whipped cream for an additional 40p', 399, 'f', '3f1bec1b-a041-4a9b-8303-e84fc1d6cfc0'),
('ec6b68dd-d7ba-4690-b573-6fb76d76aec6', 'Spicy Feta Gourmet Cold Sandwich', 'Crumbly feta cheese mixed with extra virgin olive oil, red onions, green olives, cherry tomatoes, jalapenos, salsa and mixed herbs.', 590, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('ecf7d548-6b57-49a0-91f0-e14767ff07c2', 'Spicy Feta', 'Feta cheese, extra virgin olive oil, fresh tomatoes, red onions, chopped jalapenos, salsa, oregano, cuts of bloomer and fresh salad.', 450, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('f077c190-c0d8-4302-b65d-94d9e6e3d9fa', 'Tuna Crunch Gourmet Cold Sandwich', 'Tuna flakes mixed with red onions, sweetcorn, lettuce and luxury mayonnaise.', 595, 'f', '408467b8-0857-4a6e-a75e-30cb07586b76'),
('f0821a9b-d19a-4f22-85a1-c2ea1948cd58', 'Veggie Supreme Vegetarian Grilled Sandwich', 'Cherry tomatoes, olives, sweetcorn, red onions, topped with melted cheddar cheese. Add jalapenos (40p) for some extra spice.', 495, 'f', '140d5edf-ee89-40c7-b95a-a7df88ea3038'),
('f4703597-04c4-4614-a655-ee9b6fa9a296', 'Chicken Arabiatta', 'Homemade arabiatta sauce cooked with chicken, tomatoes, red onions and a sprinkle of cheese. Spice it up with red chilli flakes or jalapenos. Just ask!', 850, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('f5b7e629-e0a9-4972-bb76-8746c3b6acf7', 'Machiatto', 'An espresso with a dash of frothed milk.', 225, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('f8838119-9fd4-46c7-bb9a-e25c689984bd', 'Latte', 'A long coffee made with 2 shots of espresso, steamed milk & a topping of small froth.', 295, 'f', 'f7aa7c01-464e-4cb5-81a9-d74a16d5c1f1'),
('f9286e48-aa2d-426a-9353-fcd3fbf1fbfa', 'Cheese Salad Gourmet Cold Sandwich', 'Mature cheddar, cucumber, cherry tomatoes, lettuce, red onions and a dash of extra virgin olive oil.', 495, 'f', 'd3887c25-3024-49ec-8906-e3f00f805ecc'),
('f94df426-af55-4eab-92b6-a2b4a106403b', 'Meat Samosa', '3 Meat samosa’s salad & mint sauce.', 375, 'f', 'ea9778c1-b1a3-4d2b-8402-1e2e03eb6cfc'),
('f9b97eef-cc93-4a29-b6bd-5a3cb46a0ce2', 'Lamb Shish Grilled Sandwich', 'Mouth-watering lamb shish topped with caramelised onions, tomatoes, homemade spicy sauce and melted cheddar cheese. A personal recommendation from the chef!', 595, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('fa7b9cc0-2cb2-4a92-bf6d-ded7431989ca', 'Pesto Chicken Grilled Sandwich', 'Roast chicken slices, pesto, caramelised onions, cherry tomatoes, jalapenos, red onions and melted cheese.', 625, 'f', 'f23ef1bc-4522-40b9-b3b1-cadb0bf9da2c'),
('febc3ace-a7af-45ab-ab73-05bc46e0b698', 'Pesto Pasta', 'Penne pasta cooked in extra virgin olive oil, pesto and fresh basil topped with Parmesan. Why not ask for melted cheese on top?', 750, 'f', '2351ad34-5170-4ca6-b94b-007f580371f7'),
('ffccaf79-294e-4cc8-b750-8643cc0b480d', 'Cheese Savoury Jacket Potato', 'Cheese mixed with red onions, sweetcorn, lettuce and creamy mayonnaise', 575, 'f', '2381e7d9-b192-4169-b222-de6d5311a7b0');

INSERT INTO "public"."menus" ("id", "restaurant_id") VALUES
(1, '015caf13-8252-476b-9e7f-c43767998c01'),
(2, '015caf13-8252-476b-9e7f-c43767998c02');

INSERT INTO "public"."order_items" ("id", "menu_item_id", "name", "price", "quantity", "order_id") VALUES
(1, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27994'),
(2, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27995'),
(3, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27996'),
(4, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27997'),
(5, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27998'),
(6, '5a6124cc-6ec9-4337-9a70-fcbd0c084c18', 'Full English Breakfast', 750, 2, 'b0e2b662-b552-4f1d-b56a-d8f5e7e27999');

INSERT INTO "public"."orders" ("id", "user_id", "restaurant_id", "delivery_fee", "service_fee", "status", "mobile_number", "address_line1", "address_line2", "city", "postcode", "placed_at", "confirmed_at", "accepted_at", "delivered_at", "rejected_at", "cancelled_at", "payment_intent_id", "payment_intent_client_secret", "number") VALUES
('b0e2b662-b552-4f1d-b56a-d8f5e7e27994', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 1),
('b0e2b662-b552-4f1d-b56a-d8f5e7e27995', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 2),
('b0e2b662-b552-4f1d-b56a-d8f5e7e27996', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 3),
('b0e2b662-b552-4f1d-b56a-d8f5e7e27997', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 4),
('b0e2b662-b552-4f1d-b56a-d8f5e7e27998', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 5),
('b0e2b662-b552-4f1d-b56a-d8f5e7e27999', '979a79d6-7b7c-4c21-88c9-8f918be90d03', '015caf13-8252-476b-9e7f-c43767998c01', 249, 50, 'Delivered', '07234567890', '12 Maine Road', '', 'Shipley', 'BD18 1LT', '2021-03-28 10:31:21.740762', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', '2021-03-28 11:32:12.216066', NULL, NULL, 'pi_1IZvtuADp7wq6SbkhGVRlBR3', 'pi_1IZvtuADp7wq6SbkhGVRlBR3_secret_qwZg5EyFOiG78sIFudy3vkfz5', 6);

INSERT INTO "public"."restaurant_cuisines" ("restaurant_id", "cuisine_name") VALUES
('015caf13-8252-476b-9e7f-c43767998c01', 'Breakfast'),
('015caf13-8252-476b-9e7f-c43767998c01', 'Café'),
('015caf13-8252-476b-9e7f-c43767998c02', 'Spanish'),
('015caf13-8252-476b-9e7f-c43767998c02', 'Tapas');

INSERT INTO "public"."restaurants" ("id", "manager_id", "name", "description", "phone_number", "address_line1", "address_line2", "city", "postcode", "latitude", "longitude", "status", "monday_open", "monday_close", "tuesday_open", "tuesday_close", "wednesday_open", "wednesday_close", "thursday_open", "thursday_close", "friday_open", "friday_close", "saturday_open", "saturday_close", "sunday_open", "sunday_close", "minimum_delivery_spend", "delivery_fee", "max_delivery_distance_in_km", "estimated_delivery_time_in_minutes") VALUES
('015caf13-8252-476b-9e7f-c43767998c01', '979a79d6-7b7c-4c21-88c9-8f918be90d01', 'Smorgasbord Coffee Bar', NULL, '01234567890', '2/4 Rawson Place', NULL, 'Bradford', 'BD1 3QQ', 53.830975, -1.75002, 'Approved', '09:00:00', '23:00:00', '09:00:00', '23:00:00', '09:00:00', '23:00:00', '09:00:00', '23:00:00', '09:00:00', '23:00:00', '09:00:00', '23:00:00', '09:00:00', '23:00:00', 1000, 249, 5, 40),
('015caf13-8252-476b-9e7f-c43767998c02', '979a79d6-7b7c-4c21-88c9-8f918be90d02', 'Tapas Tree Restaurant', NULL, '01234567890', 'Wharf House Wharf Street', NULL, 'Shipley', 'BD17 7DW', 53.814545, -1.7230635, 'Approved', '07:45:00', '23:45:00', '07:45:00', '23:45:00', '07:45:00', '23:45:00', '07:45:00', '23:45:00', '07:45:00', '23:45:00', '07:45:00', '23:45:00', '07:45:00', '23:45:00', 1000, 249, 5, 45);

INSERT INTO "public"."users" ("id", "first_name", "last_name", "email", "password", "mobile_number", "address_line1", "address_line2", "city", "postcode", "role") VALUES
('979a79d6-7b7c-4c21-88c9-8f918be90d01', 'Mr', 'Manager', 'mr.manager@gmail.com', '$2a$11$V/MzgGUlIjqDEx4hnJdJ.OFqkDYsTYGeWyCaRaT76/I4fmihsyMb.', NULL, NULL, NULL, NULL, NULL, 'RestaurantManager'),
('979a79d6-7b7c-4c21-88c9-8f918be90d02', 'Bruno', 'Walker', 'bruno@gmail.com', '$2a$11$UJpHe.HVBGsBKdQhgELUOeIlRQuv72C01vPNhAOLtvU5ZpUg0HPxO', NULL, NULL, NULL, NULL, NULL, 'RestaurantManager'),
('979a79d6-7b7c-4c21-88c9-8f918be90d03', 'Joe', 'Bloggs', 'joe.bloggs@gmail.com', '$2a$11$Xd.CnplhUnPS4xtso35XBuoVqfHM11rf0CGyvqBWCFBgJ5fIV7uBS', '07234567890', '12 Maine Road', NULL, 'Shipley', 'BD18 1LT', 'Customer');

ALTER TABLE "public"."basket_items" ADD FOREIGN KEY ("basket_id") REFERENCES "public"."baskets"("id") ON DELETE CASCADE;
ALTER TABLE "public"."basket_items" ADD FOREIGN KEY ("menu_item_id") REFERENCES "public"."menu_items"("id") ON DELETE CASCADE;
ALTER TABLE "public"."baskets" ADD FOREIGN KEY ("restaurant_id") REFERENCES "public"."restaurants"("id") ON DELETE CASCADE;
ALTER TABLE "public"."baskets" ADD FOREIGN KEY ("user_id") REFERENCES "public"."users"("id") ON DELETE CASCADE;
ALTER TABLE "public"."billing_accounts" ADD FOREIGN KEY ("restaurant_id") REFERENCES "public"."restaurants"("id") ON DELETE CASCADE;
ALTER TABLE "public"."menu_categories" ADD FOREIGN KEY ("menu_id") REFERENCES "public"."menus"("id") ON DELETE CASCADE;
ALTER TABLE "public"."menu_items" ADD FOREIGN KEY ("menu_category_id") REFERENCES "public"."menu_categories"("id") ON DELETE CASCADE;
ALTER TABLE "public"."menus" ADD FOREIGN KEY ("restaurant_id") REFERENCES "public"."restaurants"("id") ON DELETE CASCADE;
ALTER TABLE "public"."order_items" ADD FOREIGN KEY ("menu_item_id") REFERENCES "public"."menu_items"("id") ON DELETE RESTRICT;
ALTER TABLE "public"."order_items" ADD FOREIGN KEY ("order_id") REFERENCES "public"."orders"("id") ON DELETE CASCADE;
ALTER TABLE "public"."orders" ADD FOREIGN KEY ("restaurant_id") REFERENCES "public"."restaurants"("id") ON DELETE RESTRICT;
ALTER TABLE "public"."orders" ADD FOREIGN KEY ("user_id") REFERENCES "public"."users"("id") ON DELETE RESTRICT;
ALTER TABLE "public"."restaurant_cuisines" ADD FOREIGN KEY ("restaurant_id") REFERENCES "public"."restaurants"("id") ON DELETE CASCADE;
ALTER TABLE "public"."restaurant_cuisines" ADD FOREIGN KEY ("cuisine_name") REFERENCES "public"."cuisines"("name") ON DELETE CASCADE;
ALTER TABLE "public"."restaurants" ADD FOREIGN KEY ("manager_id") REFERENCES "public"."users"("id") ON DELETE CASCADE;