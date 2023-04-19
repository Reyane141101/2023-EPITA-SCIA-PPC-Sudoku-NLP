# Résolveur de Sudoku utilisant la librairie Z3

## Authors

Adrien GIGET

Ethan MACHAVOINE

Jonathan POELGER


Vous devrez également inclure un fichier Readme.md dans votre projet expliquant 
votre approche et présentant les résultats obtenus lors des tests de performance 
(attention à ne pas modifier le readme du répertoire principal). Vous pouvez 
utiliser le projet Sudoku.Benchmark pour effectuer ces tests de performance et 
mesurer le temps d'exécution de votre solver sur différentes grilles de Sudoku.

## Introduction

Ce projet présente différentes implémentations d'un solveur de Sudoku en utilisant 
la bibliothèque de SMT solver Z3 de Microsoft. Nous avons discuté et implémenté 
plusieurs approches pour résoudre des grilles de Sudoku en utilisant Z3, 
notamment les implémentations suivantes :

* Z3Int
* Z3BitVector

Chaque implémentation utilise des techniques différentes pour définir et résoudre
les contraintes de Sudoku.

## Approches

### Z3Int

Cette approche utilise les entiers Z3 pour représenter les variables des cellules 
du Sudoku. Les contraintes génériques et les contraintes spécifiques à la grille 
sont définies en utilisant les expressions booléennes et les expressions 
arithmétiques de Z3. Cette méthode est assez simple et directe, mais elle peut ne 
pas être aussi efficace que d'autres approches en termes de performances et 
d'utilisation des ressources.

### Z3BitVector

Cette approche utilise les BitVecs pour représenter les variables des cellules du 
Sudoku. Les contraintes génériques et les contraintes spécifiques à la grille sont
définies en utilisant les expressions booléennes de Z3. Mais étant codé sur 4 bits, 
suffisant pour exprimer les chiffres de 0 à 9 d'un soduku, cela permet d'accélérer 
(parfois) le traitement.

## Déclinaison avec les propriétées de Z3

* ReusableScope
* ReusableHypothesis
* Substitution
* Tactics

### ReusableScope

Cette approche utilise un contexte réutilisable avec la méthode Push() et Pop()
pour gérer efficacement les contraintes lors de la résolution du Sudoku. Cela permet
de mieux gérer les ressources et d'améliorer les performances en ne recalculant
pas les contraintes à chaque itération.

### ReusableHypothesis

Dans cette approche, on crée une hypothèse réutilisable pour les contraintes de
base du Sudoku, ce qui permet de réduire les calculs redondants et d'améliorer
les performances. Les contraintes spécifiques à la grille sont ajoutées au fur
et à mesure, et l'hypothèse est utilisée pour vérifier la validité de la grille.

### Substitution

Cette méthode exploite la substitution pour remplacer les variables par les 
valeurs réelles du Sudoku, réduisant ainsi la complexité des contraintes et 
améliorant les performances du solveur.

### Tactics

L'approche des tactiques permet d'utiliser les tactiques Z3 pour guider la 
résolution du problème du Sudoku. Les tactiques sont des heuristiques qui 
peuvent être appliquées pour simplifier et résoudre les problèmes logiques. 
En combinant plusieurs tactiques, cette méthode peut offrir une résolution 
plus rapide et efficace du Sudoku.


## Tests de performance

Les performances de chaque implémentation ont été testées sur plusieurs 
grilles de Sudoku de niveaux de difficulté "Medium". Les temps d'exécution
et l'utilisation des ressources ont été comparés pour chaque implémentation.

* Z3Int
    * Base : 2 811ms
    * ReusableScope : 6 467ms
    * ReusableHypothesis : 6 148ms
    * Substitution : 2 481ms

* Z3BitVector
    * Base : 622ms
    * ReusableScope : 1 463ms
    * ReusableHypothesis : 1 167ms
    * Substitution : 513ms
    * Tactics : 1 118ms


Les temps d'exécution peuvent varier en fonction de la machine et des 
grilles testées.


## Conclusion

En conclusion, ce projet montre comment utiliser la bibliothèque Z3 pour 
résoudre des grilles de Sudoku en utilisant différentes approches. Les 
implémentations présentées offrent des performances variables en fonction 
des tactiques et des optimisations utilisées. Les tests de performance 
montrent que l'utilisation d'une portée réutilisable et de tactiques Z3 
peut avoir un impact sur les performances et l'utilisation des ressources 
pour résoudre des grilles de Sudoku.