# Rubber Ducky Protection

Ce programme en C# a pour but de détecter et déconnecter automatiquement tout nouveau périphérique de type clavier HID (Human Interface Device) connecté à votre ordinateur. Cela permet de se prémunir contre les attaques par "Rubber Ducky", qui consistent à injecter des commandes malveillantes via un clavier USB déguisé en périphérique de stockage.

## Fonctionnalités

- Détection en temps réel de nouveaux périphériques HID connectés
- Déconnexion automatique des nouveaux périphériques HID non approuvés
- Interface graphique avec une icône dans la barre des tâches
- Possibilité d'ajouter des périphériques HID à une liste de confiance

## Utilisation

1. Exécutez le programme
2. Une icône apparaîtra dans la barre des tâches
3. Si un nouveau périphérique HID est connecté, il sera automatiquement déconnecté et une notification s'affichera
4. Pour approuver un périphérique HID, cliquez avec le bouton droit sur l'icône et sélectionnez "Ajouter les HID actuels à la liste de confiance"
5. Débranchez et rebranchez le périphérique HID à approuver
6. Le périphérique sera ajouté à la liste de confiance et ne sera plus déconnecté

## Configuration

Le programme stocke la liste des périphériques HID approuvés dans un fichier `trusted_hid_devices` situé dans le dossier `%AppData%` de l'utilisateur actuel.

## Remarques

- Ce programme nécessite des privilèges d'administrateur pour pouvoir déconnecter les périphériques
- Il est recommandé d'exécuter ce programme en permanence pour une protection continue

## Contribution

Les contributions pour améliorer ce programme sont les bienvenues. N'hésitez pas à soumettre des pull requests ou à signaler des problèmes.
