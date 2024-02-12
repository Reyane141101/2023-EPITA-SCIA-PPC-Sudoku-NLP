# Résolveur de Sudoku en utilisant Z3 avec Linq

## Auteurs

Reyane En-nabty

#
## Introduction

Ce projet présente quelques solveurs de Sudoku en utilisant Z3 avec Linq.\
Voici les différentes méthodes utilisées pour résoudre le Sudoku:
- LinqToZ3GridZ3bitVector
- LinqToZ3SolveurIntSubstitution


Chaque méthode possède un fichier pour la résolution d'un Sudoku.

#
### LinqToZ3GridZ3BitVector
Ce solveur utilise les fonctionnalités Z3 bas niveau notamment les vectors de bits tout en passant par la librairie Z3 LinqBinding qui est une librairie haut niveaux pour utiliser les fonctionnalités de Z3. L'implémentation de l'utilisation des vectors de bytes est faite à la ligne 725 et 875 du fichier Theorem.cs

#
### LinqToZ3IntSubstitution
Dans cette partie la nous cherchons à utiliser l'api de substituion déja pour les int en implémentant un switch dans le le fichier theorem de la libraire linq to Z3 pour discerner les types de substitution de nos contraintes que ce soit des constantes ou non. Malheuresement, j'ai trouvé ou implémenter le switch masi pour discerné le type de mes contraintes je n'ai pas trouver la variable associé ou la méthode associés. (cf ligne 418 à 429 du fichier Theorem.cs)