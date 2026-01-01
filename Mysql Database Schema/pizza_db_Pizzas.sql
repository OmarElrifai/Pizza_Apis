-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: pizza_db
-- ------------------------------------------------------
-- Server version	8.4.0

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

--
-- Table structure for table `Pizzas`
--

DROP TABLE IF EXISTS `Pizzas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Pizzas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  `IsExtraCheese` bit(1) DEFAULT NULL,
  `IsGlutenFree` bit(1) DEFAULT NULL,
  `Shop` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Shop_idx` (`Shop`),
  CONSTRAINT `Shop` FOREIGN KEY (`Shop`) REFERENCES `Shops` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Pizzas`
--

LOCK TABLES `Pizzas` WRITE;
/*!40000 ALTER TABLE `Pizzas` DISABLE KEYS */;
INSERT INTO `Pizzas` VALUES (2,'Sea Food',_binary '',_binary '\0',1),(3,'Sea Ranch',_binary '\0',_binary '',2),(4,'Smokey Duck Gluten free',_binary '',_binary '',2),(8,'Smokey Duck Gluten free',_binary '',_binary '',NULL),(9,'Peporni',_binary '',_binary '',NULL),(10,'vegetarian',_binary '',_binary '',NULL),(12,'Mixed Cheese',_binary '',_binary '',NULL),(13,'Shawerma',_binary '',_binary '',2),(14,'nutella',_binary '\0',_binary '\0',NULL),(16,'lutas',_binary '\0',_binary '\0',1),(17,'susage',_binary '\0',_binary '\0',3),(19,'hot dog',_binary '\0',_binary '\0',4),(20,'hot dog5',_binary '\0',_binary '\0',9),(21,'Cheese super supreme',_binary '\0',_binary '\0',9),(22,'Cheese super supreme',_binary '\0',_binary '\0',NULL),(23,'Cheese supreme',_binary '\0',_binary '\0',10),(25,'Chicken supreme',_binary '\0',_binary '\0',NULL),(26,'Sea Ranch',_binary '',_binary '\0',NULL),(27,'Sea Ranch',_binary '',_binary '\0',1),(28,'Sea Ranch',_binary '',_binary '\0',1),(29,'Sea Ranch',_binary '',_binary '\0',1),(30,'Sea Ranch1',_binary '',_binary '\0',1),(31,'Sea Ranch',_binary '',_binary '\0',NULL),(32,'Sea Ranch',_binary '',_binary '\0',11);
/*!40000 ALTER TABLE `Pizzas` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-01-01 13:31:57
