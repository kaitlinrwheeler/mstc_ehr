/* Starter Data */
-- Values for MHN, providerId, medId, allergyId, and labTestId may need to be updated when populating an empty database.
-- This will be dependent upon where the auto-incremented values start.
-- Be mindful of this if errors occur while populating an empty database.

-- Providers
INSERT INTO dbo.Providers (firstName, lastName, specialty, active) VALUES
('Meredith', 'Grey', 'General Surgery', 1),
('Derek', 'Shepherd', 'Neurology', 1),
('Miranda', 'Bailey', 'General Surgery', 1),
('Mark', 'Sloan', 'Plastic Surgery', 1),
('Cristina', 'Yang', 'Cardiology', 1),
('Alex', 'Karev', 'Pediactrics', 1);

-- Patients
INSERT INTO dbo.PatientDemographic (firstName, middleName, lastName, suffix, preferredPronouns, DOB, gender, preferredLanguage, ethnicity, race, 
    religion, primaryPhysician, genderAssignedAtBirth, raceList, Active, HasAlerts) VALUES
('Johnny', 'David', 'Rose', NULL, 'He/Him', '1960-05-14', 'Male', 'English', 'Non-Hispanic', 'White', NULL, 5, 'Male', '', 1, 0),
('Moira', 'Rose', 'Rose', NULL, 'She/Her', '1963-07-22', 'Female', 'English', 'Unknown', 'Racially ambiguous', NULL, 2, 'Female', '', 1, 1),
('David', 'Patrick', 'Rose', NULL, 'He/Him', '1983-03-01', 'Male', 'English', 'Non-Hispanic', 'White', NULL, 3, 'Male', '', 1, 0),
('Alexis', 'Claire', 'Rose', NULL, 'She/Her', '1985-12-27', 'Female', 'English', 'Non-Hispanic', 'White', NULL, 4, 'Female', '', 1, 0),
('Stevie', 'Nicks', 'Budd', NULL, 'She/Her', '1984-08-15', 'Female', 'English', 'Non-Hispanic', 'White', NULL, 3, 'Male', '', 1, 0);

-- Medications
INSERT INTO dbo.MedicationProfile (medName, description, route, activeStatus) VALUES
('Atorvastatin', 'Lowers cholesterol', 'Oral', 1),
('Levothyroxine', 'Thyroid hormone replacement', 'Oral', 1),
('Lisinopril', 'Treats high blood pressure', 'Oral', 1),
('Albuterol', 'Treats asthma', 'Inhalation', 1),
('Metformin', 'Treats type 2 diabetes', 'Oral', 1),
('Amlodipine', 'Used for heart and blood pressure', 'Oral', 1),
('Simvastatin', 'Lowers cholesterol', 'Oral', 1),
('Losartan', 'Treats high blood pressure', 'Oral', 1),
('Acetaminophen', 'Pain reliever', 'Oral', 1),
('Gabapentin', 'Relieves nerve pain', 'Oral', 1),
('Hydrochlorothiazide', 'Diuretic', 'Oral', 1),
('Omeprazole', 'Reduces stomach acid', 'Oral', 1),
('Citalopram', 'Antidepressant', 'Oral', 1),
('Amoxicillin', 'Antibiotic', 'Oral', 1),
('Metoprolol', 'Treats high blood pressure', 'Oral', 1);

-- Allergies
INSERT INTO dbo.Allergies (allergyName, allergyType, activeStatus) VALUES
('Peanuts', 'Food', 1),
('Dust mites', 'Environmental', 1),
('Penicillin', 'Medication', 1),
('Shellfish', 'Food', 1),
('Pollen', 'Environmental', 1),
('Latex', 'Material', 1),
('Mold', 'Environmental', 1),
('Pet dander', 'Animal', 1),
('Soy', 'Food', 1),
('Tree nuts', 'Food', 1),
('Bee stings', 'Insect', 1),
('Wheat', 'Food', 1),
('Cats', 'Animal', 1),
('Dogs', 'Animal', 1),
('Ibuprofen', 'Medication', 1);

