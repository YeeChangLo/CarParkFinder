IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CarParkAvailability')
BEGIN
    CREATE TABLE CarParkAvailability (
		id INT IDENTITY(1,1) PRIMARY KEY,
		car_park_no NVARCHAR(50) NOT NULL,
		lot_type NVARCHAR(10) NOT NULL,
		total_lots INT NOT NULL,
		lots_available INT NOT NULL,
		update_at DATETIME NOT NULL DEFAULT GETUTCDATE(),
		CONSTRAINT UQ_CarPark_Lot UNIQUE (car_park_no, lot_type) -- Ensures one record per car park & lot type
	);

END
