-- Dummy data for Providers table
INSERT INTO [dbo].[Providers] 
    ([firstName], [lastName], [specialty])
VALUES
    ('Meredith', 'Grey', 'General'),
    
    ('Derek', 'Shepherd', 'Neurology'),
    
    ('Miranda', 'Bailey', 'General'),
    
    ('Cristina', 'Yang', 'Cardiology'),
    
    ('Alex', 'Karev', 'Pediatrics');



-- Dummy data for PatientDemographic table
INSERT INTO [dbo].[PatientDemographic] 
    ([firstName], [middleName], [lastName], [suffix], [DOB], [gender], [perferredLanguage], [ethnicity], [religion], [providerId], [legalGuardian1], [legalGuardian2])
VALUES
    ('Johnny', 'David', 'Rose', NULL, '1952-04-04', 'Male', 'English', 'Caucasian', 'Agnostic', 4, 'Moira Rose', 'David Rose'),
    
    ('Moira', 'Louise', 'Rose', NULL, '1950-06-01', 'Female', 'English', 'Caucasian', 'None', 2, 'Johnny Rose', 'David Rose'),
    
    ('David', 'John', 'Rose', NULL, '1983-11-24', 'Male', 'English', 'Caucasian', 'None', 1, 'Patrick Brewer', 'Moira Rose'),
    
    ('Alexis', 'Claire', 'Rose', NULL, '1985-09-15', 'Female', 'English', 'Caucasian', 'None', 5, 'Johnny Rose', 'Ted Mullens'),
    
    ('Stevie', 'Nicks', 'Budd', NULL, '1988-02-28', 'Female', 'English', 'Caucasian', 'Atheist', 3, 'David Rose', 'Patrick Brewer');



-- Dummy data for PatientContact table
	INSERT INTO [dbo].[PatientContact] 
    ([MHN], [address], [city], [state], [zipcode], [phone], [email], [ECFirstName], [ECLastName], [ECRelationship], [ECPhone])
VALUES
    (3, '123 Main St', 'Metropolis', 'NY', 12345, 5551234567, 'johnny.rose@email.com', 'Moira', 'Rose', 'Spouse', 5559876543),
    
    (4, '123 Main St', 'Metropolis', 'NY', 12345, 5559876543, 'moira.rose@email.com', 'Johnny', 'Rose', 'Spouse', 5551234567),
    
    (5, '789 Oak St', 'Rivertown', 'NY', 54321, 5553456789, 'david.rose@email.com', 'Patrick', 'Brewer', 'Partner', 5558765432),
    
    (6, '101 Pine St', 'Harborville', 'NY', 34512, 5556543210, 'alexis.rose@email.com', 'Ted', 'Mullens', 'Friend', 5559876543),
    
    (7, '202 Cedar Ave', 'Meadowville', 'NY', 45123, 5554321098, 'stevie.nicks@email.com', 'David', 'Rose', 'Friend', 5553456789);



-- Dummy data for Allergies table
INSERT INTO [dbo].[Allergies] ([allergyName], [allergyType])
VALUES
    ('Penicillin', 'Drug Allergy'),
    ('Peanuts', 'Food Allergy'),
    ('Bee Venom', 'Insect Sting Allergy'),
    ('Latex', 'Contact Allergy'),
    ('Dust Mites', 'Environmental Allergy'),
    ('Mold', 'Environmental Allergy'),
    ('Shellfish', 'Food Allergy');



-- Dummy data for PatientAllergies table
-- Patient with MHN = 3 (First patient)
INSERT INTO [dbo].[PatientAllergies] ([MHN], [allergyId], [onSetDate])
VALUES
    (3, 1, '2023-01-15'), -- Penicillin
    (3, 2, '2023-01-15'); -- Peanuts

-- Patient with MHN = 7 (Last patient)
INSERT INTO [dbo].[PatientAllergies] ([MHN], [allergyId], [onSetDate])
VALUES
    (7, 4, '2023-06-10'); -- Latex



-- Dummy data for PatientProblems table
-- Patient Johnny (MHN = 3)
INSERT INTO [dbo].[PatientDx] ([MHN], [Dx], [createdAt], [createdBy], [active])
VALUES
    (3, 'I10', '2023-01-15 08:30:00', 4, 1); -- Essential (primary) hypertension (I10)