-- Lab Tests
INSERT INTO dbo.LabTestProfile (testName, description, units, referenceRange, category, Active) VALUES
('Complete Blood Count', 'Measures red and white blood cells', 'cells/uL', '4500-10000', 'Hematology', 1),
('Lipid Profile', 'Measures cholesterol levels', 'mg/dL', 'Desirable <200', 'Chemistry', 1),
('Blood Glucose', 'Measures blood sugar levels', 'mg/dL', '70-100', 'Chemistry', 1),
('Liver Function Tests', 'Measures liver enzymes', 'U/L', '7-56', 'Chemistry', 1),
('Thyroid Function Test', 'Measures thyroid hormone levels', 'ng/dL', '0.5-5.0', 'Endocrinology', 1),
('Electrolytes', 'Measures body salts', 'mEq/L', '135-145', 'Chemistry', 1),
('Hemoglobin A1c', 'Measures average glucose over 3 months', '%', '<5.7%', 'Hematology', 1),
('Urinalysis', 'Tests urine for disorders', 'N/A', 'Normal', 'Microbiology', 1),
('C-reactive protein', 'Measures inflammation', 'mg/L', '<5', 'Immunology', 1),
('Prostate-Specific Antigen', 'Screens for prostate cancer', 'ng/mL', '<4', 'Oncology', 1),
('HIV Antibody Test', 'Tests for HIV antibodies', 'Negative/Positive', 'Negative', 'Immunology', 1),
('Pregnancy Test', 'Detects hCG hormone', 'Negative/Positive', 'Negative', 'Endocrinology', 1),
('Strep Throat Test', 'Detects streptococcus bacteria', 'Negative/Positive', 'Negative', 'Microbiology', 1),
('Mononucleosis Test', 'Detects Epstein-Barr virus', 'Negative/Positive', 'Negative', 'Immunology', 1),
('COVID-19 Test', 'Detects SARS-CoV-2 virus', 'Negative/Positive', 'Negative', 'Virology', 1);

-- Visits (assuming each provider sees each patient at least once)
INSERT INTO dbo.Visits (MHN, providerId, date, time, admitted, notes) VALUES
(3, 1, '2024-05-01', '08:00:00', 1, 'Routine check-up'),
(4, 2, '2024-05-01', '09:00:00', 0, 'Consultation for symptoms'),
(5, 3, '2024-05-01', '10:00:00', 1, 'Annual physical examination'),
(6, 4, '2024-05-01', '11:00:00', 0, 'Follow-up on previous condition'),
(7, 5, '2024-05-01', '12:00:00', 1, 'Emergency visit due to allergic reaction');

-- Alerts (linking to MHN, assuming patient 4 has an alert)
INSERT INTO dbo.Alerts (MHN, alertName, activeStatus, startDate, endDate) VALUES
(4, 'Allergy to Penicillin', 1, '2024-05-01', '2024-05-31');

-- Allergy Profiles (assigning allergies to patients)
INSERT INTO dbo.PatientAllergies (MHN, allergyId, onSetDate, activeStatus) VALUES
(3, 1, '2024-01-01', 1),  -- Johnny allergic to Peanuts
(4, 3, '2024-01-02', 1),  -- Moira allergic to Penicillin
(5, 5, '2024-01-03', 1),  -- David allergic to Pollen
(6, 8, '2024-01-04', 1),  -- Alexis allergic to Pet dander
(7, 10, '2024-01-05', 1); -- Roland allergic to Tree nuts

-- Care Plans (each patient has a care plan)
INSERT INTO dbo.CarePlan (MHN, priority, startDate, endDate, title, diagnosis, visitsId, active) VALUES
(3, 'High', '2024-05-01', '2024-06-01', 'Heart Health Management', 'Hypertension', 3, 1),
(4, 'Medium', '2024-05-01', '2024-06-01', 'Allergy Management', 'Allergic to Penicillin', 4, 1),
(5, 'Low', '2024-05-01', '2024-06-01', 'Routine Health Check', 'General Check-up', 5, 1),
(6, 'Medium', '2024-05-01', '2024-06-01', 'Asthma Care', 'Asthma', 6, 1),
(7, 'High', '2024-05-01', '2024-06-01', 'Diabetes Care Plan', 'Type 2 Diabetes', 7, 1);

