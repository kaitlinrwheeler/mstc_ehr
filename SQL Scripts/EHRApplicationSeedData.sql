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
    ([firstName], [middleName], [lastName], [suffix], [DOB], [gender], [race], [ethnicity], [religion], [primaryPhysician], [legalGuardian1], [legalGuardian2],
    [genderAssignedAtBirth], [preferredLanguage], [preferredPronouns], [previousName])
VALUES
    ('Johnny', 'David', 'Rose', NULL, '1952-04-04', 'Male', 'Caucasian', 'Caucasian', 'Agnostic', 4, 'Moira Rose', 'David Rose', 'Male', 'English', '', ''),
    
    ('Moira', 'Louise', 'Rose', NULL, '1950-06-01', 'Female', 'Caucasian', 'Caucasian', 'None', 2, 'Johnny Rose', 'David Rose', 'Female', 'English', '', ''),
    
    ('David', 'John', 'Rose', NULL, '1983-11-24', 'Male', 'Caucasian', 'Caucasian', 'None', 1, 'Patrick Brewer', 'Moira Rose', 'Male', 'English', '', ''),
    
    ('Alexis', 'Claire', 'Rose', NULL, '1985-09-15', 'Female', 'Caucasian', 'Caucasian', 'None', 5, 'Johnny Rose', 'Ted Mullens', 'Female', 'English', '', ''),
    
    ('Stevie', 'Nicks', 'Budd', NULL, '1988-02-28', 'Female', 'Caucasian', 'Caucasian', 'Atheist', 3, 'David Rose', 'Patrick Brewer', 'Female', 'English', '', '');


-- Dummy data for PatientContact table
INSERT INTO [dbo].[PatientContact] 
    ([MHN], [address], [city], [state], [zipcode], [phone], [email], [ECFirstName], [ECLastName], [ECRelationship], [ECPhone])
VALUES
    (1, '123 Main St', 'Metropolis', 'NY', 12345, '5551234567', 'johnny.rose@email.com', 'Moira', 'Rose', 'Spouse', '5559876543'),
    
    (2, '123 Main St', 'Metropolis', 'NY', 12345, '5559876543', 'moira.rose@email.com', 'Johnny', 'Rose', 'Spouse', '5551234567'),
    
    (3, '789 Oak St', 'Rivertown', 'NY', 54321, '5553456789', 'david.rose@email.com', 'Patrick', 'Brewer', 'Partner', '5558765432'),
    
    (4, '101 Pine St', 'Harborville', 'NY', 34512, '5556543210', 'alexis.rose@email.com', 'Ted', 'Mullens', 'Friend', '5559876543'),
    
    (5, '202 Cedar Ave', 'Meadowville', 'NY', 45123, '5554321098', 'stevie.nicks@email.com', 'David', 'Rose', 'Friend', '5553456789');



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
-- Patient with MHN = 2 (First patient)
INSERT INTO [dbo].[PatientAllergies] ([MHN], [allergyId], [onSetDate])
VALUES
    (1, 1, '2023-01-15'), -- Penicillin
    (1, 2, '2023-01-15'); -- Peanuts

-- Patient with MHN = 6 (Last patient)
INSERT INTO [dbo].[PatientAllergies] ([MHN], [allergyId], [onSetDate])
VALUES
    (5, 4, '2023-06-10'); -- Latex



-- Dummy data for PatientProblems table // Commented out because this table still needs to be added
INSERT INTO [dbo].[PatientProblems] ([MHN], [Priority], [Description], [ICD_10], [Immediacy], [CreatedAt], [CreatedBy], [Active])
VALUES
-- Patient Johnny (MHN = 1)
(1, 'High', 'Cardiac Health', 'I25.10', 'Urgent', '2023-01-01T00:00:00', 1, 1),
(1, 'Medium', 'Weight Management', 'E66.9', 'Routine', '2023-02-15T00:00:00', 2, 1),
(1, 'Low', 'Vitamin D Deficiency', 'E55.9', 'Routine', '2023-04-10T00:00:00', 3, 1),

