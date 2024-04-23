
CREATE TABLE categories
(
  category_id          integer AUTO_INCREMENT NOT NULL,
  category_name        varchar NOT NULL,
  category_description text    NOT NULL,
  PRIMARY KEY (category_id)
);

CREATE TABLE users
(
  user_id      uuid    NOT NULL,
  full_name    varchar NOT NULL,
  gender       char    NOT NULL,
  phone_number varchar NOT NULL UNIQUE,
  email        varchar NOT NULL UNIQUE,
  password     varchar NOT NULL,
  age          integer NOT NULL,
  PRIMARY KEY (user_id)
);

CREATE TABLE products
(
  product_id     uuid DEFAULT gen_random_uuid() NOT NULL,
  title          varchar NOT NULL,
  price          float4  NOT NULL,
  total_quantity integer NOT NULL,
  description    text    NOT NULL,
  thumbnail      varchar NOT NULL,
  PRIMARY KEY (product_id)
);

CREATE TABLE product_categories
(
  id          uuid NOT NULL,
  product_id  uuid NOT NULL,
  category_id uuid NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (product_id) REFERENCES products (product_id),
  FOREIGN KEY (category_id) REFERENCES categories (category_id)
);

CREATE TABLE address
(
  address_id  uuid    NOT NULL,
  country     varchar NOT NULL,
  city        varchar NOT NULL,
  province    varchar NOT NULL,
  postal_code varchar NOT NULL,
  district    varchar NOT NULL,
  street      varchar NOT NULL,
  user_id     uuid    NOT NULL,
  PRIMARY KEY (address_id),
  FOREIGN KEY (user_id) REFERENCES users (user_id)
);

CREATE TABLE orders
(
  order_id  uuid  DEFAULT gen_random_uuid()  NOT NULL,
  user_id   uuid  NOT NULL,
  status     varchar NOT NULL,
  address_id uuid    NOT NULL,
  user_id    uuid    NOT NULL,
  PRIMARY KEY (order_id),
  FOREIGN KEY (user_id) REFERENCES users (user_id),
  FOREIGN KEY (address_id) REFERENCES address (address_id)
);

CREATE TABLE order_details
(
  order_detail_id  uuid  DEFAULT gen_random_uuid()  NOT NULL,
  quantity        integer NOT NULL,
  unit_price      float4  NOT NULL,
  product_id      uuid    NOT NULL,
  order_id        uuid    NOT NULL,
  PRIMARY KEY (order_detail_id),
  FOREIGN KEY (product_id) REFERENCES product (product_id),
  FOREIGN KEY (order_id) REFERENCES orders (order_id)
);

CREATE INDEX by_email
  ON users (email ASC);

CREATE INDEX by_number
  ON users (phone_number ASC);

CREATE INDEX by_id
  ON users (user_id ASC);


INSERT INTO categories(category_name, category_description)
  VALUES
  ('cows','Ayman');