-- Medication Orders (simulating orders for each visit)
INSERT INTO dbo.MedOrders (MHN, visitId, medId, frequency, fulfillmentStatus, orderDate, orderTime, orderedBy) VALUES
(3, 3, 1, 'Once daily', 'Pending', '2024-05-01', '08:30:00', 1),
(4, 4, 3, 'Once daily', 'Completed', '2024-05-01', '09:30:00', 2),
(5, 5, 5, 'Twice daily', 'Completed', '2024-05-01', '10:30:00', 3),
(6, 6, 7, 'Every morning', 'Pending', '2024-05-01', '11:30:00', 4),
(7, 7, 9, 'As needed', 'Pending', '2024-05-01', '12:30:00', 5);

-- Lab Orders (assuming a basic lab test for each patient)
INSERT INTO dbo.LabOrders (MHN, testId, visitsId, completionStatus, orderDate, orderTime, orderedBy) VALUES
(3, 1, 3, 'Completed', '2024-05-01', '08:45:00', 1),
(4, 2, 4, 'Pending', '2024-05-01', '09:45:00', 2),
(5, 3, 5, 'Completed', '2024-05-01', '10:45:00', 3),
(6, 4, 6, 'Completed', '2024-05-01', '11:45:00', 4),
(7, 5, 7, 'Pending', '2024-05-01', '12:45:00', 5);

-- Lab Results (for each lab order)
INSERT INTO dbo.LabResults (MHN, visitsId, testId, resultValue, abnormalFlag, orderedBy, date, time) VALUES
(3, 3, 1, '5000', 'Normal', 1, '2024-05-01', '09:00:00'),
(4, 4, 2, '190', 'High', 2, '2024-05-01', '10:00:00'),
(5, 5, 3, '90', 'Normal', 3, '2024-05-01', '11:00:00'),
(6, 6, 4, '29', 'Normal', 4, '2024-05-01', '12:00:00'),
(7, 7, 5, '80', 'Normal', 5, '2024-05-01', '13:00:00');

-- Medication Administration History (simulating actual administration of meds)
INSERT INTO dbo.MedAdministrationHistory (MHN, category, medId, status, frequency, dateGiven, timeGiven, administeredBy, visitsId) VALUES
(3, 'Oral', 1, 'Administered', 'Once daily', '2024-05-01', '09:15:00', 1, 3),
(4, 'Oral', 3, 'Administered', 'Once daily', '2024-05-01', '10:15:00', 2, 4),
(5, 'Inhalation', 5, 'Administered', 'Twice daily', '2024-05-01', '11:15:00', 3, 5),
(6, 'Intramuscular', 7, 'Administered', 'Every morning', '2024-05-01', '12:15:00', 4, 6),
(7, 'Buccal', 9, 'Administered', 'As needed', '2024-05-01', '13:15:00', 5, 7);

-- Patient Notes (additional notes made by providers)
INSERT INTO dbo.PatientNotes (MHN, Note, occurredOn, createdAt, createdBy, associatedProvider, updatedAt, category, visitsId) VALUES
(3, 'Patient in good health, continue current medication', '2024-05-01', '2024-05-01', 1, 1, '2024-05-01', 'Follow up', 3),
(4, 'Advised on allergy management and alternatives to penicillin', '2024-05-01', '2024-05-01', 2, 2, '2024-05-01', 'Consultation', 4),
(5, 'Routine checkup complete, no issues found', '2024-05-01', '2024-05-01', 3, 3, '2024-05-01', 'Routine', 5),
(6, 'Follow-up needed in one month', '2024-05-01', '2024-05-01', 4, 4, '2024-05-01', 'Check up', 6),
(7, 'Emergency visit due to allergic reaction, situation under control', '2024-05-01', '2024-05-01', 5, 5, '2024-05-01', 'Emergency', 7);

-- Vitals (recording vitals at each visit)
INSERT INTO dbo.Vitals (visitId, patientId, painLevel, temperature, bloodPressure, respiratoryRate, pulseOximetry, heightInches, weightPounds, BMI, intakeMilliLiters, outputMilliLiters, pulse) VALUES
(3, 3, 0, 98.6, '120/80', 16, 98.0, 70, 180, 25.7, 1500, 1400, 72),
(4, 4, 2, 98.7, '130/85', 18, 99.0, 65, 130, 21.6, 1200, 1000, 75),
(5, 5, 0, 98.5, '115/75', 15, 97.5, 68, 160, 24.3, 1600, 1500, 70),
(6, 6, 1, 99.0, '110/70', 20, 96.0, 64, 120, 20.6, 1400, 1300, 65),
(7, 7, 4, 99.2, '140/90', 22, 95.0, 72, 200, 27.1, 1800, 1700, 78);