-- Patient Moira (MHN = 4)
INSERT INTO [dbo].[PatientDx] ([MHN], [Dx], [createdAt], [createdBy], [active])
VALUES
    (4, 'F32.9', '2023-02-01 09:45:00', 2, 1), -- Major depressive disorder, single episode, unspecified (F32.9)
    (4, 'Z63.0', '2023-02-05 11:15:00', 2, 1); -- Problems in relationship with spouse or partner (Z63.0)

-- Patient David (MHN = 5)
INSERT INTO [dbo].[PatientDx] ([MHN], [Dx], [createdAt], [createdBy], [active])
VALUES
    (5, 'K29.70', '2023-03-10 10:00:00', 1, 1), -- Alcoholic gastritis without bleeding (K29.70)
    (5, 'F41.9', '2023-03-15 13:20:00', 1, 1); -- Anxiety disorder, unspecified (F41.9)



-- Dummy data for PatientInsurance table
-- Patient Johnny (MHN = 3)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (3, 'ABC Insurance', 123456789, 98765, 54321, 'Primary', 4, 1); -- Provider: Dr. Yang (primaryPhysician = 4)

-- Patient Moira (MHN = 4)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (4, 'XYZ Health Plans', 987654321, 56789, 12345, 'Primary', 2, 1); -- Provider: Dr. Shepherd (primaryPhysician = 2)

-- Patient David (MHN = 5)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (5, 'HealthCare Inc.', 654321987, 87654, 43210, 'Primary', 1, 1); -- Provider: Dr. Grey (primaryPhysician = 1)

-- Patient Alexis (MHN = 6)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (6, 'Wellness Insurance', 135792468, 24680, 97531, 'Primary', 5, 1); -- Provider: Dr. Karev (primaryPhysician = 5)

-- Patient Stevie (MHN = 7)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (7, 'LifeGuard Health', 246813579, 80246, 13579, 'Primary', 3, 1); -- Provider: Dr. Bailey (primaryPhysician = 3)



-- Dummy data for PatientNotes table
-- Patient Johnny (MHN = 3)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (3, 'Follow-up appointment scheduled.', '2023-01-20', '2023-01-21 09:00:00', 4, 4, '2023-01-21 09:30:00', 'Appointment');

-- Patient Moira (MHN = 4)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (4, 'Prescription renewal requested.', '2023-02-05', '2023-02-06 10:15:00', 2, 2, '2023-02-06 10:30:00', 'Medication');

-- Patient David (MHN = 5)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (5, 'Reported increased anxiety levels.', '2023-03-12', '2023-03-13 11:45:00', 1, 1, '2023-03-13 12:00:00', 'Behavioral');

-- Patient Alexis (MHN = 6)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (6, 'Annual check-up scheduled.', '2023-04-25', '2023-04-26 14:30:00', 5, 5, '2023-04-26 15:00:00', 'Appointment');

-- Patient Stevie (MHN = 7)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (7, 'Physical therapy session completed.', '2023-05-15', '2023-05-16 13:00:00', 3, 3, '2023-05-16 13:30:00', 'Therapy');



-- Dummy data for Visits table
-- Patient Johnny (MHN = 3)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (3, 1, 1, '2023-01-10', '10:00:00', 1, 'Routine check-up'),
    (3, 2, 2, '2023-02-15', '14:30:00', 0, 'Discussion about medication'),
    (3, 3, 3, '2023-03-20', '11:15:00', 0, 'Follow-up after recent illness');

-- Patient Moira (MHN = 4)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (4, 2, 2, '2023-01-05', '09:45:00', 0, 'Prescription renewal'),
    (4, 1, 1, '2023-02-20', '13:30:00', 1, 'Admitted for minor surgery');

-- Patient David (MHN = 5)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (5, 3, 3, '2023-03-05', '10:30:00', 0, 'Discussion about anxiety levels'),
    (5, 1, 1, '2023-04-15', '15:00:00', 0, 'Follow-up on gastritis treatment');

-- Patient Alexis (MHN = 6)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (6, 4, 4, '2023-05-10', '11:30:00', 0, 'Annual check-up'),
    (6, 2, 2, '2023-06-20', '14:00:00', 0, 'Discussion about allergies');

-- Patient Stevie (MHN = 7)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (7, 3, 3, '2023-07-15', '13:45:00', 0, 'Physical therapy session'),
    (7, 1, 1, '2023-08-25', '12:15:00', 0, 'Follow-up after injury');



-- Dummy data for Vitals table
