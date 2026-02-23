#!/bin/bash
# Script de nettoyage pour résoudre les problèmes de fichiers verrouillés
# Auteur: JUNGLING Pierre
# Date: 2026

echo "Nettoyage des fichiers de build et arrêt des processus..."

# Arrêter tous les processus dotnet liés à RefugeAnimaux
echo "Arrêt des processus RefugeAnimaux..."
pkill -f "RefugeAnimaux" 2>/dev/null || true
pkill -f "dotnet.*RefugeAnimaux" 2>/dev/null || true

# Attendre un peu pour que les processus se terminent
sleep 1

# Nettoyer les dossiers de build
echo "Nettoyage des dossiers bin et obj..."
rm -rf bin/ obj/ 2>/dev/null || true

# Nettoyer avec dotnet clean
echo "Nettoyage avec dotnet clean..."
dotnet clean 2>/dev/null || true

echo "Nettoyage terminé !"
echo "Vous pouvez maintenant reconstruire le projet avec 'dotnet build'"
