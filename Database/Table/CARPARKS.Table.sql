IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CarParks')
BEGIN
    CREATE TABLE CarParks (
        car_park_no NVARCHAR(50) PRIMARY KEY,
        address NVARCHAR(MAX),
        x_coord NVARCHAR(50),
        y_coord NVARCHAR(50),
        car_park_type NVARCHAR(50),
        type_of_parking_system NVARCHAR(50),
        short_term_parking NVARCHAR(50),
        free_parking NVARCHAR(50),
        night_parking NVARCHAR(50),
        car_park_decks NVARCHAR(50),
        gantry_height NVARCHAR(50),
        car_park_basement NVARCHAR(50)
    )
END