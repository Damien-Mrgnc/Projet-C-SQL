DROP DATABASE IF EXISTS SalleDeSport;
CREATE DATABASE IF NOT EXISTS SalleDeSport;
USE SalleDeSport;
CREATE TABLE IF NOT EXISTS Membres (
    MembreID INT PRIMARY KEY,
    Nom VARCHAR(100),
    Prenom VARCHAR(100),
    Adresse VARCHAR(255),
    Telephone VARCHAR(15),
    Pseudo VARCHAR(100),
    Email VARCHAR(100),
    DateInscription DATE,
    TypeAdhesion VARCHAR(50),
    Mots_de_passe VARCHAR(50),
    Niveau INT 
);
CREATE TABLE IF NOT EXISTS Adhesions (
    AdhesionID INT PRIMARY KEY ,
    MembreID INT,
    DateDebut DATE,
    DateFin DATE,  
    Statut BOOL,
    FOREIGN KEY (MembreID) REFERENCES Membres(MembreID)
);
CREATE TABLE IF NOT EXISTS Coachs (
    CoachID INT PRIMARY KEY,
    Nom VARCHAR(100),
    Prenom VARCHAR(100),
    Telephone VARCHAR(15)
);
CREATE TABLE IF NOT EXISTS Cours (
    CoursID INT PRIMARY KEY ,
    Horaire VARCHAR(15),
    CoachID INT,
    CapaciteMax INT,
    FOREIGN KEY (CoachID) REFERENCES Coachs(CoachID)
);
CREATE TABLE IF NOT EXISTS Reservations (
    ReservationID INT PRIMARY KEY,
    MembreID INT,
    CoursID INT,
    DateReservation DATE,
    Statut BOOL,
    FOREIGN KEY (MembreID) REFERENCES Membres(MembreID),
    FOREIGN KEY (CoursID) REFERENCES Cours(CoursID)
);
CREATE TABLE IF NOT EXISTS HistoriqueCours (
    HistoriqueID INT PRIMARY KEY ,
    MembreID INT,
    CoursID INT,
    DateCours DATE,
    FOREIGN KEY (MembreID) REFERENCES Membres(MembreID),
    FOREIGN KEY (CoursID) REFERENCES Cours(CoursID)
);

INSERT INTO Membres (MembreID, Nom, Prenom, Adresse, Telephone, Pseudo, Email, DateInscription, TypeAdhesion, Mots_de_passe, Niveau)
VALUES 
(1, 'Phelps', 'Michael', '1001 Swimming Ave', '0000012345', 'mphelps', 'michael.phelps@email.com', '2023-04-01', 'Annuelle', 'pass1234', 1),
(2, 'Bolt', 'Usain', '1002 Sprint St', '0000012346', 'ubolt', 'usain.bolt@email.com', '2023-04-01', 'Mensuelle', 'pass2345', 1),
(3, 'Williams', 'Serena', '1003 Tennis Rd', '0000012347', 'swilliams', 'serena.williams@email.com', '2023-04-01', 'Mensuelle', 'pass3456', 1),
(4, 'Jordan', 'Michael', '1004 Basketball Blvd', '0000012348', 'mjordan', 'michael.jordan@email.com', '2023-04-01', 'Annuelle', 'pass4567', 1),
(5, 'Woods', 'Tiger', '1005 Golf Ln', '0000012349', 'twoods', 'tiger.woods@email.com', '2023-04-01', 'Mensuelle', 'pass5678', 1),
(6, 'Morganico', 'Damien', '1006 Fitness Dr', '0000012350', 'dmorganico', 'damien.morganico@email.com', '2023-04-01', 'Annuelle', 'host', 3),
(7, 'Dupin', 'Matteo', '1007 Cycling Path', '0000012351', 'mdupin', 'matteo.dupin@email.com', '2023-04-01', 'Annuelle', 'host', 3),
(8, 'Soha', 'Lagneb', '1008 Running Way', '0000012352', 'slagneb', 'soha.lagneb@email.com', '2023-04-01', 'Annuelle', 'underhost', 2),
(9, 'Nadal', 'Rafael', '1009 Clay Ct', '0000012353', 'rnadal', 'rafael.nadal@email.com', '2023-04-01', 'Mensuelle', 'pass9012', 1),
(10, 'Hamilton', 'Lewis', '1010 Racing Rd', '0000012354', 'lhamilton', 'lewis.hamilton@email.com', '2023-04-01', 'Mensuelle', 'pass0123', 1);

INSERT INTO Coachs (CoachID, Nom, Prenom, Telephone)
VALUES 
(1, 'Mourinho', 'Jose', '0000023456'),
(2, 'Guardiola', 'Pep', '0000023457'),
(3, 'Ferguson', 'Alex', '0000023458'),
(4, 'Wenger', 'Arsene', '0000023459'),
(5, 'Jackson', 'Phil', '0000023460'),
(6, 'Klopp', 'Jurgen', '0000023461'),
(7, 'Belichick', 'Bill', '0000023462'),
(8, 'Popovich', 'Gregg', '0000023463'),
(9, 'Kerr', 'Steve', '0000023464'),
(10, 'Auriemma', 'Geno', '0000023465');

INSERT INTO Cours (CoursID, Horaire, CoachID, CapaciteMax)
VALUES 
(1, '08:00', 1, 20),
(2, '10:00', 2, 15),
(3, '12:00', 3, 10),
(4, '14:00', 4, 25),
(5, '16:00', 5, 30),
(6, '18:00', 2, 20),
(7, '20:00', 7, 15),
(8, '08:00', 5, 10),
(9, '10:00', 9, 25),
(10, '12:00', 2, 30);

INSERT INTO Reservations (ReservationID, MembreID, CoursID, DateReservation, Statut)
VALUES 
(1, 1, 1, '2023-04-02', TRUE),
(2, 2, 2, '2023-04-14', TRUE),
(3, 3, 3, '2023-04-27', false),
(4, 4, 4, '2023-05-05', false),
(5, 5, 5, '2023-05-15', false),
(6, 6, 6, '2023-05-25', TRUE),
(7, 7, 7, '2023-06-01', TRUE),
(8, 8, 8, '2023-06-12', false),
(9, 9, 9, '2023-06-22', TRUE),
(10, 10, 10, '2023-07-03', false);

INSERT INTO HistoriqueCours (HistoriqueID, MembreID, CoursID, DateCours)
VALUES 
(1, 1, 1, '2023-04-03'),
(2, 2, 2, '2023-04-15'),
(3, 3, 3, '2023-04-29'),
(4, 4, 4, '2023-05-06'),
(5, 5, 5, '2023-05-18'),
(6, 6, 6, '2023-05-29'),
(7, 7, 7, '2023-06-02'),
(8, 8, 8, '2023-06-14'),
(9, 9, 9, '2023-06-25'),
(10, 10, 10, '2023-07-04');

INSERT INTO Adhesions (AdhesionID, MembreID, DateDebut, DateFin, Statut)
VALUES 
(1, 1, '2022-11-01', '2023-11-01', True),
(2, 2, '2022-12-15', '2023-12-15', TRUE),
(3, 3, '2023-01-10', '2024-01-10', False),
(4, 4, '2023-02-20', '2024-02-19', False),
(5, 5, '2023-03-15', '2024-03-14', TRUE),
(6, 6, '2023-04-01', '2024-04-01', False),
(7, 7, '2023-05-05', '2024-05-04', TRUE),
(8, 8, '2023-06-20', '2024-06-19', False),
(9, 9, '2023-07-25', '2024-07-24', False),
(10, 10, '2023-08-30', '2024-08-29', TRUE);



