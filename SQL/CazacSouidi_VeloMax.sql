DROP DATABASE IF EXISTS VeloMax;
CREATE DATABASE IF NOT EXISTS `VeloMax` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `VeloMax`;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `Fidelio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Fidelio` (
   `noProg` INT AUTO_INCREMENT,
   `description` VARCHAR(50) DEFAULT NULL,
   `cout` INT DEFAULT NULL,
   `duree` INT DEFAULT NULL,
   `Rabais` DECIMAL(15,2) DEFAULT NULL,
   PRIMARY KEY(`noProg`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `LigneProduit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LigneProduit` (
   `idProduit` INT NOT NULL AUTO_INCREMENT,
   `nomLigne` VARCHAR(50) NOT NULL,
   PRIMARY KEY(`idProduit`, `nomLigne`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Fournisseur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Fournisseur` (
   `idFournisseur` INT NOT NULL AUTO_INCREMENT,
   `siret` VARCHAR(50) DEFAULT NULL,
   `nomEntreprise` VARCHAR(50) DEFAULT NULL,
   `numTelephone` VARCHAR(50) DEFAULT NULL,
   `courriel` VARCHAR(50) DEFAULT NULL,
   `libelle` VARCHAR(50) DEFAULT NULL,
   PRIMARY KEY(`idFournisseur`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Piece_detachee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Piece_detachee` (
   `numDeProdPiece` INT NOT NULL AUTO_INCREMENT,
   `numCategorie` INT DEFAULT NULL,
   `description` VARCHAR(50) DEFAULT NULL,
   `prixUnitairePiece` INT DEFAULT NULL,
   `delaisDAppro` INT DEFAULT NULL,
   `dateIntroPiece` DATE DEFAULT NULL,
   `dateDisProdPiece` DATE DEFAULT NULL,
   PRIMARY KEY(`numDeProdPiece`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Client` (
   `idClient` INT NOT NULL AUTO_INCREMENT,
   `nomCompagnie` VARCHAR(50) DEFAULT NULL,
   `numTelephone` VARCHAR(50) DEFAULT NULL,
   `courriel` VARCHAR(50) DEFAULT NULL,
   `nomPersonne` VARCHAR(50) DEFAULT NULL,
   `numRue` INT DEFAULT NULL,
   `rue` VARCHAR(50) DEFAULT NULL,
   `ville` VARCHAR(50) DEFAULT NULL,
   `code_postal` INT DEFAULT NULL,
   `pays` VARCHAR(50) DEFAULT NULL,
   PRIMARY KEY(`idClient`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;



DROP TABLE IF EXISTS `Magasin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Magasin` (
   `idMagasin` INT NOT NULL AUTO_INCREMENT,
   `nom` VARCHAR(50) DEFAULT NULL,
   `chiffre_affaire` DECIMAL(15,2) DEFAULT NULL,
   `numRue` INT DEFAULT NULL,
   `rue` VARCHAR(50) DEFAULT NULL,
   `ville` VARCHAR(50) DEFAULT NULL,
   `code_postal` VARCHAR(50) DEFAULT NULL,
   `pays` VARCHAR(50) DEFAULT NULL,
   PRIMARY KEY(`idMagasin`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


DROP TABLE IF EXISTS `Gerant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Gerant` (
   `idGerant` INT NOT NULL AUTO_INCREMENT,
   `nom` VARCHAR(50) DEFAULT NULL,
   `prenom` VARCHAR(50) DEFAULT NULL,
   `salaireFixe` DECIMAL(15,2) DEFAULT NULL,
   `prime` DECIMAL(15, 2) DEFAULT NULL,
   `idMagasin` INT NOT NULL,
   PRIMARY KEY(`idGerant`),
   UNIQUE INDEX `gerantMagasinExiste`(`idMagasin`),
   CONSTRAINT `fk_gerant_magasin` FOREIGN KEY(`idMagasin`) REFERENCES `magasin`(`idMagasin`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Vendeur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vendeur` (
   `idVendeur` INT NOT NULL AUTO_INCREMENT,
   `nom` VARCHAR(50) DEFAULT NULL,
   `prenom` VARCHAR(50) DEFAULT NULL,
   `salaireFixe` DECIMAL(15, 2) DEFAULT NULL,
   `prime` DECIMAL(15, 2) DEFAULT NULL,
   `idMagasin` INT NOT NULL,
   PRIMARY KEY(`idVendeur`),
   CONSTRAINT `fk_Vendeur_Magasin` FOREIGN KEY(`idMagasin`) REFERENCES `Magasin`(`idMagasin`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Velo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Velo` (
   `numDeProdVelo` INT NOT NULL AUTO_INCREMENT,
   `nomVelo` VARCHAR(50) DEFAULT NULL,
   `marque` VARCHAR(50) DEFAULT NULL,
   `prixUnitaireVelo` DECIMAL(15, 2) DEFAULT NULL,
   `dateIntroVelo` DATE DEFAULT NULL,
   `dateDisProdVelo` DATE DEFAULT NULL,
   `grandeur` VARCHAR(50) DEFAULT NULL,
   `idProduit` INT NOT NULL,
   PRIMARY KEY(`numDeProdVelo`),
   FOREIGN KEY(`idProduit`) REFERENCES `LigneProduit`(`idProduit`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Commande` (
   `numeroDeCommande` INT NOT NULL AUTO_INCREMENT,
   `dateDeCommande` DATE DEFAULT NULL,
   `dateLivraison` DATE DEFAULT NULL,
   `numRue` INT DEFAULT NULL,
   `rue` VARCHAR(50) DEFAULT NULL,
   `ville` VARCHAR(50) DEFAULT NULL,
   `code_postal` INT DEFAULT NULL,
   `pays` VARCHAR(50) DEFAULT NULL,
   `idClient` INT NOT NULL,
   `idVendeur` INT NOT NULL,
   PRIMARY KEY(`numeroDeCommande`),
   FOREIGN KEY(`idClient`) REFERENCES `Client`(`idClient`),
   FOREIGN KEY(`idVendeur`) REFERENCES `Vendeur`(`idVendeur`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `Fournit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Fournit` (
   `numDeProdPiece` INT NOT NULL ,
   `idFournisseur` INT NOT NULL ,
   PRIMARY KEY(`numDeProdPiece`, `idFournisseur`),
   FOREIGN KEY(`numDeProdPiece`) REFERENCES `Piece_detachee`(`numDeProdPiece`),
   FOREIGN KEY(`idFournisseur`) REFERENCES `Fournisseur`(`idFournisseur`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

CREATE TABLE `passe_velo` (
   `numDeProdVelo` INT NOT NULL,
   `numeroDeCommande` INT NOT NULL,
   `quantite_V` INT DEFAULT NULL,
   PRIMARY KEY(`numDeProdVelo`, `numeroDeCommande`),
   FOREIGN KEY(`numDeProdVelo`) REFERENCES `Velo`(`numDeProdVelo`),
   FOREIGN KEY(`numeroDeCommande`) REFERENCES `Commande`(`numeroDeCommande`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE TABLE `passe_piece` (
   `numDeProdPiece` INT NOT NULL,
   `numeroDeCommande` INT NOT NULL,
   `quantite_P` INT DEFAULT NULL,
   PRIMARY KEY(`numDeProdPiece`, `numeroDeCommande`),
   FOREIGN KEY(`numDeProdPiece`) REFERENCES `Piece_detachee`(`numDeProdPiece`),
   FOREIGN KEY(`numeroDeCommande`) REFERENCES `Commande`(`numeroDeCommande`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE TABLE `est_compose` (
   `numDeProdVelo` INT NOT NULL,
   `numDeProdPiece` INT NOT NULL,
   PRIMARY KEY(`numDeProdVelo`, `numDeProdPiece`),
   FOREIGN KEY(`numDeProdVelo`) REFERENCES `Velo`(`numDeProdVelo`),
   FOREIGN KEY(`numDeProdPiece`) REFERENCES `Piece_detachee`(`numDeProdPiece`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE TABLE `stock_piece`(
   `idMagasin` INT,
   `numDeProdPiece` INT,
   `quantite_stock` INT, 
   PRIMARY KEY (`idMagasin`, `numDeProdPiece`),
   FOREIGN KEY (`idMagasin`) REFERENCES `magasin`(`idMagasin`),
   FOREIGN KEY (`numDeProdPiece`) REFERENCES `Piece_detachee`(`numDeProdPiece`)
)ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;


CREATE TABLE `stock_velo`(
   `idMagasin` INT,
   `numDeProdVelo`INT, 
   `quantite_stock`INT, 
   PRIMARY KEY (`idMagasin`,`numDeProdVelo`),
   FOREIGN KEY (`idMagasin`) REFERENCES `magasin`(`idMagasin`),
   FOREIGN KEY (`numDeProdVelo`) REFERENCES `Velo`(`numDeProdVelo`)
)ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE TABLE `adhere`(
   `idClient` INT,
   `noProg`INT, 
   `date_debut` DATE, 
   PRIMARY KEY (`idClient`,`noProg`),
   FOREIGN KEY (`idClient`) REFERENCES `client`(`idClient`),
   FOREIGN KEY (`noProg`) REFERENCES `Fidelio`(`noProg`)
)ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

-- Creation de l'utilisateur bozo
DROP USER IF EXISTS bozo@localhost;
CREATE USER bozo@localhost IDENTIFIED BY 'bozo';
GRANT SELECT ON * . * TO bozo@localhost;
FLUSH PRIVILEGES;

-- INSERTION DES DONNEES
INSERT INTO ligneproduit (`nomLigne`) VALUES ('BMX');
INSERT INTO ligneproduit (`nomLigne`) VALUES ('Classique');
INSERT INTO ligneproduit (`nomLigne`) VALUES ('Velo de course');
INSERT INTO ligneproduit (`nomLigne`) VALUES ('VTT');


INSERT INTO Fidelio (description, cout, duree, Rabais) VALUES ('Fidélio', 15, 1, 5.00);
INSERT INTO Fidelio (description, cout, duree, Rabais) VALUES ('Fidélio Or', 25, 2, 8.00);
INSERT INTO Fidelio (description, cout, duree, Rabais) VALUES ('Fidélio Platine', 60, 2, 10.00);
INSERT INTO Fidelio (description, cout, duree, Rabais) VALUES ('Fidélio Max', 100, 3, 12.00);

-- Insertion de 3 magasins
INSERT INTO Magasin (nom, chiffre_affaire, numRue, rue, ville, code_postal, pays) 
VALUES 
('Magasin A', 15000.00, 123, 'Rue de la Liberté', 'Paris', '75001', 'France'),
('Magasin B', 20000.00, 456, 'Avenue des Champs-Élysées', 'Paris', '75008', 'France'),
('Magasin C', 18000.00, 789, 'Boulevard Saint-Germain', 'Paris', '75006', 'France');


    -- Insertion de 3 fournisseurs
INSERT INTO Fournisseur (siret, nomEntreprise, numTelephone, courriel, libelle) 
VALUES 
('123456789', 'Fournisseur A', '123-456-7890', 'fournisseurA@example.com', 'Tres Bon'),
('987654321', 'Fournisseur B', '987-654-3210', 'fournisseurB@example.com', 'Bon'),
('456789123', 'Fournisseur C', '456-789-1230', 'fournisseurC@example.com', 'Moyen'),
('01239812', 'Fournisseur D', '456-789-1230', 'fournisseurD@example.com', 'Mauvais');

INSERT INTO Gerant (nom, prenom, salaireFixe, prime, idMagasin) 
VALUES 
('Jacque', 'Jean', 3000.00, 500.00, 1), -- Gérant du Magasin A
('Lou', 'Marie', 3200.00, 600.00, 2), -- Gérant du Magasin B
('Caillou', 'Pierre', 2800.00, 550.00, 3); -- Gérant du Magasin C

INSERT INTO Vendeur (nom, prenom, salaireFixe, prime, idMagasin) 
VALUES 
('Vendeur A', 'Aurélien', 2000.00, 300.00, 1), -- Vendeur du Magasin A
('Vendeuse B', 'Béatrice', 2200.00, 350.00, 1), -- Vendeur du Magasin A
('Vendeur C', 'Cédric', 1800.00, 280.00, 2), -- Vendeur du Magasin B
('Vendeuse D', 'Danielle', 1900.00, 320.00, 2), -- Vendeur du Magasin B
('Vendeur E', 'Étienne', 2100.00, 330.00, 3), -- Vendeur du Magasin C
('Vendeuse F', 'Fabienne', 2300.00, 370.00, 3); -- Vendeur du Magasin C

INSERT INTO Velo (nomVelo, marque, prixUnitaireVelo, dateIntroVelo, dateDisProdVelo, grandeur, idProduit) 
VALUES 
('Vélo A', 'Giant', 500.00, '2024-04-25', '2025-04-25', 'Adultes', 1), -- Vélo de la ligne BMX
('Vélo B', 'Giant', 600.00, '2024-04-25', '2025-04-25', 'Jeunes', 2), -- Vélo de la ligne Classique
('Vélo C', 'Giant', 700.00, '2024-04-25', '2025-04-25', 'Dames', 3), -- Vélo de la ligne Vélo de course
('Vélo D', 'BMC', 800.00, '2024-04-25', '2025-04-25', 'Filles', 1), -- Vélo de la ligne BMX
('Vélo E', 'BMC', 900.00, '2024-04-25', '2025-04-25', 'Garçons', 2), -- Vélo de la ligne Classique
('Vélo F', 'Trek', 1000.00, '2024-04-25', '2025-04-25', 'Hommes', 3); -- Vélo de la ligne VTT

INSERT INTO Client (nomCompagnie, nomPersonne, numTelephone, courriel, numRue, rue, ville, code_postal, pays) 
VALUES 
('Société A', NULL, '1234567890', 'client1@example.com', 1, 'Rue de la Liberté', 'Paris', '75001', 'France'),
(NULL, 'John Doe', '0987654321', 'john.doe@example.com', 2, 'Avenue des Champs-Élysées', 'Paris', '75008', 'France'),
('Entreprise B Ltd.', NULL, '2345678901', 'client2@example.com', 3, 'Boulevard Saint-Germain', 'Paris', '75006', 'France'),
(NULL, 'Jane Smith', '9876543210', 'jane.smith@example.com', 4, 'Rue de Rivoli', 'Paris', '75004', 'France'),
(NULL, 'Alice', '3456789012', 'alice@example.com', 5, 'Quai des Orfèvres', 'Paris', '75001', 'France'),
('Compagnie C', NULL, '8765432109', 'client3@example.com', 6, 'Avenue Montaigne', 'Paris', '75008', 'France'),
('Client A Inc.','Jean Dupont', '123-456-7890', 'clientA@example.com',  100, 'Avenue des Roses', 'Paris', '75001', 'France'),
('Société B', 'Marie Martin','987-654-3210', 'clientB@example.com',  200, 'Rue de la Paix', 'Marseille', '13001', 'France'),
('LDV Ltd.','Pierre Durand', '456-789-1230', 'clientC@example.com', 300, 'Boulevard Voltaire', 'Lyon', '69001', 'France'),
(NULL, 'Sophie', '8901234567', 'sophie@example.com', 13, 'Rue Royale', 'Paris', '75008', 'France');

INSERT INTO Piece_detachee (numCategorie, description, prixUnitairePiece, delaisDAppro, dateIntroPiece, dateDisProdPiece) 
VALUES 
(1, 'Cadre en aluminium', 100.00, 10, '2024-04-25', '2025-04-25'),
(1, 'Cadre en acier', 80.00, 10, '2024-04-25', '2025-04-25'),
(1, 'Cadre en carbone', 150.00, 10, '2024-04-25', '2025-04-25'),
(2, 'Guidon de course en aluminium', 50.00, 7, '2024-04-25', '2025-04-25'),
(2, 'Guidon de VTT en carbone', 70.00, 7, '2024-04-25', '2025-04-25'),
(2, 'Guidon de ville en acier', 40.00, 7, '2024-04-25', '2025-04-25'),
(3, 'Frein avant', 50.00, 7, '2024-04-25', '2025-04-25' ),
(4, 'Selle de course', 40.00, 7, '2024-04-25', '2025-04-25'),
(4, 'Selle de VTT', 45.00, 7, '2024-04-25', '2025-04-25'),
(4, 'Selle de ville', 35.00, 7, '2024-04-25', '2025-04-25'),
(5, 'Dérailleur avant Shimano', 60.00, 7, '2024-04-25', '2025-04-25'),
(6, 'Dérailleur arrière Shimano', 80.00, 7, '2024-04-25', '2025-04-25'),
(7, 'Roue avant standard', 80.00, 7, '2024-04-25', '2025-04-25'),
(7, 'Roue avant de VTT', 90.00, 7, '2024-04-25', '2025-04-25'),
(7, 'Roue avant de route', 85.00, 7, '2024-04-25', '2025-04-25'),
(8, 'Roue arrière standard', 80.00, 7, '2024-04-25', '2025-04-25'),
(8, 'Roue arrière de VTT', 90.00, 7, '2024-04-25', '2025-04-25'),
(8, 'Roue arrière de route', 85.00, 7, '2024-04-25', '2025-04-25'),
(9, 'Réflecteurs', 10.00, 3, '2024-04-25', '2025-04-25'),
(10, 'Pédalier', 30.00, 7, '2024-04-25', '2025-04-25'),
(11, 'Panier', 20.00, 5, '2024-04-25', '2025-04-25');

INSERT INTO Commande (dateDeCommande, dateLivraison, numRue, rue, ville, code_postal, pays, idClient, idVendeur) 
VALUES 
('2024-05-01', '2024-05-06', 123, 'Rue de la Liberté', 'Paris', 75001, 'France', 1, 1),
('2024-05-02', '2024-05-07', 123, 'Rue de la Liberté', 'Paris', 75001, 'France', 1, 2),
('2024-04-27', '2024-05-02', 789, 'Boulevard Saint-Germain', 'Paris', 75006, 'France', 3, 5),
('2024-04-28', '2024-05-03', 101, 'Rue de Rivoli', 'Paris', 75004, 'France', 4, 2),
('2024-04-29', '2024-05-04', 202, 'Rue de la Paix', 'Marseille', 13001, 'France', 5, 4),
('2024-04-30', '2024-05-05', 303, 'Boulevard Voltaire', 'Lyon', 69001, 'France', 6, 6);
-- Insertion dans la table Fournit
INSERT INTO
    Fournit (numDeProdPiece, idFournisseur)
VALUES (1, 1), -- Cadre en aluminium fourni par Fournisseur A
    (2, 2), -- Cadre en acier fourni par Fournisseur B
    (3, 3), -- Cadre en carbone fourni par Fournisseur C
    (4, 1), -- Cadre de vélo de montagne fourni par Fournisseur A
    (5, 2), -- Cadre de vélo de route fourni par Fournisseur B
    (6, 3), -- Guidon de course en aluminium fourni par Fournisseur C
    (7, 1), -- Guidon de VTT en carbone fourni par Fournisseur A
    (8, 2), -- Guidon de ville en acier fourni par Fournisseur B
    (9, 3), -- Guidon de route en aluminium fourni par Fournisseur C
    (10, 1), -- Guidon de vélo de montagne en aluminium fourni par Fournisseur A
    (11, 2), -- Frein avant fourni par Fournisseur B
    (12, 3), -- Selle de course fournie par Fournisseur C
    (13, 1), -- Selle de VTT fournie par Fournisseur A
    (14, 2), -- Selle de ville fournie par Fournisseur B
    (15, 3), -- Selle de route fournie par Fournisseur C
    (16, 1), -- Selle ergonomique fournie par Fournisseur A
    (17, 2), -- Dérailleur avant Shimano fourni par Fournisseur B
    (18, 3), -- Dérailleur arrière Shimano fourni par Fournisseur C
    (19, 1), -- Roue avant standard fournie par Fournisseur A
    (20, 2); -- Roue avant de VTT fournie par Fournisseur B

-- Pédalier fourni par Fournisseur C
-- Insertion dans la table est_compose (association entre vélos et pièces détachées)
-- Voici une suggestion, vous pouvez ajuster les associations selon vos besoins
INSERT INTO
    est_compose (numDeProdVelo, numDeProdPiece)
VALUES (1, 1), -- Vélo A composé du Cadre en aluminium
    (1, 11), -- Vélo A composé du Frein avant
    (1, 19), -- Vélo A composé de la Roue avant standard
    (1, 18), -- Vélo A composé de la Roue arrière de course
    (2, 2), -- Vélo B composé du Cadre en acier
    (2, 11), -- Vélo B composé du Frein avant
    (2, 20), -- Vélo B composé de la Roue avant de VTT
    (2, 15), -- Vélo B composé de la Roue arrière de vélo de montagne
    (3, 3), -- Vélo C composé du Cadre en carbone
    (3, 11), -- Vélo C composé du Frein avant
    (3, 2), -- Vélo C composé de la Roue avant de route
    (3, 6), -- Vélo C composé de la Roue arrière de route
    (4, 4), -- Vélo D composé du Cadre de vélo de montagne
    (4, 11), -- Vélo D composé du Frein avant
    (4, 7), -- Vélo D composé de la Roue avant standard
    (4, 8), -- Vélo D composé de la Roue arrière de course
    (5, 5), -- Vélo E composé du Cadre de vélo de route
    (5, 11), -- Vélo E composé du Frein avant
    (5, 20), -- Vélo E composé de la Roue avant de VTT
    (5, 7), -- Vélo E composé de la Roue arrière de vélo de montagne
    (6, 6), -- Vélo F composé du Guidon de course en aluminium
    (6, 11), -- Vélo F composé du Frein avant
    (6, 19); -- Vélo F composé de la Roue avant standard
-- Vélo F composé de la Roue arrière de route
-- Insertion dans la table stock_piece
-- Voici une suggestion, vous pouvez ajuster les stocks selon vos besoins
INSERT INTO
    stock_piece (
        idMagasin, numDeProdPiece, quantite_stock
    )
VALUES (1, 1, 2), -- Magasin A a 50 Cadres en aluminium en stock
    (1, 2, 30), -- Magasin A a 30 Cadres en acier en stock
    (1, 3, 20), -- Magasin A a 20 Cadres en carbone en stock
    (1, 4, 40), -- Magasin A a 40 Cadres de vélo de montagne en stock
    (1, 5, 25), -- Magasin A a 25 Cadres de vélo de route en stock
    (2, 6, 50), -- Magasin B a 50 Guidons de course en aluminium en stock
    (2, 7, 0), -- Magasin B a 30 Guidons de VTT en carbone en stock
    (2, 8, 20), -- Magasin B a 20 Guidons de ville en acier en stock
    (2, 9, 40), -- Magasin B a 40 Guidons de route en aluminium en stock
    (2, 10, 25), -- Magasin B a 25 Guidons de vélo de montagne en aluminium en stock
    (3, 11, 50), -- Magasin C a 50 Freins avant en stock
    (3, 12, 30), -- Magasin C a 30 Selles de course en stock
    (3, 13, 20), -- Magasin C a 20 Selles de VTT en stock
    (3, 14, 40), -- Magasin C a 40 Selles de ville en stock
    (3, 15, 25), -- Magasin C a 25 Selles de route en stock
    (1, 16, 50), -- Magasin A a 50 Selles ergonomiques en stock
    (2, 17, 30), -- Magasin B a 30 Dérailleurs avant Shimano en stock
    (2, 18, 20), -- Magasin B a 20 Dérailleurs arrière Shimano en stock
    (2, 19, 1), -- Magasin B a 40 Roues avant standard en stock
    (2, 20, 25); -- Magasin B a 25 Roues avant de VTT en stock

-- Magasin B a 25 Pédaliers en stock
-- Insertion dans la table stock_velo
-- Voici une suggestion, vous pouvez ajuster les stocks selon vos besoins
INSERT INTO
    stock_velo (
        idMagasin, numDeProdVelo, quantite_stock
    )
VALUES (1, 1, 10), -- Magasin A a 10 Vélo A en stock
    (1, 2, 20), -- Magasin A a 20 Vélo B en stock
    (1, 3, 15), -- Magasin A a 15 Vélo C en stock
    (2, 4, 30), -- Magasin B a 30 Vélo D en stock
    (2, 5, 25), -- Magasin B a 25 Vélo E en stock
    (3, 6, 18);
-- Magasin C a 18 Vélo F en stock
-- Insertion dans la table passe_velo (association entre vélos et commandes)
-- Voici une suggestion, vous pouvez ajuster les associations selon vos besoins
INSERT INTO
    passe_velo (
        numDeProdVelo, numeroDeCommande, quantite_V
    )
VALUES (1, 1, 2), -- Vélo A acheté dans la Commande 1, quantité: 2
    (2, 2, 1), -- Vélo B acheté dans la Commande 2, quantité: 1
    (3, 3, 3), -- Vélo C acheté dans la Commande 3, quantité: 3
    (4, 4, 2), -- Vélo D acheté dans la Commande 4, quantité: 2
    (5, 5, 1), -- Vélo E acheté dans la Commande 5, quantité: 1
    (6, 6, 4);
-- Vélo F acheté dans la Commande 6, quantité: 4

-- Insertion dans la table passe_piece (association entre pièces détachées et commandes)
-- Voici une suggestion, vous pouvez ajuster les associations selon vos besoins
INSERT INTO
    passe_piece (
        numDeProdPiece, numeroDeCommande, quantite_P
    )
VALUES (1, 1, 5), -- Cadre en aluminium acheté dans la Commande 1, quantité: 5
    (11, 1, 10), -- Frein avant acheté dans la Commande 1, quantité: 10
    (19, 1, 7), -- Roue avant standard achetée dans la Commande 1, quantité: 7
    (2, 2, 3), -- Cadre en acier acheté dans la Commande 2, quantité: 3
    (11, 2, 5), -- Frein avant acheté dans la Commande 2, quantité: 5
    (20, 2, 4), -- Roue avant de VTT achetée dans la Commande 2, quantité: 4
    (3, 3, 8), -- Cadre en carbone acheté dans la Commande 3, quantité: 8
    (11, 3, 12), -- Frein avant acheté dans la Commande 3, quantité: 12
    (4, 4, 4), -- Cadre de vélo de montagne acheté dans la Commande 4, quantité: 4
    (11, 4, 8), -- Frein avant acheté dans la Commande 4, quantité: 8
    (19, 4, 6), -- Roue avant standard achetée dans la Commande 4, quantité: 6
    (5, 5, 2), -- Cadre de vélo de route acheté dans la Commande 5, quantité: 2
    (11, 5, 4), -- Frein avant acheté dans la Commande 5, quantité: 4
    (20, 5, 3), -- Roue avant de VTT achetée dans la Commande 5, quantité: 3
    (6, 6, 9), -- Guidon de course en aluminium acheté dans la Commande 6, quantité: 9
    (11, 6, 11), -- Frein avant acheté dans la Commande 6, quantité: 11
    (19, 6, 8); -- Roue avant standard achetée dans la Commande 6, quantité: 8

-- Roue arrière de route achetée dans la Commande 6, quantité: 7
-- Insertion dans la table adhere (association entre clients et programme Fidélio)
-- Voici une suggestion, vous pouvez ajuster les associations selon vos besoins
INSERT INTO
    adhere (idClient, noProg, date_debut)
VALUES (1, 1, '2024-01-01'), -- Client 1 adhère au programme Fidélio (Fidélio) à partir du 1er janvier 2024
    (2, 2, '2024-02-15'), -- Client 2 adhère au programme Fidélio Or à partir du 15 février 2024
    (3, 3, '2024-03-20'), -- Client 3 adhère au programme Fidélio Platine à partir du 20 mars 2024
    (4, 1, '2024-01-10'), -- Client 4 adhère au programme Fidélio à partir du 10 janvier 2024
    (5, 2, '2024-02-25'), -- Client 5 adhère au programme Fidélio Or à partir du 25 février 2024
    (6, 3, '2024-03-30');
-- Client 6 adhère au programme Fidélio Platine à partir du 30 mars 2024