-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-04-20 04:46:38.52

CREATE SCHEMA trip;
GO

-- tables
-- Table: Client
CREATE TABLE trip.Client (
                             IdClient int  NOT NULL,
                             FirstName nvarchar(120)  NOT NULL,
                             LastName nvarchar(120)  NOT NULL,
                             Email nvarchar(120)  NOT NULL,
                             Telephone nvarchar(120)  NOT NULL,
                             Pesel nvarchar(120)  NOT NULL,
                             CONSTRAINT Client_pk PRIMARY KEY  (IdClient)
);

-- Table: Client_Trip
CREATE TABLE trip.Client_Trip (
                                  IdClient int  NOT NULL,
                                  IdTrip int  NOT NULL,
                                  RegisteredAt datetime  NOT NULL,
                                  PaymentDate datetime  NULL,
                                  CONSTRAINT Client_Trip_pk PRIMARY KEY  (IdClient,IdTrip)
);

-- Table: Country
CREATE TABLE trip.Country (
                              IdCountry int  NOT NULL,
                              Name nvarchar(120)  NOT NULL,
                              CONSTRAINT Country_pk PRIMARY KEY  (IdCountry)
);

-- Table: Country_Trip
CREATE TABLE trip.Country_Trip (
                                   IdCountry int  NOT NULL,
                                   IdTrip int  NOT NULL,
                                   CONSTRAINT Country_Trip_pk PRIMARY KEY  (IdCountry,IdTrip)
);

-- Table: Trip
CREATE TABLE trip.Trip (
                           IdTrip int  NOT NULL,
                           Name nvarchar(120)  NOT NULL,
                           Description nvarchar(220)  NOT NULL,
                           DateFrom datetime  NOT NULL,
                           DateTo datetime  NOT NULL,
                           MaxPeople int  NOT NULL,
                           CONSTRAINT Trip_pk PRIMARY KEY  (IdTrip)
);

-- foreign keys
-- Reference: Country_Trip_Country (table: Country_Trip)
ALTER TABLE trip.Country_Trip ADD CONSTRAINT Country_Trip_Country
    FOREIGN KEY (IdCountry)
        REFERENCES trip.Country (IdCountry);

-- Reference: Country_Trip_Trip (table: Country_Trip)
ALTER TABLE trip.Country_Trip ADD CONSTRAINT Country_Trip_Trip
    FOREIGN KEY (IdTrip)
        REFERENCES trip.Trip (IdTrip);

-- Reference: Table_5_Client (table: Client_Trip)
ALTER TABLE trip.Client_Trip ADD CONSTRAINT Table_5_Client
    FOREIGN KEY (IdClient)
        REFERENCES trip.Client (IdClient);

-- Reference: Table_5_Trip (table: Client_Trip)
ALTER TABLE trip.Client_Trip ADD CONSTRAINT Table_5_Trip
    FOREIGN KEY (IdTrip)
        REFERENCES trip.Trip (IdTrip);

INSERT INTO Trip (IdTrip, Name, Description, DateFrom, DateTo, MaxPeople) VALUES (1, 'Trip 1', 'Description 1', '2024-06-01', '2024-06-10', 50);
INSERT INTO Trip (IdTrip, Name, Description, DateFrom, DateTo, MaxPeople) VALUES (2, 'Trip 2', 'Description 2', '2024-07-01', '2024-07-10', 30);

INSERT INTO Client (IdClient, FirstName, LastName, Email, Telephone, Pesel) VALUES (1, 'John', 'Doe', 'john.doe@example.com', '123456789', '12345678901');
INSERT INTO Client (IdClient, FirstName, LastName, Email, Telephone, Pesel) VALUES (2, 'Jane', 'Smith', 'jane.smith@example.com', '987654321', '09876543210');

INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) VALUES (1, 1, '2024-05-20', '2024-05-20');
INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) VALUES (2, 2, '2024-05-21', '2024-05-21');

-- End of file.
