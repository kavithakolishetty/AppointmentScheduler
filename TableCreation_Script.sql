-- Create Database
CREATE DATABASE AppointmentDB;

-- Check if the database exists
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'AppointmentDB')
BEGIN
    -- Use the created database
    USE AppointmentDB;
   
    BEGIN
        -- Create Table    
	CREATE TABLE [dbo].[Appointments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentDateTime] [smalldatetime] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

        PRINT 'Table [Appointments] created successfully.';
    END
   END 
