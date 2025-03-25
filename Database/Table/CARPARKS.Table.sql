IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CarParks')
BEGIN
    CREATE TABLE CarParks (
        car_park_no NVARCHAR(50) PRIMARY KEY,
        address NVARCHAR(MAX) NULL,
        x_coord NVARCHAR(50) NULL,
        y_coord NVARCHAR(50) NULL,
        car_park_type NVARCHAR(50) NULL,
        type_of_parking_system NVARCHAR(50) NULL,
        short_term_parking NVARCHAR(50) NULL,
        free_parking NVARCHAR(50) NULL,
        night_parking NVARCHAR(50) NULL,
        car_park_decks NVARCHAR(50) NULL,
        gantry_height NVARCHAR(50) NULL,
        car_park_basement NVARCHAR(50) NULL
    )
END