-- Patient Moira (MHN = 2)
(2, 'High', 'Diabetes Management', 'E11.9', 'Urgent', '2023-03-01T00:00:00', 1, 1),
(2, 'Medium', 'Migraine Treatment', 'G43.9', 'Urgent', '2023-05-20T00:00:00', 2, 1),
(2, 'Low', 'Allergy Management', 'T78.40', 'Routine', '2023-07-15T00:00:00', 3, 1),

-- Patient David (MHN = 3)
(3, 'High', 'Anxiety Treatment', 'F41.9', 'Urgent', '2023-02-10T00:00:00', 1, 1),
(3, 'Medium', 'Gastritis Management', 'K29.40', 'Urgent', '2023-04-05T00:00:00', 2, 1),
(3, 'Low', 'Sleep Disorder Treatment', 'G47.9', 'Routine', '2023-06-25T00:00:00', 3, 1),

-- Patient Alexis (MHN = 4)
(4, 'High', 'Allergy Management', 'T78.40', 'Urgent', '2023-03-15T00:00:00', 1, 1),
(4, 'Medium', 'Asthma Treatment', 'J45.909', 'Urgent', '2023-05-10T00:00:00', 2, 1),
(4, 'Low', 'Chronic Pain Management', 'G89.29', 'Routine', '2023-07-30T00:00:00', 3, 1),

-- Patient Stevie (MHN = 5)
(5, 'High', 'Physical Therapy', 'Z47.89', 'Urgent', '2023-04-01T00:00:00', 1, 1),
(5, 'Medium', 'Injury Follow-up', 'S49.90', 'Routine', '2023-06-18T00:00:00', 2, 1),
(5, 'Low', 'General Wellness', 'Z00.00', 'Routine', '2023-08-10T00:00:00', 3, 1);




-- Dummy data for PatientInsurance table
-- Patient Johnny (MHN = 1)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (1, 'ABC Insurance', 123456789, 98765, 54321, 'Primary', 4, 1); -- Provider: Dr. Yang (primaryPhysician = 4)

-- Patient Moira (MHN = 2)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (2, 'XYZ Health Plans', 987654321, 56789, 12345, 'Primary', 2, 1); -- Provider: Dr. Shepherd (primaryPhysician = 2)

-- Patient David (MHN = 3)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (3, 'HealthCare Inc.', 654321987, 87654, 43210, 'Primary', 1, 1); -- Provider: Dr. Grey (primaryPhysician = 1)

-- Patient Alexis (MHN = 4)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (4, 'Wellness Insurance', 135792468, 24680, 97531, 'Primary', 5, 1); -- Provider: Dr. Karev (primaryPhysician = 5)

-- Patient Stevie (MHN = 5)
INSERT INTO [dbo].[PatientInsurance] ([MHN], [providerName], [memberId], [policyNumber], [groupNumber], [priority], [primaryPhysician], [active])
VALUES
    (5, 'LifeGuard Health', 246813579, 80246, 13579, 'Primary', 3, 1); -- Provider: Dr. Bailey (primaryPhysician = 3)


-- Dummy data for PatientNotes table
-- Patient Johnny (MHN = 1)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (1, 'Follow-up appointment scheduled.', '2023-01-20', '2023-01-21 09:00:00', 4, 4, '2023-01-21 09:30:00', 'Appointment');

-- Patient Moira (MHN = 2)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (2, 'Prescription renewal requested.', '2023-02-05', '2023-02-06 10:15:00', 2, 2, '2023-02-06 10:30:00', 'Medication');

-- Patient David (MHN = 3)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (3, 'Reported increased anxiety levels.', '2023-03-12', '2023-03-13 11:45:00', 1, 1, '2023-03-13 12:00:00', 'Behavioral');

-- Patient Alexis (MHN = 4)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (4, 'Annual check-up scheduled.', '2023-04-25', '2023-04-26 14:30:00', 5, 5, '2023-04-26 15:00:00', 'Appointment');

-- Patient Stevie (MHN = 5)
INSERT INTO [dbo].[PatientNotes] ([MHN], [Note], [occurredOn], [createdAt], [createdBy], [associatedProvider], [updatedAt], [category])
VALUES
    (5, 'Physical therapy session completed.', '2023-05-15', '2023-05-16 13:00:00', 3, 3, '2023-05-16 13:30:00', 'Therapy');




