
CREATE TABLE Categories
(
  Category_ID          uuid  DEFAULT gen_random_uuid()  NOT NULL,
  Category_Name        varchar NOT NULL,
  Category_Description text    NOT NULL,
  PRIMARY KEY (Category_ID)
);

CREATE TABLE Customers
(
  Customer_ID  uuid    NOT NULL,
  Full_Name    varchar NOT NULL,
  Gender       char    NOT NULL,
  Phone_Number varchar NOT NULL UNIQUE,
  Email        varchar NOT NULL UNIQUE,
  Password     varchar NOT NULL,
  Age          integer NOT NULL,
  Address      varchar NOT NULL,
  PRIMARY KEY (Customer_ID)
);

CREATE TABLE Products
(
  Product_ID     uuid    NOT NULL,
  Title          varchar NOT NULL,
  Price          float4  NOT NULL,
  Total_Quantity integer NOT NULL,
  Description    text    NOT NULL,
  Thumbnail      varchar NOT NULL,
  PRIMARY KEY (Product_ID)
);

CREATE TABLE Product_Categories
(
  ID          uuid NOT NULL,
  Product_ID  uuid NOT NULL,
  Category_ID uuid NOT NULL,
  PRIMARY KEY (ID),
  FOREIGN KEY (Product_ID) REFERENCES Products (Product_ID),
  FOREIGN KEY (Category_ID) REFERENCES Categories (Category_ID)
);

CREATE TABLE Orders
(
  Order_ID      uuid    NOT NULL,
  Customer_ID   uuid    NOT NULL,
  Order_Address varchar NOT NULL,
  Order_Date    date    NOT NULL,
  Status        varchar NOT NULL,
  PRIMARY KEY (Order_ID),
  FOREIGN KEY (Customer_ID) REFERENCES Customers (Customer_ID)
);

CREATE TABLE Order_Details
(
  Order_Detail_ID uuid    NOT NULL,
  Quantity        integer NOT NULL,
  Unit_Price      integer NOT NULL,
  Product_ID      uuid    NOT NULL,
  Order_ID        uuid    NOT NULL,
  PRIMARY KEY (Order_Detail_ID),
  FOREIGN KEY (Product_ID) REFERENCES Products (Product_ID),
  FOREIGN KEY (Order_ID) REFERENCES Orders (Order_ID)
);

CREATE INDEX By_Email
  ON Customers (Email ASC);

CREATE INDEX By_Number
  ON Customers (Phone_Number ASC);

CREATE INDEX By_ID
  ON Customers (Customer_ID ASC);


INSERT INTO Categories(Category_Name,Category_Description)
  VALUES
  ('Bags','bags are good');