-- Patient Insurance
INSERT INTO dbo.PatientInsurance (MHN, providerName, memberId, policyNumber, groupNumber, priority, primaryPhysician, active) VALUES
(3, 'HealthPlus Ins', 'HP1234567', 'PN987654', 'G1234', 'Primary', 1, 1),
(4, 'CareFirst Ins', 'CF1234567', 'PN987655', 'G1235', 'Secondary', 2, 1),
(5, 'WellBeing Ins', 'WB1234567', 'PN987656', 'G1236', 'Primary', 3, 1),
(6, 'SecureHealth Ins', 'SH1234567', 'PN987657', 'G1237', 'Secondary', 4, 1),
(7, 'MediCare Ins', 'MC1234567', 'PN987658', 'G1238', 'Primary', 5, 1);

-- Patient Medications (continuing from medication profiles)
INSERT INTO dbo.PatientMedications (MHN, medId, category, activeStatus, prescriptionInstructions, dosage, route, prescribedBy, datePrescribed, endDate) VALUES
(3, 1, 'Cholesterol', 'Active', 'Take one pill every morning', '20mg', 'Oral', 1, '2024-04-01', '2025-04-01'),
(4, 3, 'Blood Pressure', 'Active', 'Take one pill every night', '5mg', 'Oral', 2, '2024-04-01', '2025-04-01'),
(5, 5, 'Diabetes', 'Active', 'Take two pills daily', '500mg', 'Oral', 3, '2024-04-01', '2025-04-01'),
(6, 7, 'Cholesterol', 'Active', 'Take one pill every morning', '40mg', 'Oral', 4, '2024-04-01', '2025-04-01'),
(7, 9, 'Pain Relief', 'Active', 'Take as needed for pain', '500mg', 'Oral', 5, '2024-04-01', '2025-04-01');

-- Patient Problems (common issues each patient might face)
INSERT INTO dbo.PatientProblems (MHN, priority, description, ICD_10, immediacy, createdAt, createdBy, active, visitsId) VALUES
(3, 'High', 'Hypertension', 'I10', 'Urgent', '2024-05-01', 1, 1, 3),
(4, 'Medium', 'Seasonal Allergies', 'J30.9', 'Routine', '2024-05-01', 2, 1, 4),
(5, 'Low', 'Anxiety', 'F41.9', 'Routine', '2024-05-01', 3, 1, 5),
(6, 'Medium', 'Migraines', 'G43.9', 'Routine', '2024-05-01', 4, 1, 6),
(7, 'High', 'Type 2 Diabetes', 'E11', 'Urgent', '2024-05-01', 5, 1, 7);

-- Patient Contact 
INSERT INTO [dbo].[PatientContact] 
(MHN, address, city, state, zipcode, phone, email, ECFirstName, ECLastName, ECRelationship, ECPhone)
VALUES 
(3, '123 Creek Lane', 'Elmdale', 'New Hampshire', 11122, '5105551234', 'johnny.rose@example.com', 'Moira', 'Rose', 'Spouse', '5105554321'),
(4, '123 Creek Lane', 'Elmdale', 'New Hampshire', 11122, '5105554321', 'moira.rose@example.com', 'Johnny', 'Rose', 'Spouse', '5105551234'),
(5, '125 Creek Lane', 'Elmdale', 'New Hampshire', 11123, '5105551235', 'david.rose@example.com', 'Patrick', 'Brewer', 'Spouse', '5105555679'),
(6, '127 Creek Lane', 'Elmdale', 'New Hampshire', 11124, '5105551236', 'alexis.rose@example.com', 'Ted', 'Mullens', 'Friend', '5105555680'),
(7, '129 Creek Lane', 'Elmdale', 'New Hampshire', 11125, '5105551237', 'stevie.budd@example.com', 'David', 'Rose', 'Friend', '5105551235');