-- Dummy data for Visits table
-- Patient Johnny (MHN = 1)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (1, 1, 1, '2023-01-10', '10:00:00', 1, 'Routine check-up'),
    (1, 2, 2, '2023-02-15', '14:30:00', 0, 'Discussion about medication'),
    (1, 3, 3, '2023-03-20', '11:15:00', 0, 'Follow-up after recent illness');

-- Patient Moira (MHN = 2)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (2, 2, 2, '2023-01-05', '09:45:00', 0, 'Prescription renewal'),
    (2, 1, 1, '2023-02-20', '13:30:00', 1, 'Admitted for minor surgery');

-- Patient David (MHN = 3)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (3, 3, 3, '2023-03-05', '10:30:00', 0, 'Discussion about anxiety levels'),
    (3, 1, 1, '2023-04-15', '15:00:00', 0, 'Follow-up on gastritis treatment');

-- Patient Alexis (MHN = 4)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (4, 4, 4, '2023-05-10', '11:30:00', 0, 'Annual check-up'),
    (4, 2, 2, '2023-06-20', '14:00:00', 0, 'Discussion about allergies');

-- Patient Stevie (MHN = 5)
INSERT INTO [dbo].[Visits] ([MHN], [providersId], [providerId], [date], [time], [admitted], [notes])
VALUES
    (5, 3, 3, '2023-07-15', '13:45:00', 0, 'Physical therapy session'),
    (5, 1, 1, '2023-08-25', '12:15:00', 0, 'Follow-up after injury');



-- Dummy data for Vitals table
INSERT INTO [dbo].[Vitals] ([visitId], [patientId], [painLevel], [temperature], [bloodPressure], [respiratoryRate], [pulseOximetry], [heightInches], [weightPounds], [BMI], [intakeMilliLiters], [outputMilliLiters])
VALUES 
(1, 1, 2, 98.6, 120, 18, 98.5, 68.5, 155, 23.3, 1500, 1200),
(2, 1, 1, 99.2, 118, 16, 99.0, 70.2, 160, 23.9, 1300, 1100),
(3, 1, 0, 98.9, 122, 20, 97.8, 68.9, 152, 22.8, 1600, 1400),
(4, 2, 1, 98.0, 115, 22, 98.7, 67.4, 145, 22.6, 1400, 1300),
(5, 2, 2, 99.5, 121, 19, 98.2, 69.8, 158, 24.3, 1200, 1000),
(6, 3, 0, 98.8, 117, 21, 99.3, 66.5, 138, 21.0, 1800, 1600),
(7, 3, 1, 99.1, 120, 18, 98.9, 68.3, 153, 23.0, 1700, 1500),
(8, 4, 2, 98.7, 116, 20, 97.5, 70.1, 162, 24.5, 1100, 900),
(9, 4, 0, 98.4, 114, 24, 98.0, 68.7, 149, 22.3, 1900, 1800),
(10, 5, 1, 99.3, 119, 19, 98.4, 67.9, 142, 21.5, 2000, 1700),
(11, 5, 0, 98.5, 118, 23, 97.7, 66.8, 140, 22.1, 1600, 1400);



-- Dummy data for MedicationProfile table
INSERT INTO [dbo].[MedicationProfile] ([medName], [description], [route])
VALUES 
('Aspirin', 'Antiplatelet agent', 'Oral'),
('Lisinopril', 'ACE inhibitor', 'Oral'),
('Atorvastatin', 'Statins', 'Oral'),
('Metformin', 'Antidiabetic agent', 'Oral'),
('Levothyroxine', 'Thyroid hormone replacement', 'Oral'),
('Hydrochlorothiazide', 'Diuretic', 'Oral'),
('Omeprazole', 'Proton pump inhibitor', 'Oral'),
('Metoprolol', 'Beta-blocker', 'Oral'),
('Losartan', 'ARB (Angiotensin II receptor blocker)', 'Oral'),
('Amlodipine', 'Calcium channel blocker', 'Oral'),
('Warfarin', 'Anticoagulant', 'Oral'),
('Albuterol', 'Bronchodilator', 'Inhalation'),
('Citalopram', 'SSRI (Selective serotonin reuptake inhibitor)', 'Oral'),
('Gabapentin', 'Anticonvulsant', 'Oral'),
('Ibuprofen', 'NSAID (Nonsteroidal anti-inflammatory drug)', 'Oral');



