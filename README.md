# Real Estate Listings Web Scraping Project

## Purpose

The goal of this project is to perform web scraping on real estate listing websites. This is primarily an educational project, aimed at refreshing and enhancing development skills by using various tools and technologies.

## Why

This project is designed to:
- Conduct practical development tests.
- Explore and learn to use advanced tools, even if they might seem overkill for the task.
- Gain a better understanding of technologies such as Docker, RabbitMQ, Selenium, and SQL Server.

## Tools Used

- **Docker**: To containerize the application and ensure consistent execution across different machines.
- **RabbitMQ**: Used as a message broker to manage communication between different parts of the application.
- **Selenium**: To automate navigation and data extraction from real estate listing websites.
- **SQL Server**: Database used to store the extracted data.

### Overview

The project is structured to utilize Docker for container management, RabbitMQ for messaging, Selenium for web scraping automation, and SQL Server for data storage.

## Usage

The project is designed to be run using Docker Compose, which sets up the necessary containers for RabbitMQ, Selenium, and SQL Server with an initialization script (`init.sql`) for the database. 

- The console application can start the web scraping process.
- Alternatively, a Windows service (`FetchAnnouncementWorkerService`) can be installed to automate the scraping process.
- The web application allows users to view the scraped results.

## Installation and Setup

While the project can be set up and run locally, the primary purpose is to review the code and understand the architecture and implementation. The `docker-compose.yml` file orchestrates the setup of RabbitMQ, Selenium, and SQL Server, and the `init.sql` file initializes the database with necessary data.

## Contact

If you need more information, feel free to contact me.

---

# Projet de Web Scraping d'Annonces Immobilières

## But

Ce projet a pour objectif de réaliser du web scraping sur des sites d'annonces immobilières. Il s'agit avant tout d'un projet éducatif, permettant de se remettre à niveau en développement logiciel en utilisant divers outils et technologies.

## Pourquoi

Ce projet est conçu pour :
- Faire des tests pratiques en développement.
- Explorer et apprendre l'utilisation d'outils avancés, même si ceux-ci peuvent sembler surdimensionnés pour la tâche.
- Acquérir une meilleure compréhension des technologies Docker, RabbitMQ, Selenium et SQL Server.

## Outils Utilisés

- **Docker** : Pour containeriser l'application et assurer une exécution cohérente sur différentes machines.
- **RabbitMQ** : Utilisé comme broker de messages pour gérer la communication entre les différentes parties de l'application.
- **Selenium** : Pour automatiser la navigation et l'extraction des données depuis les sites web d'annonces immobilières.
- **SQL Server** : Base de données utilisée pour stocker les données extraites.


### Vue d'Ensemble

Le projet est structuré pour utiliser Docker pour la gestion des conteneurs, RabbitMQ pour la messagerie, Selenium pour l'automatisation du web scraping, et SQL Server pour le stockage des données.

## Utilisation

Le projet est conçu pour être exécuté à l'aide de Docker Compose, qui configure les conteneurs nécessaires pour RabbitMQ, Selenium et SQL Server avec un script d'initialisation (`init.sql`) pour la base de données.

- L'application console peut démarrer le processus de web scraping.
- Alternativement, un service Windows (`FetchAnnouncementWorkerService`) peut être installé pour automatiser le processus de scraping.
- L'application web permet aux utilisateurs de visualiser les résultats récupérés.

## Installation et Configuration

Bien que le projet puisse être configuré et exécuté localement, le but principal est de consulter le code et de comprendre l'architecture et l'implémentation. Le fichier `docker-compose.yml` orchestre la configuration de RabbitMQ, Selenium et SQL Server, et le fichier `init.sql` initialise la base de données avec les données nécessaires.


## Contact

Si vous avez besoin de plus d'informations, n'hésitez pas à me contacter.
