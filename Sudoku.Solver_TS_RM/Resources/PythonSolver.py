instance = ((0,0,0,0,9,4,0,3,0),
           (0,0,0,5,1,0,0,0,7),
           (0,8,9,0,0,0,0,4,0),
           (0,0,0,0,0,0,2,0,8),
           (0,6,0,2,0,1,0,5,0),
           (1,0,2,0,0,0,0,0,0),
           (0,7,0,0,0,0,5,2,0),
           (9,0,0,0,6,5,0,0,0),
           (0,4,0,9,7,0,0,0,0))


def affiche(txt):
    for i in range(9):
        for j in range(9):
            print(txt[i*9+j], end="")
            if ((j+1) % 3 == 0):
                print("|", end="")
        if ((i+1) % 3 == 0):
            print("\n___________", end="")
        print('')


def ligne(i, solve, traitement):
    traiter = []
    indice = [] #traitement
    for j in range(9):
        #recherche des chiffres deja traites
        if (len(solve[i][j]) == 1):
            traiter.append(int(solve[i][j][0]))
        else:
            indice.append(j)
    if (len(traiter) == 9):
        #print("error traiter")
        return solve, 1
        # ligne complete
    reste = []
    for j in range(9):
        if (j+1) not in traiter:
            #print ("traiter=", traiter, "i+1=", j+1)
            reste.append(j+1)
    """print("traiter =",traiter)
    print("indice =",indice)
    print("reste =",reste)
    #print("solve =",solve)"""

    #suppression de certains cas
    #print("902 005 403")
    #print("915 000 270")
    for j in indice:
        for elt in range (1,10):
            if (elt in solve[i][j]):
                #print("elt =", elt)
                if elt in traiter:
                    #print("elt2 =", elt)
                    #print("solve[i][j] =", solve[j][i])
                    #print("i=", i, "j=", j, "\n")
                    solve[i][j].remove(elt)
                    #print("b         i=", i, "j=", j, "solve[ji]=", solve[j][i], "elt=", elt, "\n")
    """print("traiter =",traiter)
    print("indice =",indice)
    print("reste =",reste)
    print("solve =",solve)"""

    return solve, traitement



def colonne(i, solve, traitement):
    #nettoie les possibilités inutiles par lignes
    #nettoie les possibilités inutiles par colonnes
    #nettoie les possibilités inutiles par carre
    #ATTENTION, i : colonne, i : ligne
    traiter = []
    indice = [] #traitement
    for j in range(9):
        #recherche des chiffres deja traites
        if (len(solve[j][i]) == 1):
            traiter.append(int(solve[j][i][0]))
        else:
            indice.append(j)
    if (len(traiter) == 9):
        #print("error traiter")
        return solve, 1
        # ligne complete
    reste = []
    for j in range(9):
        if (j+1) not in traiter:
            #print ("traiter=", traiter, "i+1=", j+1)
            reste.append(j+1)
    """print("traiter =",traiter)
    print("indice =",indice)
    print("reste =",reste)
    #print("solve =",solve)"""

    #suppression de certains cas
    #print("902 005 403")
    #print("915 000 270")
    for j in indice:
        for elt in range (1,10):
            if (elt in solve[j][i]):
                #print("elt =", elt)
                if elt in traiter:
                    #print("elt2 =", elt)
                    #print("solve[i][j] =", solve[j][i])
                    #print("i=", i, "j=", j, "\n")
                    solve[j][i].remove(elt)
                    #print("b         i=", i, "j=", j, "solve[ji]=", solve[j][i], "elt=", elt, "\n")
    """print("traiter =",traiter)
    print("indice =",indice)
    print("reste =",reste)
    print("solve =",solve)"""

    return solve, traitement

def traiteLigne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = ligne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1
    return solve, traitement

def traiteColonne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = colonne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1
    """for i in range(9):
        print(solve[0][i], "\n")"""
    return solve, traitement

def resolveur(txt):
    tout=[1,2,3,4,5,6,7,8,9]
    #cas 0, possibilité
    """solve = [[[[]]*9]*9]
    solve2 = [[[]*9]*9]
    print(solve2)
    print(solve)"""
    solve = txt
    #print(solve)
    traitement = 0 #0:bon, 1:fini, 2:error
    solve = traiteLigne(solve, traitement)[0]
    return traiteColonne(solve, traitement)[0]
    while (traitement == 0):
        solve, traitement = traiteLigne(solve, traitement)
        if (traitement == 0):
            solve, traitement = traiteColonne(solve, traitement)
        if (traitement == 0):
            solve, traitement = traiteCarre(solve, traitement)
    #verifie si tout est correcte sur ligne, colonne, carre a faire
    return solve

def main(txt):
    solveur = resolveur(txt)
    return solveur
r = main(instance)