-- Dummy data for PatientMedication table
INSERT INTO [dbo].[PatientMedications] (
    [MHN], [medId], [category], [activeStatus], [prescriptionInstructions], [dosage], [route], [prescribedBy], [datePrescribed], [endDate]
) 
VALUES 
(1, 1, 'Pain Management', 'Active', 'Take one tablet daily with food', '81 mg', 'Oral', 2, '2023-01-10', '2023-02-10'),
(1, 3, 'Cholesterol Management', 'Active', 'Take one tablet at bedtime', '20 mg', 'Oral', 1, '2023-03-20', '2023-04-20'),
(2, 2, 'Hypertension', 'Active', 'Take one tablet daily in the morning', '10 mg', 'Oral', 3, '2023-02-15', '2023-03-15'),
(3, 11, 'Multivitamin', 'Active', 'Take one tablet daily with a meal', '1 tablet', 'Oral', 1, '2023-01-10', '2023-02-10'),
(3, 12, 'Allergy Relief', 'Active', 'Take one tablet as needed for allergies', '10 mg', 'Oral', 1, '2023-02-15', '2023-03-15'),
(4, 13, 'Vitamin D Supplement', 'Active', 'Take one tablet daily with a meal', '1000 IU', 'Oral', 2, '2023-03-20', '2023-04-20'),
(4, 14, 'Omega-3 Fatty Acids', 'Active', 'Take one capsule daily with a meal', '1000 mg', 'Oral', 2, '2023-01-05', '2023-02-05'),
(5, 15, 'Probiotic', 'Active', 'Take one capsule daily with a meal', '5 billion CFU', 'Oral', 3, '2023-02-20', '2023-03-20')



-- Dummy data for MedOrders table
INSERT INTO [dbo].[MedOrders] (
    [MHN], [visitId], [medId], [frequency], [fulfillmentStatus], [orderDate], [orderTime], [orderedBy]
)
VALUES
(1, 1, 1, 'Once daily', 'Pending', '2023-01-10', '10:30:00', 1),
(2, 2, 2, 'Twice daily', 'Pending', '2023-02-15', '14:45:00', 2),
(3, 3, 3, 'Once daily', 'Pending', '2023-03-20', '11:00:00', 3),
(4, 4, 4, 'Once daily', 'Pending', '2023-01-05', '10:00:00', 4),
(5, 5, 5, 'Once daily', 'Filled', '2023-02-20', '12:30:00', 1),
(1, 6, 6, 'Once daily', 'Filled', '2023-03-05', '09:45:00', 2),
(2, 7, 7, 'Twice daily', 'Filled', '2023-04-15', '15:15:00', 3);



-- Dummy data for LabTestProfile table
INSERT INTO [dbo].[LabTestProfile] (
    [testName], [description], [units], [referenceRange], [category]
)
VALUES
('Complete Blood Count (CBC)', 'Measures different components of the blood', 'cells/mcL', 'Male: 4.5 - 5.5, Female: 4.0 - 5.0', 'Hematology'),
('Basic Metabolic Panel (BMP)', 'Checks the status of your kidneys, electrolyte levels, and more', 'mg/dL', 'Varies by component', 'Chemistry'),
('Lipid Panel', 'Measures cholesterol levels', 'mg/dL', 'Total cholesterol: <200, LDL: <100, HDL: >60', 'Cardiac'),
('Liver Function Tests (LFTs)', 'Evaluates liver function and health', 'units/L', 'Varies by component', 'Hepatology'),
('Thyroid Stimulating Hormone (TSH)', 'Assesses thyroid function', 'mIU/L', '0.4 - 4.0', 'Endocrinology'),
('Hemoglobin A1c (HbA1c)', 'Monitors average blood sugar levels over time', '%', 'Normal: <5.7%, Prediabetes: 5.7 - 6.4%, Diabetes: ≥6.5%', 'Endocrinology'),
('Urinalysis', 'Examines physical and chemical properties of urine', 'Varies by component', 'Varies by component', 'Urology'),
('C-Reactive Protein (CRP)', 'Indicates inflammation in the body', 'mg/L', 'Low risk: <1.0, Average risk: 1.0 - 3.0, High risk: >3.0', 'Inflammatory'),
('Prothrombin Time (PT)', 'Evaluates blood clotting time', 'seconds', '10 - 13', 'Coagulation'),
('Activated Partial Thromboplastin Time (APTT)', 'Measures blood clotting time', 'seconds', '25 - 35', 'Coagulation'),
('Blood Type and Rh Factor', 'Determines blood type compatibility', 'Blood type and Rh factor', 'Varies by blood type', 'Immunology'),
('Hepatitis C Antibody Test', 'Detects antibodies to the hepatitis C virus', 'Index value', 'Non-reactive: <1.0, Reactive: ≥1.0', 'Infectious Diseases'),
('HIV Antibody Test', 'Screens for antibodies to the human immunodeficiency virus', 'Index value', 'Non-reactive: <1.0, Reactive: ≥1.0', 'Infectious Diseases'),
('Fasting Blood Glucose', 'Measures blood sugar levels after fasting', 'mg/dL', 'Normal: <100, Prediabetes: 100 - 125, Diabetes: ≥126', 'Endocrinology'),
('Creatinine Clearance', 'Evaluates kidney function', 'mL/min', 'Varies by age and gender', 'Nephrology');



-- Dummy data for LabOrders table
INSERT INTO [dbo].[LabOrders] (
    [MHN], [testId], [visitsId], [completionStatus], [orderDate], [orderTime], [orderedBy]
)
VALUES
(1, 1, 1, 'Pending', '2023-01-10', '10:30:00', 1),
(2, 2, 2, 'Completed', '2023-02-15', '15:45:00', 2),
(3, 3, 3, 'Pending', '2023-03-20', '12:00:00', 3),
(4, 4, 4, 'Completed', '2023-01-05', '11:15:00', 1),
(5, 5, 5, 'Pending', '2023-02-20', '13:00:00', 2);


-- Dummy data for LabResults table
INSERT INTO [dbo].[LabResults] (
    [MHN], [visitId], [visitsId], [testId], [resultValue], [abnormalFlag], [orderedBy], [date], [time]
)
VALUES
(2, 2, 2, 2, '120', 'Normal', 2, '2023-02-15', '15:50:00'),
(4, 4, 4, 4, '98', 'Normal', 1, '2023-01-05', '11:30:00'),
(1, 1, 1, 1, '65', 'High', 1, '2023-01-10', '11:00:00');



-- Dummy data for MedAdministrationHistory table
INSERT INTO [dbo].[MedAdministrationHistory] (
    [MHN], [category], [medId], [status], [frequency], [dateGiven], [timeGiven], [administeredBy]
)
VALUES
(2, 'Oral', 1, 'Administered', 'Once Daily', '2023-02-15', '15:50:00', 2),
(4, 'Injection', 3, 'Administered', 'Twice Daily', '2023-01-05', '11:30:00', 1),
(1, 'Oral', 2, 'Administered', 'Once Daily', '2023-01-10', '11:00:00', 1),
(3, 'Oral', 4, 'Administered', 'Once Daily', '2023-03-05', '12:30:00', 3),
(5, 'Oral', 5, 'Administered', 'Twice Daily', '2023-04-15', '16:20:00', 1);



-- Dummy data for Alerts table
INSERT INTO [dbo].[Alerts] (
    [MHN], [alertName], [activeStatus], [endDate], [startDate]
)
VALUES
(2, 'Delusions', 'Active', '2024-02-01T00:00:00.0000000', '2023-01-01T00:00:00.0000000'),
(2, 'Fall Risk', 'Inactive', '2023-01-15T00:00:00.0000000', '2023-01-01T00:00:00.0000000'),
(4, 'Flight Risk', 'Active', '2023-12-01T00:00:00.0000000', '2023-01-01T00:00:00.0000000');



-- Dummy data for CarePlan table
INSERT INTO [dbo].[CarePlan] (
    [MHN], [priority], [startDate], [endDate], [activeStatus], [title], [diagnosis]
)
VALUES
(1, 'High', '2023-01-01T00:00:00.0000000', '2023-06-30T00:00:00.0000000', 'Active', 'Cardiac Health', 'Hypertension'),
(2, 'Medium', '2023-03-15T00:00:00.0000000', '2023-09-15T00:00:00.0000000', 'Active', 'Diabetes Management', 'Type 2 Diabetes');
